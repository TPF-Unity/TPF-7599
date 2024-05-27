/* Adapted from https://github.com/vazgriz/DungeonGenerator

Copyright (c) 2019 Ryan Vazquez

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is furnished
to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in al
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Graphs;
using System;
using Unity.AI.Navigation;

[DefaultExecutionOrder(-1)] // This script should be initialized first
public class DungeonGenerator : MonoBehaviour
{
    enum CellType
    {
        None,
        Room,
        Hallway
    }

    class Room
    {
        public RectInt bounds;

        public Room(Vector2Int location, Vector2Int size)
        {
            bounds = new RectInt(location, size);
        }

        public static bool Intersect(Room a, Room b)
        {
            return !((a.bounds.position.x >= (b.bounds.position.x + b.bounds.size.x)) || ((a.bounds.position.x + a.bounds.size.x) <= b.bounds.position.x)
                || (a.bounds.position.y >= (b.bounds.position.y + b.bounds.size.y)) || ((a.bounds.position.y + a.bounds.size.y) <= b.bounds.position.y));
        }
    }

    [SerializeField]
    Vector2Int size;
    [SerializeField]
    int roomCount;
    [SerializeField]
    Vector2Int roomMinSize;
    [SerializeField]
    Vector2Int roomMaxSize;
    [SerializeField]
    GameObject groundPrefab;
    [SerializeField]
    GameObject[] wallPrefabs;
    [SerializeField]
    GameObject[] decorationPrefabs;
    [SerializeField]
    GameObject opponentPrefab;
    [SerializeField]
    GameObject environmentHolder;
    [SerializeField]
    GameObject patrolPointHolder;
    [SerializeField]
    GameObject keySpawnHolder;
    [SerializeField]
    GameObject doorSpawnHolder;
    [SerializeField]
    GameObject[] powerupSpawners;

    public int totalKeys;
    public int totalDoors;

    Grid<CellType> grid;
    List<Room> rooms;
    Delaunay delaunay;
    HashSet<Prim.Edge> selectedEdges;

    // All positions will be multiplied by 4 since our groundPrefab has a size of 4x4
    const int positionMultiplier = 4;

    void Start()
    {
        Random.InitState((int)DateTime.UtcNow.Ticks);
        do
        {
            Generate();
        } while (rooms.Count < 5);
        PlacePrefabs();
        PlacePatrolPoints();
        PlaceSpawnPositions();
        BakeNavmesh();

        // Initialize GameManager after all spawns are set
        GameManager.instance.Initialize();
    }

    void Generate()
    {
        grid = new Grid<CellType>(size, Vector2Int.zero);
        rooms = new List<Room>();

        PlaceRooms();
        Triangulate();
        CreateHallways();
        PathfindHallways();
    }

    void PlaceRooms()
    {
        for (int i = 0; i < roomCount; i++)
        {
            Vector2Int location = new Vector2Int(
                Random.Range(0, size.x),
                Random.Range(0, size.y)
            );

            Vector2Int roomSize = new Vector2Int(
                Random.Range(roomMinSize.x, roomMaxSize.x + 1),
                Random.Range(roomMinSize.y, roomMaxSize.y + 1)
            );

            bool add = true;
            Room newRoom = new Room(location, roomSize);
            Room buffer = new Room(location + new Vector2Int(-1, -1), roomSize + new Vector2Int(2, 2));

            foreach (var room in rooms)
            {
                if (Room.Intersect(room, buffer))
                {
                    add = false;
                    break;
                }
            }

            if (newRoom.bounds.xMin < 0 || newRoom.bounds.xMax >= size.x
                || newRoom.bounds.yMin < 0 || newRoom.bounds.yMax >= size.y)
            {
                add = false;
            }

            if (add)
            {
                rooms.Add(newRoom);

                foreach (var pos in newRoom.bounds.allPositionsWithin)
                {
                    grid[pos] = CellType.Room;
                }
            }
        }
    }

    void Triangulate()
    {
        List<Vertex> vertices = new List<Vertex>();

        foreach (var room in rooms)
        {
            vertices.Add(new Vertex<Room>((Vector2)room.bounds.position + ((Vector2)room.bounds.size) / 2, room));
        }

        delaunay = Delaunay.Triangulate(vertices);
    }

    void CreateHallways()
    {
        List<Prim.Edge> edges = new List<Prim.Edge>();

        foreach (var edge in delaunay.Edges)
        {
            edges.Add(new Prim.Edge(edge.U, edge.V));
        }

        List<Prim.Edge> mst = Prim.MinimumSpanningTree(edges, edges[0].U);

        selectedEdges = new HashSet<Prim.Edge>(mst);
        var remainingEdges = new HashSet<Prim.Edge>(edges);
        remainingEdges.ExceptWith(selectedEdges);

        foreach (var edge in remainingEdges)
        {
            if (Random.value < 0.2)
            {
                selectedEdges.Add(edge);
            }
        }
    }

    void PathfindHallways()
    {
        DungeonPathfinder2D aStar = new DungeonPathfinder2D(size);

        foreach (var edge in selectedEdges)
        {
            var startRoom = (edge.U as Vertex<Room>).Item;
            var endRoom = (edge.V as Vertex<Room>).Item;

            var startPosf = startRoom.bounds.center;
            var endPosf = endRoom.bounds.center;
            var startPos = new Vector2Int((int)startPosf.x, (int)startPosf.y);
            var endPos = new Vector2Int((int)endPosf.x, (int)endPosf.y);

            var path = aStar.FindPath(startPos, endPos, (DungeonPathfinder2D.Node a, DungeonPathfinder2D.Node b) => {
                var pathCost = new DungeonPathfinder2D.PathCost();

                pathCost.cost = Vector2Int.Distance(b.Position, endPos);    //heuristic

                if (grid[b.Position] == CellType.Room)
                {
                    pathCost.cost += 10;
                }
                else if (grid[b.Position] == CellType.None)
                {
                    pathCost.cost += 5;
                }
                else if (grid[b.Position] == CellType.Hallway)
                {
                    pathCost.cost += 1;
                }

                pathCost.traversable = true;

                return pathCost;
            });

            if (path != null)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    var current = path[i];

                    if (grid[current] == CellType.None)
                    {
                        grid[current] = CellType.Hallway;
                    }

                    if (i > 0)
                    {
                        var prev = path[i - 1];

                        var delta = current - prev;
                    }
                }
            }
        }
    }

    void PlaceGround(Vector2Int location)
    {
        GameObject ground = Instantiate(groundPrefab, new Vector3(location.x * positionMultiplier, 0, location.y * positionMultiplier), Quaternion.identity);
        ground.transform.parent = environmentHolder.transform;
    }

    enum WallRotation
    {
        Up,
        Down,
        Left,
        Right,
    }

    void PlaceWall(Vector2Int location, WallRotation rotation)
    {
        GameObject wall = null;
        int index = Random.Range(0, wallPrefabs.Length);
        GameObject wallPrefab = wallPrefabs[index];

        switch (rotation)
        {
            case WallRotation.Up:
                wall = Instantiate(
                    wallPrefab,
                    new Vector3(location.x * positionMultiplier, 0, location.y * positionMultiplier + (positionMultiplier / 2)),
                    Quaternion.Euler(0, 90, 0));
                break;
            case WallRotation.Down:
                wall = Instantiate(
                    wallPrefab,
                    new Vector3(location.x * positionMultiplier, 0, location.y * positionMultiplier - (positionMultiplier / 2)),
                    Quaternion.Euler(0, -90, 0));
                break;
            case WallRotation.Left:
                wall = Instantiate(
                    wallPrefab,
                    new Vector3(location.x * positionMultiplier - (positionMultiplier / 2), 0, location.y * positionMultiplier),
                    Quaternion.Euler(0, 0, 0));
                break;
            case WallRotation.Right:
                wall = Instantiate(
                    wallPrefab,
                    new Vector3(location.x * positionMultiplier + (positionMultiplier / 2), 0, location.y * positionMultiplier),
                    Quaternion.Euler(0, 180, 0));
                break;
        }

        if (wall != null)
        {
            wall.transform.parent = environmentHolder.transform;
        }
    }

    Boolean IsBetweenBounds(Vector2Int position)
    {
        return position.x < size.x && position.x >= 0 && position.y < size.y && position.y >= 0;
    }

    void PlacePrefabs()
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector2Int currentPosition = new Vector2Int(x, y);
                CellType currentCell = grid[currentPosition];

                if (currentCell == CellType.None)
                {
                    continue;
                }

                PlaceGround(currentPosition);

                Vector2Int abovePosition = currentPosition + Vector2Int.up;
                if (!IsBetweenBounds(abovePosition) || grid[abovePosition] == CellType.None)
                {
                    PlaceWall(currentPosition, WallRotation.Up);
                }

                Vector2Int belowPosition = currentPosition + Vector2Int.down;
                if (!IsBetweenBounds(belowPosition) || grid[belowPosition] == CellType.None)
                {
                    PlaceWall(currentPosition, WallRotation.Down);
                }

                Vector2Int leftPosition = currentPosition + Vector2Int.left;
                if (!IsBetweenBounds(leftPosition) || grid[leftPosition] == CellType.None)
                {
                    PlaceWall(currentPosition, WallRotation.Left);
                }

                Vector2Int rightPosition = currentPosition + Vector2Int.right;
                if (!IsBetweenBounds(rightPosition) || grid[rightPosition] == CellType.None)
                {
                    PlaceWall(currentPosition, WallRotation.Right);
                }
            }
        }
    }

    void BakeNavmesh()
    {
        NavMeshSurface navMeshSurface = environmentHolder.GetComponent<NavMeshSurface>();

        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }
        else
        {
            Debug.LogWarning("NavMeshSurface component not found on prefabHolder.");
        }
    }

    void PlacePatrolPoints()
    {
        foreach (Room room in rooms)
        {
            Vector3 roomCenter = new Vector3(
                room.bounds.center.x * positionMultiplier - (positionMultiplier / 2),
                0f,
                room.bounds.center.y * positionMultiplier - (positionMultiplier / 2));

            GameObject patrolPoint = new GameObject("PatrolPoint");
            patrolPoint.transform.position = roomCenter;
            patrolPoint.tag = "PatrolPoint";
            patrolPoint.transform.parent = patrolPointHolder.transform;
        }
    }

    bool IsHallwayAdjacent(Vector2Int position)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (Vector2Int direction in directions)
        {
            Vector2Int adjacentPosition = position + direction;
            if (IsBetweenBounds(adjacentPosition) && grid[adjacentPosition] == CellType.Hallway)
            {
                return true;
            }
        }
        return false;
    }

    List<Vector2Int> GetAvailableCells()
    {
        List<Vector2Int> availableCells = new List<Vector2Int>();

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector2Int currentPosition = new Vector2Int(x, y);
                if (grid[currentPosition] == CellType.Room)
                {
                    if (!IsHallwayAdjacent(currentPosition))
                    {
                        availableCells.Add(currentPosition);
                    }
                }
            }
        }

        return availableCells;
    }

    void PlaceSpawnPositions()
    {
        List<Vector2Int> availableCells = GetAvailableCells();

        Vector3 randomPosition = GetRandomPosition(availableCells);
        Player.Instance.transform.position = randomPosition;
        randomPosition = GetRandomPosition(availableCells);
        Instantiate(opponentPrefab, randomPosition, Quaternion.identity);

        for (int i = 0; i < totalKeys; i++)
        {
            randomPosition = GetRandomPosition(availableCells);

            GameObject keySpawn = new GameObject("KeySpawn");
            keySpawn.transform.position = randomPosition;
            keySpawn.tag = "KeySpawn";
            keySpawn.transform.parent = keySpawnHolder.transform;
        }

        for (int i = 0; i < totalDoors; i++)
        {
            randomPosition = GetRandomPosition(availableCells);

            GameObject doorSpawn = new GameObject("DoorSpawn");
            doorSpawn.transform.position = randomPosition;
            doorSpawn.tag = "DoorSpawn";
            doorSpawn.transform.parent = doorSpawnHolder.transform;
        }

        for (int i = 0; i < powerupSpawners.Length; i++)
        {
            randomPosition = GetRandomPosition(availableCells);
            powerupSpawners[i].transform.position = randomPosition;
        }

        PlaceDecorations(availableCells);
    }

    Vector3 GetRandomPosition(List<Vector2Int> availableCells)
    {
        int randomIndex = Random.Range(0, availableCells.Count);
        Vector2Int randomPosition = availableCells[randomIndex];
        availableCells.RemoveAt(randomIndex);

        return new Vector3(
            randomPosition.x * positionMultiplier,
            0f,
            randomPosition.y * positionMultiplier);
    }

    void PlaceDecorations(List<Vector2Int> availableCells)
    {
        foreach (var cell in availableCells)
        {
            if (Random.value < 0.15)
            {
                Vector3 position = new Vector3(
                    cell.x * positionMultiplier,
                    0f,
                    cell.y * positionMultiplier);
                GameObject randomObject = decorationPrefabs[Random.Range(0, decorationPrefabs.Length)];
                Instantiate(randomObject, position, Quaternion.identity);
            }
        }
    }
}
