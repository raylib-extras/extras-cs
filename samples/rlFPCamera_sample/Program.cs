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

                        DrawCubeTexture(tx, new Vector3( x, 1.5f, z ), 1, 1, 1, Color.GREEN);
                        DrawCubeTexture(tx, new Vector3( x, 0.5f, z ), 0.25f, 1, 0.25f, Color.BROWN);
                    }
                }

                cam.EndMode3D();
                Raylib.DrawFPS(0, 0);
                Raylib.EndDrawing();

            }

            Raylib.UnloadTexture(tx);
            Raylib.CloseWindow();
        }

        // Draw cube textured
        // NOTE: Cube position is the center position
        static void DrawCubeTexture(
            Texture2D texture,
            Vector3 position,
            float width,
            float height,
            float length,
            Color color
        )
        {
            float x = position.X;
            float y = position.Y;
            float z = position.Z;

            // Set desired texture to be enabled while drawing following vertex data
            Rlgl.rlSetTexture(texture.id);

            // Vertex data transformation can be defined with the commented lines,
            // but in this example we calculate the transformed vertex data directly when calling Rlgl.rlVertex3f()
            // Rlgl.rlPushMatrix();
            // NOTE: Transformation is applied in inverse order (scale -> rotate -> translate)
            // Rlgl.rlTranslatef(2.0f, 0.0f, 0.0f);
            // Rlgl.rlRotatef(45, 0, 1, 0);
            // Rlgl.rlScalef(2.0f, 2.0f, 2.0f);

            Rlgl.rlBegin(DrawMode.QUADS);
            Rlgl.rlColor4ub(color.r, color.g, color.b, color.a);

            // Front Face
            // Normal Pointing Towards Viewer
            Rlgl.rlNormal3f(0.0f, 0.0f, 1.0f);
            Rlgl.rlTexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.rlVertex3f(x - width / 2, y - height / 2, z + length / 2);
            Rlgl.rlTexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.rlVertex3f(x + width / 2, y - height / 2, z + length / 2);
            Rlgl.rlTexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.rlVertex3f(x + width / 2, y + height / 2, z + length / 2);
            Rlgl.rlTexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.rlVertex3f(x - width / 2, y + height / 2, z + length / 2);

            // Back Face
            // Normal Pointing Away From Viewer
            Rlgl.rlNormal3f(0.0f, 0.0f, -1.0f);
            Rlgl.rlTexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.rlVertex3f(x - width / 2, y - height / 2, z - length / 2);
            Rlgl.rlTexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.rlVertex3f(x - width / 2, y + height / 2, z - length / 2);
            Rlgl.rlTexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.rlVertex3f(x + width / 2, y + height / 2, z - length / 2);
            Rlgl.rlTexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.rlVertex3f(x + width / 2, y - height / 2, z - length / 2);

            // Top Face
            // Normal Pointing Up
            Rlgl.rlNormal3f(0.0f, 1.0f, 0.0f);
            Rlgl.rlTexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.rlVertex3f(x - width / 2, y + height / 2, z - length / 2);
            Rlgl.rlTexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.rlVertex3f(x - width / 2, y + height / 2, z + length / 2);
            Rlgl.rlTexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.rlVertex3f(x + width / 2, y + height / 2, z + length / 2);
            Rlgl.rlTexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.rlVertex3f(x + width / 2, y + height / 2, z - length / 2);

            // Bottom Face
            // Normal Pointing Down
            Rlgl.rlNormal3f(0.0f, -1.0f, 0.0f);
            Rlgl.rlTexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.rlVertex3f(x - width / 2, y - height / 2, z - length / 2);
            Rlgl.rlTexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.rlVertex3f(x + width / 2, y - height / 2, z - length / 2);
            Rlgl.rlTexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.rlVertex3f(x + width / 2, y - height / 2, z + length / 2);
            Rlgl.rlTexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.rlVertex3f(x - width / 2, y - height / 2, z + length / 2);

            // Right face
            // Normal Pointing Right
            Rlgl.rlNormal3f(1.0f, 0.0f, 0.0f);
            Rlgl.rlTexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.rlVertex3f(x + width / 2, y - height / 2, z - length / 2);
            Rlgl.rlTexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.rlVertex3f(x + width / 2, y + height / 2, z - length / 2);
            Rlgl.rlTexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.rlVertex3f(x + width / 2, y + height / 2, z + length / 2);
            Rlgl.rlTexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.rlVertex3f(x + width / 2, y - height / 2, z + length / 2);

            // Left Face
            // Normal Pointing Left
            Rlgl.rlNormal3f(-1.0f, 0.0f, 0.0f);
            Rlgl.rlTexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.rlVertex3f(x - width / 2, y - height / 2, z - length / 2);
            Rlgl.rlTexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.rlVertex3f(x - width / 2, y - height / 2, z + length / 2);
            Rlgl.rlTexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.rlVertex3f(x - width / 2, y + height / 2, z + length / 2);
            Rlgl.rlTexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.rlVertex3f(x - width / 2, y + height / 2, z - length / 2);
            Rlgl.rlEnd();
            //rlPopMatrix();

            Rlgl.rlSetTexture(0);
        }
    }
}
