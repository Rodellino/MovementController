# MovementController
A simple and upgradeable character movement solution for Unity 3D projects

#### English
---
This solution provides movement to a character in a Unity 3D environment, through a series of swappable and upgradeable components.

Developed as a graduate thesis on videogame prototyping, and to be used in personal developments.

Once imported into a Unity project, the main MovementController script must be added as a component to the desired character object to be controlled. That will allow for some settings to be adjusted through the inspector, such as the character capsule size, that will allow for accurate collision handling.

 ![image](https://user-images.githubusercontent.com/45183382/166152283-0667f8c2-0aa4-4c27-a743-317eea293daa.png)

The types of movement supported by default are:
-	Horizontal movement (x, z axis) at a single max speed
-	Horizontal dash in the same axis as the base movement
-	Falling, due to gravity force when overpassing a ledge, without jumping

Adding further movement types such as running or jumping should be a simple task. A new movement state must be defined into the Movement State Machine, as well as defining a new input type if needed.

Object displacement and collision resolution is done through Unity’s base component [Character Controller][Unity's Character Controller], using its method “Move()” as the interface.

To catch inputs, Unity’s [New Input System][Unity's New Input System] is used. As an optional package, in order to fix its dependence, the package must be imported to the project or replaced by a custom solution and associate it in the InputHandler code.


#### Español
---
Este paquete permite dotar de movimiento a un personaje en un entorno 3D en Unity, a través de un conjunto de componentes intercambiables y escalables. 

Desarrollado como proyecto universitario para el prototipado de videojuegos y para su uso en proyectos personales.

Una vez importado en un proyecto de Unity, solo debe añadirse el script principal MovementController como componente al objeto del personaje a controlar. Permite ajustar parámetros básicos del movimiento desde el inspector, como la velocidad de movimiento, y se deben establecer las dimensiones de la cápsula que contenga el objeto del personaje, para manejar adecuadamente las colisiones.

![image](https://user-images.githubusercontent.com/45183382/166144234-0176052c-bf29-42b5-ba42-8eab51aae948.png)

Los tipos de movimiento implementados de base en el sistema son:
-	Movimiento horizontal (en el eje x, z) a una única velocidad máxima
-	Dash o acelerón en el mismo eje que el movimiento horizontal
-	Caída, consecuencia de la fuerza de la gravedad al rebasar una cornisa, sin salto

Añadir otros tipos de movimiento como una segunda velocidad de movimiento (correr) o un salto es una operación relativamente sencilla a través de la inclusión de estados en la máquina finita de estados que controla los tipos de movimiento.

El desplazamiento y la gestión de colisiones se realiza por defecto a través del componente base de Unity [Character Controller][Unity's Character Controller] y usando como interfaz su método “Move()”.

Para la captura de inputs se utiliza por defecto [New Input System][Unity's New Input System] de Unity. Al tratarse de un paquete opcional, para corregir su dependencia deberá importarse en el proyecto o sustituido por una solución distinta de preferencia y asociarla en el código del InputHandler.

[Unity's Character Controller]: https://docs.unity3d.com/ScriptReference/CharacterController.html
[Unity's New Input System]: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/QuickStartGuide.html
[MovementController]: ../MovementController/Movement/MovementController.cs
[InputHandler]: ../MovementController/Controls/InputHandler.cs
