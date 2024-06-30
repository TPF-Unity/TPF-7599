# Dungeon Escape

"Dungeon Escape" es un videojuego desarrollado en el marco de la asignatura Trabajo Profesional de la carrera Ingeniería en Informática de la Facultad de Ingeniería de la Universidad de Buenos Aires.

## Jugar

Para iniciar el juego, buscar el archivo ejecutable en el último Release del repositorio y ejecutarlo.

## Editar proyecto

Para abrir el proyecto en el editor de Unity se deben seguir los siguientes pasos:
1. Clonar o descargar el contenido del repositorio.
2. Instalar Unity Hub siguiendo las instrucciones proporcionadas en el [manual de Unity](https://docs.unity3d.com/hub/manual/InstallHub.html).
3. Dirigirse a la sección “Projects” en Unity Hub e importar el directorio “DungeonEscape” del repositorio. Referirse a las instrucciones del [manual de Unity](https://docs.unity3d.com/hub/manual/AddProject.html) para más información.
4. Abrir el proyecto importado para iniciar el editor de Unity. En caso de no poseer la versión correcta de Unity instalada, Unity Hub solicitará instalarla y simplemente se debe aceptar esa opción.

## Instrucciones

#### Objetivo

En el juego se controla a un mago que debe escapar de una mazmorra mientras es perseguido por distintos enemigos y compite contra un oponente.

Para avanzar de nivel se deben recolectar las llaves necesarias para activar los portales por los que se puede escapar.

Si el mago muere o el oponente alcanza el portal antes, el juego termina.

#### Ayudas

El juego cuenta con distintos "Powerups" que al recolectarlos incrementan temporalmente alguno de los atributos del personaje como su velocidad, daño de ataque, velocidad de ataque y vida.

#### Controles

- Moverse: utilizar las teclas WASD para desplazarse.
- Correr: mantener presionada la tecla Shift.
- Atacar: apuntar con el cursor y disparar con click izquierdo.
- Pausar el juego: presionar la tecla Escape.

#### Configuraciones

En el menú de inicio se puede configurar la dificultad del juego y el algoritmo de IA que controla al oponente.

Las opciones de dificultad son Fácil, Normal, Difícil y Adaptativa. Esta úlitma consiste en ajustar la dificultad dinámicamente en base al rendimiento en cada nivel.

Las opciones de IA para el oponente son GOAP y Behaviour Tree.
