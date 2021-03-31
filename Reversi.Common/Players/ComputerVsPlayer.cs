using Reversi.DTO;
using Reversi.Common.Board;

namespace Reversi.Common.Players
{
    public class ComputerVsPlayer : Player
    {
        public ComputerVsPlayer(string i_Name, eSignMarks i_TokenMark)
            : base(i_Name, i_TokenMark)
        {
        }

        public override void GetPlayerMove(
            GameBoard i_PresentBoard,
            TokenLocation i_SelectedPlace,
            out eSignMarks[,] o_NewBoardStatus,
            out int o_NumberTokensThatReplaced)
        {
            o_NewBoardStatus = null;
            o_NumberTokensThatReplaced = 0;
        }
    }
}
