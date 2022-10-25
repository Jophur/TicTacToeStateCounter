// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

    //  Output
    //Unique states: 5478
    //Unique states, ignoring symmetry: 765
    //Unique end states: 958
    //Unique end states, ignoring symmetry: 138


TicTacToeReflector.TTT t = new();
var boards = t.UniqueStates(false);
Console.WriteLine("Unique states: " + boards.Count());

boards = t.UniqueStates(true);
Console.WriteLine("Unique states, ignoring symmetry: " + boards.Count());

boards = t.UniqueEndStates(false);
Console.WriteLine("Unique end states: " + boards.Count());

boards = t.UniqueEndStates(true);
Console.WriteLine("Unique end states, ignoring symmetry: " + boards.Count());
