using System;
using Reversi.DTO;
using Reversi.Common.Board;

namespace Reversi.Common.Players
{
    public class HumanVsPlayer : Player 
    {
        public HumanVsPlayer(string i_Name, eSignMarks i_TokenMark)
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
            i_PresentBoard.LocationToken(TokenMark, i_SelectedPlace.RowN, i_SelectedPlace.ColN, out o_NewBoardStatus, out o_NumberTokensThatReplaced);
            if (o_NumberTokensThatReplaced <= 0 || o_NewBoardStatus == null)
            {
                throw new ArgumentException("Method was supposed to be called with legal move but was called with invalid token placment");
            }
        }
    }
}
