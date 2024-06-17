public class MainPlayer : Player
{
    public static Player Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}