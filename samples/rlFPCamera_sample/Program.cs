using System;
using System.Numerics;

using raylibExtras;
using Raylib_cs;

namespace rlFPCamera_sample
{
    class Program
    {
        static void Main(string[] args)
        {
            int screenWidth = 1900;
            int screenHeight = 900;

            Raylib.InitWindow(screenWidth, screenHeight, "raylibExtras [camera] example - First person camera");
            Raylib.SetTargetFPS(144);

            Image img = Raylib.GenImageChecked(256, 256, 32, 32, Color.DARKGRAY, Color.WHITE);
            Texture2D tx = Raylib.LoadTextureFromImage(img);
            Raylib.UnloadImage(img);
            Raylib.SetTextureFilter(tx, TextureFilter.TEXTURE_FILTER_ANISOTROPIC_16X);
            Raylib.SetTextureWrap(tx, TextureWrap.TEXTURE_WRAP_CLAMP);

            rlFPCamera cam = new rlFPCamera();
            cam.Setup(45, new Vector3( 1, 0, 0));
            cam.MoveSpeed.Z = 10;
            cam.MoveSpeed.X = 5;
            cam.FarPlane = 5000;

            // Main game loop
            while (!Raylib.WindowShouldClose())    // Detect window close button or ESC key
            {
                cam.Update();

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.SKYBLUE);

                //Raylib.BeginMode3D(cam.Camera);

                cam.BeginMode3D();

                // grid of cube trees on a plane to make a "world"
                Raylib.DrawPlane(new Vector3( 0, 0, 0 ), new Vector2( 50, 50 ), Color.BEIGE); // simple world plane
                float spacing = 4;
                int count = 5;

                for (float x = -count * spacing; x <= count * spacing; x += spacing)
                {
                    for (float z = -count * spacing; z <= count * spacing; z += spacing)
                    {
                        Vector3 pos = new Vector3( x, 0.5f, z );

                        Vector3 min = new Vector3( x - 0.5f, 0, z - 0.5f );
                        Vector3 max = new Vector3( x + 0.5f, 1, z + 0.5f );

                        Raylib.DrawCubeTexture(tx, new Vector3( x, 1.5f, z ), 1, 1, 1, Color.GREEN);
                        Raylib.DrawCubeTexture(tx, new Vector3( x, 0.5f, z ), 0.25f, 1, 0.25f, Color.BROWN);
                    }
                }

                cam.EndMode3D();
                Raylib.DrawFPS(0, 0);
                Raylib.EndDrawing();
           
            }

            Raylib.UnloadTexture(tx);
            Raylib.CloseWindow();
        }
    }
}
