// See https://aka.ms/new-console-template for more information
 //Console.WriteLine("Hello, World!");

TicTacToeReflector.TTT t = new();
var boards = t.UniqueEndStates();
foreach(var board in boards)
{
    //TicTacToeReflector.TTT.WriteBoard(board, Console.Out);
    Console.WriteLine(board);
}
