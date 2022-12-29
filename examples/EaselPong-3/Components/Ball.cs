using System.Numerics;

using Easel;
using Easel.Entities;
using Easel.Scenes;
using Easel.Entities.Components;
using Easel.Audio;

namespace EaselPong.Components;

public class Ball : Component
{
    private Vector3 velocity;
    private float speed;
    private float radius;
    private Entity[] paddles;

    public Ball(float radius, float speed)
    {
        this.radius = radius;
        this.speed = speed;
    }

    protected override void Initialize()
    {
        base.Initialize();
        paddles = GetEntitiesWithTag("paddle");
        Serve(Side.Right);
    }

    protected override void Update()
    {
        base.Update();
        Transform.Position += velocity * Time.DeltaTime;

        
        // Whenever the ball hits the ceiling or floor, make it bounce. 
        // Thanks to a weird bug where the ball would get stuck inside of the ceiling/floor and continually bounce, 
        // the if statements are split up so we can just set it to a position where it can't get stuck
        if (Transform.Position.Y <= radius)
        {
            Transform.Position.Y = radius;
            velocity.Y = -velocity.Y;
        }

        if (Transform.Position.Y >= Graphics.Viewport.Height - radius)
        {
            Transform.Position.Y = Graphics.Viewport.Height - radius;
            velocity.Y = -velocity.Y;

        }

        // If the ball hits the left or right ends of the screen, score for the appropriate player.
        if (Transform.Position.X <= radius)
        {
            Serve(Side.Left);
            paddles[1].GetComponent<Paddle>().Score++;
        }

        else if (Transform.Position.X >= Graphics.Viewport.Width - radius)
        {
            Serve(Side.Right);
            paddles[0].GetComponent<Paddle>().Score++;
        }
        
        // Check if the ball has collided with either paddle.
        foreach (var entity in paddles)
        {
            // Could also store components as well to avoid constant getcomponent calls?
            var paddle = entity.GetComponent<Paddle>();
            var paddleX = entity.Transform.Position.X;
            var paddleY = entity.Transform.Position.Y;
            if (CheckCollision(
                    paddleX - paddle.PaddleSize.X / 2 - radius,
                    paddleX + paddle.PaddleSize.X / 2 + radius, 
                    paddleY - paddle.PaddleSize.Y / 2 - radius,
                    paddleY + paddle.PaddleSize.Y / 2 + radius))
            {
                velocity.X = -velocity.X;
            }
        }

    }

    // Resets the position of the ball and adds velocity towards the last non scored player.
    private void Serve(Side side)
    {
        Transform.Position = new Vector3(300f, 200f, 1f);
        if (side == Side.Left)
            velocity = new Vector3(-speed, speed, 0f);
        if (side == Side.Right)
            velocity = new Vector3(speed, speed, 0f);
    }

    
    private bool CheckCollision(float left, float right, float bottom, float top)
    {
        // Shorthands so that our conditional isn't too wordy
        var x = Transform.Position.X;
        var y = Transform.Position.Y;
        return x >= left && x <= right && y >= bottom && y <= top;
    }
    
}