*** IMPORTANT: Currently this is just a proof of concept and is not complete! ***

## About
Transparent overlay with game controller buttons for touch screen devices. Great for Windows tablets that can play Xbox style console games. ***Games will need to be played in a windowed mode. They cannot be fullscreen!*** 

This project uses [RayLib](https://www.raylib.com/) to build the transparent window and touchscreen buttons and [VJoy](https://sourceforge.net/projects/vjoystick/) to generate a virtual controller. You will need [x360ce](https://www.x360ce.com/) to bind the joystick buttons to emulate an Xbox controller if the game does not support generic joysticks. For example, OpenGL games will support generic joysticks, while DirectX games will most likely not.

## How To Build
Open the `src.sln` file in Visual Studio. Be sure NuGet retrieved the `Raylib-cs` and `VJoy.wrappper` packages, and press build. _NOTE: You may need to target an x86 platform._

