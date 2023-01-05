using Easel.Entities;
using Easel.Scenes;
using Easel.Entities.Components;
using System.Numerics;
using Easel.Graphics;
using Easel.Graphics.Renderers;
using EaselPong.Components;

using Easel.GUI;
using Easel.Math;


namespace EaselPong;

public class MainScene : Scene
{
    
    public const float PaddleSize = 7f;
    public const float BallSize = 5f;
    public const float BallSpeed = 300f;
    public const float PaddleSpeed = 500f;

    private Entity ball;
    private Entity leftPlayer;
    private Entity rightPlayer;
    
    protected override void Initialize()
    {
        base.Initialize();
        Camera.Main.CameraType = CameraType.Orthographic;
        World.SpriteRenderMode = SpriteRenderMode.Nearest;
        //UI.Theme.Font = new Font("Content/square.ttf");

        
        // We create our paddle and ball entities, add appropriate sprite and associated components,
        // then add them to the scene.
        ball = new Entity();
        leftPlayer = new Entity();
        rightPlayer = new Entity();
        
        leftPlayer.Tag = "paddle";
        rightPlayer.Tag = "paddle";

        //Texture2D ballTexture = new Texture2D("Content/ball.png");
        Texture2D paddleTexture = new Texture2D("Content/paddle.png");
        
        Texture2D ballTexture = Content.Load<Texture2D>("ball.png");


        ball.AddComponent(new Sprite(ballTexture));
        leftPlayer.AddComponent(new Sprite(paddleTexture));
        rightPlayer.AddComponent(new Sprite(paddleTexture));
        
        ball.AddComponent(new Ball(12.5f, BallSpeed));
        leftPlayer.AddComponent(new Paddle(Side.Left, PaddleSpeed));
        rightPlayer.AddComponent(new Paddle(Side.Right, PaddleSpeed));

        // Some size refactoring to better fit the screen + gameplay
        ball.Transform.Scale = new Vector3(BallSize, BallSize, 1f);
        leftPlayer.Transform.Scale = new Vector3(PaddleSize, PaddleSize, 1f);
        rightPlayer.Transform.Scale = new Vector3(PaddleSize, PaddleSize, 1f);
        
        ball.Transform.Origin = new Vector3(ballTexture.Size.Width / 2, ballTexture.Size.Height / 2, 1);
        leftPlayer.Transform.Origin = new Vector3(paddleTexture.Size.Width / 2, paddleTexture.Size.Height / 2, 1);
        rightPlayer.Transform.Origin = new Vector3(paddleTexture.Size.Width / 2, paddleTexture.Size.Height / 2, 1);

        ball.Transform.Position = new Vector3(300f, 200f, 1f);
        leftPlayer.Transform.Position = new Vector3(paddleTexture.Size.Width * PaddleSize / 2, Graphics.Viewport.Height / 2, 1f);
        rightPlayer.Transform.Position = new Vector3(
            Graphics.Viewport.Width - paddleTexture.Size.Width * PaddleSize / 2, 
            Graphics.Viewport.Height / 2, 
            1f
        );
        
        AddEntity(leftPlayer);
        AddEntity(rightPlayer);
        AddEntity(ball);

    }

    protected override void Update()
    {
        base.Update();

    }

}