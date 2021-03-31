using System;
using System.Collections.Generic;
using Reversi.DTO;

namespace Reversi.Common.Board.Events
{
    public class BoardChangedEventArgs : EventArgs
    {
        public BoardChangedEventArgs(eSignMarks[,] i_BoardState, List<TokenLocation> i_AvailableMoves)
        {
            BoardState = i_BoardState;
            AvailableMoves = i_AvailableMoves;
        }

        public readonly eSignMarks[,] BoardState;
        public readonly List<TokenLocation> AvailableMoves;
    }
}
