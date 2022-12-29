using Easel;  
using Easel.Math;  
using EaselPong;

GameSettings gameSettings = new GameSettings()  
{  
    Title = "Pong demo",  
    Size = new Size(600, 400),  
};

using EaselGame game = new EaselGame(gameSettings, new MainScene());

Logger.UseConsoleLogs();
game.Run();
