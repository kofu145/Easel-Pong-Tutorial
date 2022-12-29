using System.Numerics;
using Easel;
using Easel.Entities;
using Easel.Entities.Components;
using Pie.Windowing;

namespace EaselPong.Components;

public enum Side
{
    Left,
    Right
}

public class Paddle : Component
{
    public Side PaddleSide;
    private float speed;
    private float velocity;
    public Vector2 PaddleSize;
    public int Score;
    
    // The set keybinds to use to control either side.
    private Key[] leftPlayerInputs = { Key.W, Key.S };
    private Key[] rightPlayerInputs = { Key.Up, Key.Down };

    public Paddle(Side side, float speed)
    {
        PaddleSide = side;
        this.speed = speed;
        Score = 0;
    }

    protected override void Initialize()
    {
        base.Initialize();
        // We store paddleSize here so that we can use it when checking collisions in the ball component
        // (otherwise, we'd have to call getcomponent<sprite>() per frame)
        var paddleSize = Entity.GetComponent<Sprite>().Texture.Size;
        PaddleSize = new Vector2(paddleSize.Width * Transform.Scale.X, 
            paddleSize.Height * Transform.Scale.Y);
    }

    protected override void Update()
    {
        base.Update();
        
        // Basic switch statements that moves each paddle to its accordingly bound keys, clamped to screen bounds.
        velocity = 0f;

        switch (PaddleSide)
        {
            case Side.Left:
                if (Input.KeyDown(leftPlayerInputs[0]))
                    velocity = -speed;
                if (Input.KeyDown(leftPlayerInputs[1]))
                    velocity = speed;
                break;
            
            case Side.Right:
                if (Input.KeyDown(rightPlayerInputs[0]))
                    velocity = -speed;
                if (Input.KeyDown(rightPlayerInputs[1]))
                    velocity = speed;
                break;
            
            default:
                break;
        }

        var yOffset = PaddleSize.Y / 2;
        Transform.Position.Y += velocity * Time.DeltaTime;
        Transform.Position.Y = Math.Clamp(Transform.Position.Y, yOffset, Graphics.Viewport.Height- yOffset);

    }

}