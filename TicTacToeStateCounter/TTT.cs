using System.Text;
using System.Text.RegularExpressions;

namespace TicTacToeReflector
{
    internal class TTT
    {
        // How this works
        // My son asked me last night: How many different Tic Tac Toe boards are there?
        //  What if you remove all the reflections and rotations that are basically the same?
        //  For example if you take the mirror image of a board, it's basically the same board
        //  even though it's technically different

        // We found 7 transformations that are functionally equivalent.
        // For example: 
        //  x|o|x   is functionally equivalent to:  x| |o
        //   | |    in terms of decision making     o| | 
        //  x| |o                                   x| |x
        // The transformations are:
        //  Rotate (counter clockwise) once
        //  Rotate twice
        //  Rotate three times
        //  Flip on vertical axis (MirrorImage in my code)
        //      and flip each of the 3 rotations on the vertical axis

        // So how I do this is I start with a new game (root), and visit all possible moves (nodes)
        //  When I find a node that I haven't seen before, I add it to a list of visited nodes
        //  Then I add all the transformations to the list of visited nodes
        //  When I find an end-state, either because someone won or because there are no more moves,
        //      I add it to the result collection
        //      If it is an end-state because someone won, I stop visiting child nodes

        public enum Winner { None, X, O }
        private const char X = 'x';
        private const char O = 'o';
        private const char Empty = '.';
        private static string newBoard = new(Empty, 9);
        private readonly Dictionary<string, string> AllBoards = new();

        #region Static Methods
        public static string NewBoard { get => newBoard; }

        public static void WriteBoard(string board, TextWriter writer)
        {
            writer.Write(board[0]);
            writer.Write("|");
            writer.Write(board[1]);
            writer.Write("|");
            writer.Write(board[2]);
            writer.Write("\n");
            writer.Write("—––—–\n");
            writer.Write(board[3]);
            writer.Write("|");
            writer.Write(board[4]);
            writer.Write("|");
            writer.Write(board[5]);
            writer.Write("\n");
            writer.Write("—––—–\n");
            writer.Write(board[6]);
            writer.Write("|");
            writer.Write(board[7]);
            writer.Write("|");
            writer.Write(board[8]);
            writer.Write("\n");
        }
        public static IEnumerable<string> Moves(string board)
        {
            // x or o's turn?
            int countx = Regex.Matches(board, X.ToString()).Count;
            int counto = Regex.Matches(board, O.ToString()).Count;
            char player = X;
            // states:
            // #x = #o -> x
            // #o < #x -> o
            if (counto < countx) player = O;
            List<string> result = new();

            // I'm using a StringBuilder because I don't want to figure out if there's a better way
            //  to build a string from char types
            StringBuilder sb = new();
            for(int i = 0; i < board.Length; ++i)
            {
                if (board[i] == Empty)
                {
                    StringBuilder newMove = new();
                    newMove.Append(sb).Append(player).Append(board.AsSpan(i + 1));
                    result.Add(newMove.ToString());
                }
               
                sb.Append(board[i]);
            }
            return result;
        }
        public static Winner Score(string board)
        {
            char winner = Empty;
            if (board[0] == board[1] && board[0] == board[2] && board[0] != Empty) winner = board[0];
            else if (board[3] == board[4] && board[3] == board[5] && board[3] != Empty) winner = board[3];
            else if (board[6] == board[7] && board[6] == board[8] && board[6] != Empty) winner = board[6];
            else if (board[0] == board[3] && board[0] == board[6] && board[0] != Empty) winner = board[0];
            else if (board[1] == board[4] && board[1] == board[7] && board[1] != Empty) winner = board[1];
            else if (board[2] == board[5] && board[2] == board[8] && board[2] != Empty) winner = board[2];
            else if (board[0] == board[4] && board[0] == board[8] && board[0] != Empty) winner = board[0];
            else if (board[2] == board[4] && board[2] == board[6] && board[2] != Empty) winner = board[2];

            Winner result = winner switch
            {
                Empty => Winner.None,
                O => Winner.O,
                X => Winner.X,
                _ => throw new Exception("Unexpected input"),
            };
            return result;
        }
        public static string MirrorImage(string input)
        {
            StringBuilder sb = new();
            sb = sb.Append(input[2]).Append(input[1]).Append(input[0])
                .Append(input[5]).Append(input[4]).Append(input[3])
                .Append(input[8]).Append(input[7]).Append(input[6]);

            return sb.ToString();
        }
        public static string Rotate(string input)
        {
            StringBuilder sb = new();
            sb = sb.Append(input[2]).Append(input[5]).Append(input[8])
                .Append(input[1]).Append(input[4]).Append(input[7])
                .Append(input[0]).Append(input[3]).Append(input[6]);
            return sb.ToString();
        }
        #endregion Static Methods
        public IEnumerable<string> UniqueEndStates()
        {
            List<string> result = new List<string>();
            string board = TTT.NewBoard;
            AddBoard(board, "New Game");
            result.AddRange(UniqueEndStates(board));

            return result;
        }
        private IList<string> UniqueEndStates(string board)
        {   //xoxoxoxxo
            List<string> result = new List<string>();
            var moves = TTT.Moves(board);
            // Is this an end state?
            if (moves.Count() == 0)
                result.Add(board);
            foreach (var move in moves)
            {
                // only consider boards we haven't yet encountered
                if (!AllBoards.ContainsKey(move))
                {
                    string note = "";
                    // is this a winning move?
                    Winner winner = TTT.Score(move);
                    if (winner != Winner.None)
                    {
                        note += winner.ToString() + " wins!";
                        result.Add(move); // this is an end-state
                    }
                    else
                    {
                        note = "player move";
                    }
                    AddBoard(move, note);   // add this move to the list of states we've seen
                    AddTransforms(move);   // add all transforms to the list of states we've seen
                    if (winner == Winner.None)
                    {
                        result.AddRange(UniqueEndStates(move));
                    }
                }
            }
            return result;
        }
        private void AddTransforms(string board)
        {
            AddBoard(TTT.MirrorImage(board), " reflection of " + board);

            string r1 = TTT.Rotate(board);
            AddBoard(r1, "rotation of " + board);
            string r2 = TTT.Rotate(r1);
            AddBoard(r2, "rotation of " + r1);
            string r3 = TTT.Rotate(r2);
            AddBoard(r3, "rotation of " + r2);
            AddBoard(TTT.MirrorImage(r1), " reflection of " + r1);
            AddBoard(TTT.MirrorImage(r2), " reflection of " + r2);
            AddBoard(TTT.MirrorImage(r3), " reflection of " + r3);
        }
        private void AddBoard(string board, string note)
        {
            if (!AllBoards.ContainsKey(board))
            {
                AllBoards.Add(board, note);
            }
        }
    }
}
