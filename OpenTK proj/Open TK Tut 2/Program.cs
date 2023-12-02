// See https://aka.ms/new-console-template for more information

using Open_TK_Tut_1;

Console.WriteLine("Hello, World!");

using (Game myGame = new Game(800, 600, "Intro to Computer Graphics"))
{
    myGame.Run();
}