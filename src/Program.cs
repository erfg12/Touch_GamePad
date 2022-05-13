using Raylib_cs;
using System.Diagnostics;
using System.Runtime.InteropServices;
using vJoy.Wrapper;
using System.Numerics;

namespace HelloWorld
{
    static class Program
    {
        static Program()
        {
            Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_TRANSPARENT);
            Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_UNDECORATED); // currently making the whole monitor go black, not sure why (Win11 beta)
            Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_TOPMOST);
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
            Vector2 touchPosition = new Vector2(0.0f, 0.0f);

            // initial raylib stuff
            Raylib.InitWindow(200,200, "Touch GamePad");
            Raylib.SetWindowPosition(0,0);
            Raylib.SetTargetFPS(60);
            //System.Threading.Thread.Sleep(1000);
            Raylib.SetWindowSize(Raylib.GetMonitorWidth(0), Raylib.GetMonitorHeight(0));

            // button textures
            Texture2D btn_a = Raylib.LoadTexture(Directory.GetCurrentDirectory() + "\\Resources\\a_btn.png"); // make sure Resources/*.png files are marked as "Copy Always"
            Texture2D btn_b = Raylib.LoadTexture(Directory.GetCurrentDirectory() + "\\Resources\\b_btn.png");
            Texture2D btn_x = Raylib.LoadTexture(Directory.GetCurrentDirectory() + "\\Resources\\x_btn.png");
            Texture2D btn_y = Raylib.LoadTexture(Directory.GetCurrentDirectory() + "\\Resources\\y_btn.png");

            // joystick positioning
            Vector2 scleraLeftPosition = new Vector2 (200.0f, Raylib.GetScreenHeight() / 2.0f + 250);
            Vector2 scleraRightPosition = new Vector2(Raylib.GetScreenWidth() - 200.0f, Raylib.GetScreenHeight() / 2.0f + 250);
            float scleraRadius = 160;

            Vector2 irisLeftPosition = new Vector2(200.0f, Raylib.GetScreenHeight() / 2.0f + 250);
            Vector2 irisRightPosition = new Vector2(Raylib.GetScreenWidth() - 200.0f, Raylib.GetScreenHeight() / 2.0f + 250);
            float irisRadius = 50;

            float angle = 0.0f;
            float dx1 = 0.0f, dy1 = 0.0f, dxx1 = 0.0f, dyy1 = 0.0f;
            float dx2 = 0.0f, dy2 = 0.0f, dxx2 = 0.0f, dyy2 = 0.0f;

            // button positions on screen
            Vector2 btnSize = new Vector2(68, 68);
            int btnPadding = 50;
            int wallPadding = 50;
            bool btn_a_col = false;
            bool btn_b_col = false;
            bool btn_x_col = false;
            bool btn_y_col = false;
            bool joy_l_col = false;
            bool joy_r_col = false;
            Rectangle btn_a_pos = new Rectangle (Raylib.GetScreenWidth() - btnSize.X - wallPadding, Raylib.GetScreenHeight() / 2, btnSize.X, btnSize.X);
            Rectangle btn_b_pos = new Rectangle(Raylib.GetScreenWidth() - btnSize.X + btnPadding - wallPadding, Raylib.GetScreenHeight() / 2 - btnPadding, btnSize.X, btnSize.X);
            Rectangle btn_x_pos = new Rectangle(Raylib.GetScreenWidth() - btnSize.X - btnPadding - wallPadding, Raylib.GetScreenHeight() / 2 - btnPadding, btnSize.X, btnSize.X);
            Rectangle btn_y_pos = new Rectangle(Raylib.GetScreenWidth() - btnSize.X - wallPadding, Raylib.GetScreenHeight() / 2 - (btnPadding * 2), btnSize.X, btnSize.X);

            while (!Raylib.WindowShouldClose())
            {
                btn_a_col = false;
                btn_b_col = false;
                btn_x_col = false;
                btn_y_col = false;
                joy_l_col = false;
                joy_r_col = false;

                // check for button/joystick presses / JOYSTICK AXIS: 0 = left, bottom. 16000 = center. 32000 = right, top
                for (int i = 0; i < 10; ++i)
                {
                    touchPosition = Raylib.GetTouchPosition(i);                    // Get the touch point

                    if ((touchPosition.X >= 0) && (touchPosition.Y >= 0))   // Make sure point is not (-1,-1) as this means there is no touch for it
                    {
                        if (Raylib.CheckCollisionPointRec(touchPosition, btn_a_pos))
                            btn_a_col = true;

                        if (Raylib.CheckCollisionPointRec(touchPosition, btn_b_pos))
                            btn_b_col = true;

                        if (Raylib.CheckCollisionPointRec(touchPosition, btn_x_pos))
                            btn_x_col = true;

                        if (Raylib.CheckCollisionPointRec(touchPosition, btn_y_pos))
                            btn_y_col = true;

                        if (Raylib.CheckCollisionPointCircle(touchPosition, scleraLeftPosition, scleraRadius - 20))
                        {
                            joy_l_col = true;
                            irisLeftPosition = touchPosition;
                        }

                        if (Raylib.CheckCollisionPointCircle(touchPosition, scleraRightPosition, scleraRadius - 20))
                        {
                            joy_r_col = true;
                            irisRightPosition = touchPosition;
                        }
                    }
                }

                if (btn_a_col) 
                    joystick.SetJoystickButton(true, 1);
                else 
                    joystick.SetJoystickButton(false, 1);
                if (btn_b_col)
                    joystick.SetJoystickButton(true, 2);
                else
                    joystick.SetJoystickButton(false, 2);
                if (btn_x_col)
                    joystick.SetJoystickButton(true, 3);
                else
                    joystick.SetJoystickButton(false, 3);
                if (btn_y_col)
                    joystick.SetJoystickButton(true, 4);
                else
                    joystick.SetJoystickButton(false, 4);

                // joysticks - Check not inside the sclera (does nothing yet)
                if (!Raylib.CheckCollisionPointCircle(irisLeftPosition, scleraLeftPosition, scleraRadius - 20) && joy_l_col)
                {
                    dx1 = irisLeftPosition.X - scleraLeftPosition.X;
                    dy1 = irisLeftPosition.Y - scleraLeftPosition.Y;

                    angle = MathF.Atan2(dy1, dx1);

                    dxx1 = (scleraRadius - irisRadius) * MathF.Cos(angle);
                    dyy1 = (scleraRadius - irisRadius) * MathF.Sin(angle);

                    irisLeftPosition.X = scleraLeftPosition.X + dxx1;
                    irisLeftPosition.Y = scleraLeftPosition.Y + dyy1;
                }
                if (!Raylib.CheckCollisionPointCircle(irisRightPosition, scleraRightPosition, scleraRadius - 20) && joy_r_col)
                {
                    dx2 = irisRightPosition.X - scleraRightPosition.X;
                    dy2 = irisRightPosition.Y - scleraRightPosition.Y;

                    angle = MathF.Atan2(dy2, dx2);

                    dxx2 = (scleraRadius - irisRadius) * MathF.Cos(angle);
                    dyy2 = (scleraRadius - irisRadius) * MathF.Sin(angle);

                    irisRightPosition.X = scleraRightPosition.X + dxx2;
                    irisRightPosition.Y = scleraRightPosition.Y + dyy2;
                }

                if (!joy_l_col)
                {
                    irisLeftPosition = new Vector2(200.0f, Raylib.GetScreenHeight() / 2.0f + 250);
                    joystick.SetJoystickAxis(16000, Axis.HID_USAGE_X);
                    joystick.SetJoystickAxis(16000, Axis.HID_USAGE_Y);
                }
                else
                {
                    joystick.SetJoystickAxis((int)(16000 * (((irisLeftPosition.X - scleraLeftPosition.X) / 139) + 1)), Axis.HID_USAGE_X);
                    joystick.SetJoystickAxis((int)(16000 * (((irisLeftPosition.Y - scleraLeftPosition.Y) / 139) + 1)), Axis.HID_USAGE_Y);
                }
                if (!joy_r_col)
                {
                    irisRightPosition = new Vector2(Raylib.GetScreenWidth() - 200.0f, Raylib.GetScreenHeight() / 2.0f + 250);
                    joystick.SetJoystickAxis(16000, Axis.HID_USAGE_RX);
                    joystick.SetJoystickAxis(16000, Axis.HID_USAGE_RY);
                }
                else
                {
                    joystick.SetJoystickAxis((int)(16000 * (((irisRightPosition.X - scleraRightPosition.X) / 139) + 1)), Axis.HID_USAGE_RX);
                    joystick.SetJoystickAxis((int)(16000 * (((irisRightPosition.Y - scleraRightPosition.Y) / 139) + 1)), Axis.HID_USAGE_RY);
                }

                //Debug.WriteLine(irisRightPosition.Y - scleraRightPosition.Y);

                // draw on screen
                Raylib.BeginDrawing();
                    Raylib.ClearBackground(Color.BLANK); // transparent background

                    // buttons
                    Raylib.DrawTexture(btn_a, (int)btn_a_pos.x, (int)btn_a_pos.y, Raylib.Fade(Color.WHITE, 0.5f)); // NOTE: Color.BLANK makes the image not show
                    Raylib.DrawTexture(btn_b, (int)btn_b_pos.x, (int)btn_b_pos.y, Raylib.Fade(Color.WHITE, 0.5f));
                    Raylib.DrawTexture(btn_x, (int)btn_x_pos.x, (int)btn_x_pos.y, Raylib.Fade(Color.WHITE, 0.5f));
                    Raylib.DrawTexture(btn_y, (int)btn_y_pos.x, (int)btn_y_pos.y, Raylib.Fade(Color.WHITE, 0.5f));

                    // joysticks
                    Raylib.DrawCircleV(scleraLeftPosition, scleraRadius, Raylib.Fade(Color.LIGHTGRAY, 0.5f));
                    Raylib.DrawCircleV(irisLeftPosition, irisRadius, Raylib.Fade(Color.BLACK, 0.5f));
                    Raylib.DrawCircleV(scleraRightPosition, scleraRadius, Raylib.Fade(Color.LIGHTGRAY, 0.5f));
                    Raylib.DrawCircleV(irisRightPosition, irisRadius, Raylib.Fade(Color.BLACK, 0.5f));
                Raylib.EndDrawing();
            }

            // clean up
            joystick.SetJoystickButton(false, 1);
            joystick.SetJoystickButton(false, 2);
            joystick.SetJoystickButton(false, 3);
            joystick.SetJoystickButton(false, 4);
            joystick.SetJoystickAxis(16000, Axis.HID_USAGE_X);
            joystick.SetJoystickAxis(16000, Axis.HID_USAGE_Y);
            joystick.SetJoystickAxis(16000, Axis.HID_USAGE_RX);
            joystick.SetJoystickAxis(16000, Axis.HID_USAGE_RY);
            Raylib.UnloadTexture(btn_a);
            Raylib.UnloadTexture(btn_b);
            Raylib.UnloadTexture(btn_x);
            Raylib.UnloadTexture(btn_y);
            Raylib.CloseWindow();
        }
    }
}