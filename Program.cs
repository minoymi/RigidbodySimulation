using Raylib_cs;
using System.Numerics;

namespace RigidbodySimulation
{
    public class Ball
    {
        public Vector2 pos;
        public Vector2 speed;
        public Color color;
        public Ball(Vector2 pos, Color color) 
        {
            this.pos = pos;
            this.color = color;
            speed.X = 0;
            speed.Y = 0;
        }
    }


    internal class Program
    {
        const int SCREEN_WIDTH = 1000;
        const int SCREEN_HEIGHT = 1000;
        const int TARGET_FPS = 75;

        const float BALL_RADIUS = 20.0f;
        const float GRAVITY = 0.01f;


        static void Main()
        {
            
            Color BG_COLOR = Color.WHITE;
            Color[] Rainbow = new Color[] 
            {
                Color.RED, 
                Color.ORANGE, 
                Color.YELLOW, 
                Color.GREEN, 
                Color.SKYBLUE, 
                Color.BLUE, 
                Color.VIOLET
            };
            Random rnd = new();

            List<Ball> balls = new List<Ball>();


            Raylib.InitWindow(SCREEN_WIDTH, SCREEN_HEIGHT, "Rigidbody Simulation");
            Raylib.SetTargetFPS(TARGET_FPS);

            while (!Raylib.WindowShouldClose())
            {
                int cycles = 10;
                for (int i = 0; i < cycles; i++)
                {
                    ApplyGravity();
                    PerformCollisions();

                }


                Vector2 mouse_pos = Raylib.GetMousePosition();
                if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT) || Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_RIGHT))
                {
                    Ball ball = new Ball(mouse_pos, Rainbow[rnd.Next(Rainbow.Length)]);
                    balls.Add(ball);
                }
                
                Raylib.BeginDrawing();
                Raylib.ClearBackground(BG_COLOR);

                foreach (Ball ball in balls)
                {
                    Raylib.DrawCircleV(ball.pos, BALL_RADIUS, ball.color);
                }

                Raylib.DrawFPS(10, 10);
                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();






            void PerformCollisions() 
            {
                for (int i = 0; i < balls.Count; i++)
                {
                    if (balls[i].pos.Y > SCREEN_HEIGHT - BALL_RADIUS)
                    {
                        balls[i].pos.Y = SCREEN_HEIGHT - BALL_RADIUS;
                        balls[i].speed.Y = 0;
                    }

                    if (balls[i].pos.X > SCREEN_WIDTH - BALL_RADIUS)
                    {
                        balls[i].pos.X = SCREEN_WIDTH - BALL_RADIUS;
                        balls[i].speed.X = 0;
                    }
                    else if (balls[i].pos.X < BALL_RADIUS)
                    {
                        balls[i].pos.X = BALL_RADIUS;
                        balls[i].speed.X = 0;
                    }


                    for (int k = 0; k < balls.Count; k++)
                    {
                        if (i != k)
                        {
                            float distance = Vector2.Distance(balls[i].pos, balls[k].pos);
                            if (distance < BALL_RADIUS * 2 - 1) 
                            {
                                Vector2 correction = Vector2.Normalize(balls[i].pos - balls[k].pos);
                                balls[i].pos += correction;
                                balls[k].pos -= correction;

                                balls[i].speed.Y = 0;
                                balls[k].speed.Y = 0;

                            }
                        }
                    }
                }
            }


            void ApplyGravity()
            {
                for (int i = 0; i < balls.Count; i++)
                {
                    balls[i].speed.Y += GRAVITY;
                    balls[i].pos += balls[i].speed;
                }
            }

        }



    }
}