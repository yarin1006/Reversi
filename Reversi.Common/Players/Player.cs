using Reversi.DTO;
using Reversi.Common.Board;

namespace Reversi.Common.Players
{
    /// <summary>
    ///   This struct is used to keep player records. No logic is used in these structs because GameEngine controlls AI and Human decides on moves by himself.
    /// </summary>
    public abstract class Player
    {
        /// <summary>
        ///   Methods
        /// </summary>
        public Player(string i_Name, eSignMarks i_TokenMark)
        {
            PlayerName = i_Name;
            TokenMark = i_TokenMark;
            TotalGamesScore = 0;
        }

        public readonly string PlayerName;
        public readonly eSignMarks TokenMark;

        public int TotalGamesScore { get; set; }

        public abstract void GetPlayerMove(
        GameBoard i_PresentBoard,
        TokenLocation i_SelectedPlace,
        out eSignMarks[,] o_NewBoardStatus,
        out int o_NumberTokensThatReplaced);
    }
}
