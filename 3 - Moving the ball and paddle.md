# Creating our own Components
In the previous tutorial, we created entities to represent our game objects, and by using a ``Sprite`` component, drew them to the screen!

Components are how we program behavior and attribute data to the entities we can utilize in our game. To represent this, let's create our Ball and Paddle logic!

In your project, create a new directory called Components. In there, create two files, one named ``Ball.cs`` and another named ``Paddle.cs``.
You should have class definitions inside of them, just like this:

```cs
// Ball.cs
namespace EaselPong.Components;

public class Ball
{
    
}
```
```cs
// Paddle.cs
namespace EaselPong.Components;

public class Paddle
{
    
}
```
## Paddle

We'll start by implementing the paddle component and its behavior. In Easel, components (just like scenes) have their own defined ``Initialize`` and ``Update`` methods that behave the same way. We'll start by inheriting the ``Component`` class. To do this, let's import all of the necessary namespaces that we'll need from Easel beforehand.

At the top of ``Paddle.cs``, import the following:

```cs
using System.Numerics;
using Easel;
using Easel.Entities;
using Pie.Windowing;
using Easel.Entities.Components;
```

Now we'll inherit the ``Component`` class unto our ``Paddle`` to implement it as one, so that we can add this component to any ``Entity`` as we so please.

```cs
//...
namespace EaselPong.Components;

public class Paddle : Component // NEW
{
    
}
//...
```

> Note that in Easel, every single entity can only hold one of each type of component. So for example, every entity can only ever hold one of the ``Paddle`` component that we are currently creating.

Let's also quickly define a ``Side`` enum, so that we have some sort of representation for which side the paddle is on. Right after the namespace declaration, and before the class declaration, define the following.

```cs
//...
namespace EaselPong.Components;

// NEW
public enum Side
{
    Left,
    Right
}

//...
```
Now we'll declare some variables that we'll utilize to define some details about each paddle.

```cs
//...
public class Paddle : Component
{
    // NEW
    public Side PaddleSide;
    private float speed;
    private float velocity;
    public Vector2 PaddleSize;
    public int Score;

    // The set keybinds to use to control either side.
    private Key[] leftPlayerInputs = { Key.W, Key.S };
    private Key[] rightPlayerInputs = { Key.Up, Key.Down };

//...
/...
```

The declared variables are relatively self explanatory, but we'll go over them anyway. ``PaddleSide`` is used to store which side each paddle is on, as represented using the ``Side`` enum defined earlier. The speed and velocity of the component is stored as floating point integers, and we'll use them to manipulate the position of the paddles as we so please. The size of the paddle is stored as a ``Vector2`` (under ``System.Numerics``) which handily has two accessible values that we can use for the X and Y sides. Finally, we're storing the scores for each player on their respective paddles, as well as defining which buttons are going to trigger the inputs of the paddles.

> Quick Note: If you want to modify the keybinds for moving the paddles, feel free to do so! ``Pie.Windowing``'s ``Key`` enum stores a definition for just about every keyboard key out there! Just substitute the values in the inputs array for either player.

Next, let's actually define these values. Create a constructor for our ``Paddle`` component, so that the respective side and the intended speed can be defined.

```cs
//...
// The set keybinds to use to control either side.
private Key[] leftPlayerInputs = { Key.W, Key.S };
private Key[] rightPlayerInputs = { Key.Up, Key.Down };

// NEW
public Paddle(Side side, float speed)
{
    PaddleSide = side;
    this.speed = speed;
    Score = 0;
}

//...
```

Now, we'll define the values that need to be known upon the Scene's initialization, rather than when the component is created. We want to make sure our gameobjects exist, and we know that everything must be loaded when the game starts up, vs when our object is created. To do this, we'll utilize the component's ``Initialize`` method. After the constructor, override it as so:

```cs
//...

protected override void Initialize()
{
    base.Initialize();
    // We store paddleSize so that we can use it when checking collisions in the ball component
    // (otherwise, we'd have to call getcomponent<sprite>() per frame)
    var paddleSize = Entity.GetComponent<Sprite>().Texture.Size;

    // We also want to adjust the paddle size for any scaling we might have done
    PaddleSize = new Vector2(paddleSize.Width * Transform.Scale.X, 
        paddleSize.Height * Transform.Scale.Y);
}

//...
```

Now we can define our game logic! The only things each paddle needs to do in a game of pong is to exist as something to collide with, and to be able to move up and down. We'll define this logic in our ``Update`` method, which is called once every frame. (Don't worry, we'll break all of this down.)

```cs
//...
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
//...
```

We'll deconstruct this logic by going line by line. First, you'll notice that we set the velocity to 0 every frame. This is so that whenever we set the velocity by pressing a key, we're resetting it to not move when a key isn't being pressed. Otherwise, if we set the velocity to a positive value, it would just continually slide upwards forever.

Afterwards, we define a switch statement based on the paddle's side, so that we can differentiate which input keybind array we want to use. We know that the first element of the array is the key to go up, and the second element is the key to go down, so we set the velocity of the paddle accordingly. 

> Quick Note: Remember from chapter 2 that in graphics programming, the origin, or (0, 0) is usually set at the top left, so decreasing the Y value actually means we go up, or closer to 0, rather than down, like you'd think in maths. You can revisit the [Transform](2%20-%20Creating%20Game%20Objects.md/#Transform) section if you feel like this is unclear.


## Ball

Just like for our paddle, at the top of ``Ball.cs``, import the following:
```cs
using System.Numerics;

using Easel;
using Easel.Entities;
using Easel.Scenes;
using Easel.Entities.Components;
```




On our ``Ball`` class, inherit the ``Component`` class.

```cs
//...
namespace EaselPong.Components;

public class Ball : Component
{
    
}
//...
```
To begin implementation, we'll declare some variables to use to store important data that we'll need the ball to have during gameplay. In our ``Ball`` class, declare the following:

```cs
//...
public class Ball : Component
{
    private Vector3 velocity;
    private float speed;
    private float radius;
    private Entity[] paddles;

}
```
We'll also create a constructor for our class defining the predetermined values we want on the ``Ball`` component's creation. 

```cs
//...
private Entity[] paddles;

// NEW
public Ball(float radius, float speed)
{
    this.radius = radius;
    this.speed = speed;
}
```
The ball's velocity will be decided during gameplay and change throughout, so we'll leave it be for now. 

We also defined an ``Entity`` array variable to store references to our paddles, so that we don't need to constantly need to grab the entities with paddles every frame. We're defining this variable in ``Initialize`` and not the constructor though, so that we can safely grab all of our gameobjects upon the game starting up, and not on the ``Ball``'s creation. 

### Tags
In order to tell our program what entities are paddles, we'll set the ``Tag`` property of the ``Entity`` objects we're to set our paddles on. In ``MainScene.cs``, right under where we instantiated our entities, we'll set a "paddle" tag to our paddle entities:
```cs
protected override void Initialize()
{
    //...
    // We create our paddle and ball entities, add appropriate sprite and associated components, then add them to the scene.  
    ball = new Entity();  
    leftPlayer = new Entity();  
    rightPlayer = new Entity();  

    // NEW
    leftPlayer.Tag = "paddle";
    rightPlayer.Tag = "paddle";

    //...
}
```
Back on implementing our ``Ball`` component, let's start working on our ``Initialize`` and ``Update`` methods. In ``Ball.cs``, override your ``Initialize`` method right under you constructor.

```cs
protected override void Initialize()
{
    base.Initialize();
    paddles = GetEntitiesWithTag("paddle");
}
```

Here, we're using the ``GetEntitiesWithTag`` function under the ``Entity`` class to grab the entities we associated with that tag just earlier.


Moving on to base game logic, we'll want to write up our ball's behavior in our ``Update`` function.

```cs
protected override void Update()
{
    base.Update();
    Transform.Position += velocity * Time.DeltaTime;

    
    // Whenever the ball hits the ceiling or floor, make it bounce. 
    // the if statements are split up so we can just set it to a position where it can't get stuck.
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
}
```

First, the line ```Transform.Position += velocity * Time.DeltaTime;``` applies an actual change to the current position, with the speed being the intended velocity of the object, equalized with ``Time.DeltaTime``. 

``Time.DeltaTime`` is the approximate time between frames, given by the engine, which we use here in order to make sure the ball travels the same distance in a given amount of time for the same velocity. This means that regardless of if the game runs at 60 fps, or 300 fps, the ball will travel at the same speed no matter what we set our velocity to.

Whenever the ball hits the top or bottom of the screen, we want to make it bounce. We can easily achieve this by simply setting the ball's position to the border of the screen, then reversing the velocity of the ball on the Y axis, as written above.

