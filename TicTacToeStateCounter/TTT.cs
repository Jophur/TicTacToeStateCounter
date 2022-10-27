using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace TicTacToeStateCounter
{
    /// <summary>
    /// Calculates all possible Tic Tac Toe Stats for games of length n x n
    /// NOT Threadsafe!
    /// </summary>
    public class TTT
    {
        public enum Status { Unfinished, X_Won, O_Won, Tie }
        public struct Metadata
        {
            public Metadata(Status status, bool transform, int ply)
            {
                Status = status;
                Transform = transform;
                Ply = ply;
            }
            public Status Status { get; }
            public bool Transform { get; }
            public int Ply { get; }
        }
        private enum Note { New, Move, Win, Transform}
        private const char X = 'x';
        private const char O = 'o';
        private const char Empty = '.';
        private readonly string newBoard;
        private readonly int N;
        private readonly Dictionary<string, Metadata> AllBoards = new();


        #region Constructor
        /// <summary>
        /// Instantiates a new state calculator of Tic Tac Toe boards of size nxn
        /// </summary>
        /// <param name="n"></param>
        public TTT(int n)
        {
            this.newBoard =  new(Empty, n * n);
            this.N = n;
        }
        #endregion

        #region Static Methods
        public static IEnumerable<string> Moves(string board)
        {
            // x or o's turn?
            int countx = Regex.Matches(board, X.ToString()).Count;
            int counto = Regex.Matches(board, O.ToString()).Count;
            char player = X;
            // possible states:
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

        public static void WriteBoard(TTT ttt, string board, TextWriter writer)
        {
            Debug.Assert(board != null);
            Debug.Assert(board.Length == ttt.N * ttt.N);
            string spaced = board.Replace(Empty, ' ');
            // TODO: make this work for any N
            for (int row = 0; row < ttt.N; ++row)
            {
                if(0 < row)
                {
                    writer.WriteLine(new string('—', 2 * ttt.N - 1));
                }

                for (int col = 0; col < ttt.N; ++col)
                {
                    if (0 < col)
                        writer.Write('|');
                    writer.Write(spaced[col]);
                }
                writer.WriteLine();
            }
        }
        /// <summary>
        /// returns the number of turns required to reach this state
        /// </summary>
        /// <param name="ttt"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public static int Ply(TTT ttt, string board)
        {
            Debug.Assert(board != null);
            Debug.Assert(board.Length == ttt.N * ttt.N);
            // emtpy
            int blanks = board.Count(f => f == Empty);

            return ttt.N * ttt.N - blanks;
        }
        /// <summary>
        /// Determine if there's a winner
        /// </summary>
        /// <param name="ttt"></param>
        /// <param name="board"></param>
        /// <returns>Winner.X, Winner.O, or Winner.None</returns>
        /// <exception cref="Exception"></exception>
        public static Status GetStatus(TTT ttt, string board)
        {
            Debug.Assert(board != null);
            Debug.Assert(board.Length == ttt.N * ttt.N);
            char streak = Empty;
            // check each row
            for (int row = 0; row < ttt.N; ++row)
            {
                int col = 0;
                streak = board[row * ttt.N + col];
                ++col;
                while (col < ttt.N && streak != Empty)
                {
                    if (board[row * ttt.N + col] != streak)
                        streak = Empty;
                    ++col;
                }
                if (streak != Empty)
                    break;
            }
            // check each column
            if (streak == Empty)
            {
                for (int col = 0; col < ttt.N; ++col)
                {
                    int row = 0;
                    streak = board[row * ttt.N + col];
                    ++row;
                    while (row < ttt.N && streak != Empty)
                    {
                        if (board[row * ttt.N + col] != streak)
                            streak = Empty;
                        ++row;
                    }
                    if (streak != Empty)
                        break;
                }
            }
            // check diagonal 1
            if (streak == Empty)
            {
                int i = 0;
                streak = board[i * ttt.N + i];
                ++i;
                while (i < ttt.N && streak != Empty)
                {
                    if (board[i * ttt.N + i] != streak)
                        streak = Empty;
                    ++i;
                }
            }
            // check diagonal 2
            if (streak == Empty)
            {
                int i = 0;
                streak = board[i * ttt.N + (ttt.N - 1) - i];
                ++i;
                while (i < ttt.N && streak != Empty)
                {
                    if (board[i * ttt.N + (ttt.N - 1) - i] != streak)
                        streak = Empty;
                    ++i;
                }
            }
            // If someone won, return the winner
            if (streak != Empty) {
                return streak switch
                {
                    O => Status.O_Won,
                    X => Status.X_Won,
                    _ => throw new Exception("Unexpected input"),

                };
            }
            // If there are no empty spaces, the game is a tie
            if (board.IndexOf(Empty) == -1)
                return Status.Tie;

            // Otherwise it is unfinished
            return Status.Unfinished;
        }
        public static string MirrorImage(TTT ttt, string input)
        {
            Debug.Assert(input != null);
            Debug.Assert(input.Length <= ttt.N * ttt.N);
            StringBuilder sb = new();
            for (int row = 0; row < ttt.N; ++row)
            {
                for (int col = ttt.N - 1; 0 <= col; --col)
                {
                    sb.Append(input[row * ttt.N + col]);
                }
            }
            return sb.ToString();
        }
        public static string Rotate(TTT ttt, string input)
        {
            Debug.Assert(input != null);
            Debug.Assert(input.Length <= ttt.N * ttt.N);
            StringBuilder sb = new();
            for (int col = ttt.N - 1; 0 <= col; --col)
            {
                for (int row = 0; row < ttt.N; ++row)
                {
                    int index = row * ttt.N + col;
                    char c = input[index];
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        #endregion Static Methods

        #region Public methods
        public IReadOnlyDictionary<string, Metadata> Data()
        {
            if (AllBoards.Count == 0)
            {
                CountStates();
            }
            return this.AllBoards;
        }
        

        /// <summary>
        /// Returns a list of all possible states
        /// </summary>
        /// <param name="ignoreSymmetry">If true, then states which are symmetrical will not be included</param>
        /// <returns></returns>
        public IEnumerable<string> UniqueStates(bool ignoreSymmetry)
        {
            List<string> result = new();
            if (AllBoards.Count == 0)
            {
                CountStates();
            }
            foreach (var kvp in AllBoards)
            {
                // Truth table for the symmetry inclusion:
                // include?         true    true    true    false
                // ignoreSymmetry   false   false   true    true
                // Transform        false   true    false   true
                // So !(ignoreSymmetry && Transform)
                if (!(ignoreSymmetry && kvp.Value.Transform))
                {
                    result.Add(kvp.Key);
                }
            }
            return result;
        }
        
        /// <summary>
        /// Same as UniqueEndStates(true); I just didn't want to refactor my code
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> UniqueEndStates()
        {
            return UniqueEndStates(true);
        }
        /// <summary>
        /// Finds all unique end states. If ignoreSymmetry is true, then an x in the upper right corner is a duplicate of an x in the upper left corner
        /// </summary>
        /// <param name="ignoreSymmetry"></param>
        /// <returns></returns>
        public IEnumerable<string> UniqueEndStates(bool ignoreSymmetry)
        {
            List<string> result = new();
            if (AllBoards.Count == 0)
            {
                CountStates();
            }
            foreach(var kvp in AllBoards)
            {
                // Truth table for the symmetry inclusion:
                // include?         true    true    true    false
                // ignoreSymmetry   false   false   true    true
                // Transform        false   true    false   true
                // So !(ignoreSymmetry && Transform)
                if (kvp.Value.Status != Status.Unfinished
                    && !(ignoreSymmetry && kvp.Value.Transform))
                {
                    result.Add(kvp.Key);
                }
            }
            return result;
        }
        #endregion Public
        #region Private Methods
        /// <summary>
        /// Starts with a new game and counts all states;
        /// </summary>
        private void CountStates()
        {
            this.AllBoards.Clear();
            Queue<string> queue = new Queue<string>();
            string board = this.newBoard;
            queue.Enqueue(board);
            while(0 < queue.Count)
            {
                board = queue.Dequeue();
                if (!AllBoards.ContainsKey(board))
                {
                    Status status = GetStatus(this, board);
                    int ply = TTT.Ply(this, board);
                    // Add this board to the list
                    AddBoard(board, new Metadata(status, false, ply));
                    AddTransforms(board, status, ply);
                    if(status == Status.Unfinished)
                    {
                        var moves = Moves(board);
                        foreach(var move in moves)
                            queue.Enqueue(move);
                    }
                }
            }
        }

        private void AddTransforms(string board, Status status, int ply)
        {
            Metadata metadata = new Metadata(status, true, ply);
            AddBoard(MirrorImage(this, board), metadata);

            string r1 = Rotate(this, board);
            AddBoard(r1, metadata);
            string r2 = Rotate(this, r1);
            AddBoard(r2, metadata);
            string r3 = Rotate(this, r2);
            AddBoard(r3, metadata);
            AddBoard(MirrorImage(this, r1), metadata);
            AddBoard(MirrorImage(this, r2), metadata);
            AddBoard(MirrorImage(this, r3), metadata);
        }
        private void AddBoard(string board, Metadata metadata)
        {
            if (!AllBoards.ContainsKey(board))
                AllBoards.Add(board, metadata);
        }
        #endregion Private Methods
    }
}
