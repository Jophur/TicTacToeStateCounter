// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

//  Output
//Unique states 3x3: 5478
//Unique states 3x3, ignoring symmetry: 765
//Unique end states 3x3: 958
//Unique end states 3x3, ignoring symmetry: 138

//Unique states 4x4: 9722011
//Unique states 4x4, ignoring symmetry: 1217977
//Unique end states 4x4: 659392
//Unique end states 4x4, ignoring symmetry: 82763

var count = (int n) =>
{

    TicTacToeStateCounter.TTT t = new(n);

    var states = t.Data();
    int maxPlyes = n * n + 1;
    int[] rawPlyes = new int[maxPlyes];
    int[] uniquePlyes = new int[maxPlyes];
    int uniqueTotal = 0;
    foreach (var state in states)
    {
        ++rawPlyes[state.Value.Ply];
        if (!state.Value.Transform)
        {
            ++uniquePlyes[state.Value.Ply];
            ++uniqueTotal;
        }
    }

    Console.WriteLine("Ply" + '\t' + "Unique(" + n + ")\t" + "NoSymmetries(" + n + ")");
    for (int i = 0; i < maxPlyes; i++)
    {
        Console.Write(i);
        Console.Write('\t');
        Console.Write(rawPlyes[i]);
        Console.Write('\t');
        Console.WriteLine(uniquePlyes[i]);
    }
    Console.Write("Total:" + '\t');
    Console.Write(states.Count);
    Console.Write('\t');
    Console.WriteLine(uniqueTotal);
    Console.WriteLine();
};

for (int i = 1; i <= 4; ++i)
{
    count(i);
}