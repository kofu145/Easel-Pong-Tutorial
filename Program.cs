// See https://aka.ms/new-console-template for more information

using Easel;
using Easel.Math;
using EaselPongTutorial;

GameSettings gameSettings = new GameSettings()
{
    Title = "Pong demo",
    Size = new Size(600, 400),
};

Logger.UseConsoleLogs();
using EaselGame game = new EaselGame(gameSettings, new MainScene());

game.Run();


Console.WriteLine("Hello, World!");