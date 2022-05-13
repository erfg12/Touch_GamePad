using Raylib_cs;
using System.Diagnostics;
using System.Runtime.InteropServices;
using vJoy.Wrapper;
using static Raylib_cs.MouseButton;
using System.Numerics;

namespace HelloWorld
{
    static class Program
    {
        static Program()
        {
            Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_TRANSPARENT);
            //Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_UNDECORATED); // currently making the whole monitor go black, not sure why (Win11 beta)
            //Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_TOPMOST);
        }
        public static void Main()
        {
            // VJoy joystick setup.
            var joystick = new VirtualJoystick(1);
            try
            {
                joystick.Aquire();
            } catch
            {
                Console.WriteLine("ERROR: Issues with joystick.Aquire(), is VJoy installed on your PC?! sourceforge.net/projects/vjoystick");
            }
            Vector2 mousePoint = new Vector2(0.0f, 0.0f);

            // initial raylib stuff
            Raylib.InitWindow(Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), "Touch GamePad");
            Raylib.SetWindowPosition(0,60);
            Raylib.SetTargetFPS(60);

            // button textures
            Texture2D btn_a = Raylib.LoadTexture(Directory.GetCurrentDirectory() + "\\Resources\\a_btn.png"); // make sure Resources/*.png files are marked as "Copy Always"
            Texture2D btn_b = Raylib.LoadTexture(Directory.GetCurrentDirectory() + "\\Resources\\b_btn.png");
            Texture2D btn_x = Raylib.LoadTexture(Directory.GetCurrentDirectory() + "\\Resources\\x_btn.png");
            Texture2D btn_y = Raylib.LoadTexture(Directory.GetCurrentDirectory() + "\\Resources\\y_btn.png");

            // button positions on screen
            Vector2 btnSize = new Vector2(68, 68);
            int btnPadding = 50;
            int wallPadding = 50;
            Rectangle btn_a_pos = new Rectangle (Raylib.GetScreenWidth() - btnSize.X - wallPadding, Raylib.GetScreenHeight() / 2, btnSize.X, btnSize.X);
            Rectangle btn_b_pos = new Rectangle(Raylib.GetScreenWidth() - btnSize.X + btnPadding - wallPadding, Raylib.GetScreenHeight() / 2 - btnPadding, btnSize.X, btnSize.X);
            Rectangle btn_x_pos = new Rectangle(Raylib.GetScreenWidth() - btnSize.X - btnPadding - wallPadding, Raylib.GetScreenHeight() / 2 - btnPadding, btnSize.X, btnSize.X);
            Rectangle btn_y_pos = new Rectangle(Raylib.GetScreenWidth() - btnSize.X - wallPadding, Raylib.GetScreenHeight() / 2 - (btnPadding * 2), btnSize.X, btnSize.X);

            while (!Raylib.WindowShouldClose())
            {
                mousePoint = Raylib.GetMousePosition();
                // check for button presses
                if (Raylib.CheckCollisionPointRec(mousePoint, btn_a_pos) && Raylib.IsMouseButtonDown(MOUSE_BUTTON_LEFT))
                    joystick.SetJoystickButton(true, 1);
                else
                    joystick.SetJoystickButton(false, 1);

                if (Raylib.CheckCollisionPointRec(mousePoint, btn_b_pos) && Raylib.IsMouseButtonDown(MOUSE_BUTTON_LEFT))
                    joystick.SetJoystickButton(true, 2);
                else
                    joystick.SetJoystickButton(false, 2);

                if (Raylib.CheckCollisionPointRec(mousePoint, btn_x_pos) && Raylib.IsMouseButtonDown(MOUSE_BUTTON_LEFT))
                    joystick.SetJoystickButton(true, 3);
                else
                    joystick.SetJoystickButton(false, 3);

                if (Raylib.CheckCollisionPointRec(mousePoint, btn_y_pos) && Raylib.IsMouseButtonDown(MOUSE_BUTTON_LEFT))
                    joystick.SetJoystickButton(true, 4);
                else
                    joystick.SetJoystickButton(false, 4);

                // draw buttons on screen
                //Raylib.RestoreWindow();
                Raylib.BeginDrawing();
                    Raylib.ClearBackground(Color.BLANK); // transparent background
                    Raylib.DrawTexture(btn_a, (int)btn_a_pos.x, (int)btn_a_pos.y, Color.WHITE); // NOTE: Color.BLANK makes the image not show
                    Raylib.DrawTexture(btn_b, (int)btn_b_pos.x, (int)btn_b_pos.y, Color.WHITE);
                    Raylib.DrawTexture(btn_x, (int)btn_x_pos.x, (int)btn_x_pos.y, Color.WHITE);
                    Raylib.DrawTexture(btn_y, (int)btn_y_pos.x, (int)btn_y_pos.y, Color.WHITE);
                Raylib.EndDrawing();
            }

            // clean up
            joystick.SetJoystickButton(false, 1);
            Raylib.UnloadTexture(btn_a);
            Raylib.UnloadTexture(btn_b);
            Raylib.UnloadTexture(btn_x);
            Raylib.UnloadTexture(btn_y);
            Raylib.CloseWindow();
        }
    }
}