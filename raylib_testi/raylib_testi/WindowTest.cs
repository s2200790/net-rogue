using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;

namespace raylib_testi
{
    internal class WindowTest
    {
        const int screen_width = 900;
        const int screen_height = 460;
        public WindowTest()
        {

        }

        public void Run()
        {
            Raylib.InitWindow(screen_width, screen_height, "Raylib");
            Raylib.SetTargetFPS(60);

            while (Raylib.WindowShouldClose() == false)
            {
                Update();
                Draw();
            }

            Raylib.CloseWindow();
        }

        private void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLUE);
            // Draws a maroon circle in the middle
            Raylib.DrawCircle(screen_width / 5, 90, 40, Raylib.WHITE);
            Raylib.DrawCircle(screen_width / 5, 160, 50, Raylib.WHITE);
            Raylib.DrawCircle(screen_width / 5, 250, 70, Raylib.WHITE);
            Raylib.DrawCircle(screen_width / 5 - 17, 80, 2, Raylib.BLACK);
            Raylib.DrawCircle(screen_width / 4 - 30, 80, 2, Raylib.BLACK);
            Raylib.DrawCircle(screen_width / 5, 140, 2, Raylib.BLACK);
            Raylib.DrawCircle(screen_width / 5, 170, 2, Raylib.BLACK);
            Raylib.DrawCircle(screen_width / 5, 210, 2, Raylib.BLACK);
            Raylib.DrawCircle(screen_width / 5, 95, 4, Raylib.ORANGE);

            Raylib.DrawTriangle(new Vector2(screen_width / 4.0f * 3.0f, 90.0f),
                         new Vector2(screen_width / 4.0f * 3.0f - 40.0f, 150.0f ),
                         new Vector2 (screen_width / 4.0f * 3.0f + 40.0f, 150.0f ), Raylib.GREEN);

            Raylib.DrawTriangle(new Vector2(screen_width / 4.0f * 3.0f, 130.0f),
                         new Vector2(screen_width / 4.0f * 3.0f - 50.0f, 200.0f),
                         new Vector2(screen_width / 4.0f * 3.0f + 50.0f, 200.0f), Raylib.GREEN);

            Raylib.DrawTriangle(new Vector2(screen_width / 4.0f * 3.0f, 180.0f),
                         new Vector2(screen_width / 4.0f * 3.0f - 60.0f, 250.0f),
                         new Vector2(screen_width / 4.0f * 3.0f + 60.0f, 250.0f), Raylib.GREEN);
            Raylib.DrawRectangle(screen_width / 4 * 2 + 205, 250, 40, 40, Raylib.BROWN);

            Raylib.DrawRectangle(screen_width / 4 * 2 - 60, 80, 160, 250, Raylib.MAROON);
            Raylib.DrawRectangle(screen_width / 4 * 2 - 40, 100, 50, 50, Raylib.YELLOW);
            Raylib.DrawRectangle(screen_width / 4 * 2 + 25, 100, 50, 50, Raylib.YELLOW);
            Raylib.DrawRectangle(screen_width / 4 * 2 - 40, 170, 50, 50, Raylib.YELLOW);
            Raylib.DrawRectangle(screen_width / 4 * 2 + 25, 170, 50, 50, Raylib.YELLOW);
            Raylib.DrawRectangle(screen_width / 4 * 2 - 40, 240, 50, 50, Raylib.YELLOW);
            Raylib.DrawRectangle(screen_width / 4 * 2 + 25, 240, 50, 90, Raylib.BROWN);
            // Draw rest of the game here

            Raylib.EndDrawing();
        }

        private void Update()
        {
            // Update game here
        }
    }
}
