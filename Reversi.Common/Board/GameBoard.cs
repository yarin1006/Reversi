using System;
using Reversi.DTO;

namespace Reversi.Common.Board
{
    public sealed class GameBoard
    {
        public void LocationToken(eSignMarks i_Token, int i_Row, int i_Col, out eSignMarks[,] o_NewBoardStatus, out int o_NumberTokensThatReplaced)
        {
            if (i_Row >= BoardSize || i_Row >= BoardSize || CurrentTableState[i_Row, i_Col] != eSignMarks.Blank)
            {
                throw new System.InvalidOperationException("Ileagal token location");
            }

            o_NumberTokensThatReplaced = 0;
            o_NewBoardStatus = CurrentTableState;
            foreach (eMoveDirection direction in Enum.GetValues(typeof(eMoveDirection)))
            {
                int numberOfTokenReplacedForDirection = 0;
                eSignMarks[,] boardWithDirectionUpdated = null;

                performTokensReplacementForDirection(i_Token, direction, i_Row, i_Col, o_NewBoardStatus, out boardWithDirectionUpdated, out numberOfTokenReplacedForDirection);
                if (numberOfTokenReplacedForDirection > 0 && boardWithDirectionUpdated != null)
                {
                    o_NewBoardStatus = boardWithDirectionUpdated;
                    o_NumberTokensThatReplaced += numberOfTokenReplacedForDirection;
                }
            }
        }

         private void performTokensReplacementForDirection(
         eSignMarks i_Symbol,
         eMoveDirection i_MoveDirection,
         int i_Row,
         int i_Col,
         eSignMarks[,] i_PreviousBoardStatus,
         out eSignMarks[,] o_NewBoardStatus,
         out int o_NumberOfTokenReplaced)
        {
            o_NewBoardStatus = null;
            o_NumberOfTokenReplaced = 0;

            eSignMarks[,] boardStateWithUpdatedDirection = new eSignMarks[BoardSize, BoardSize];
            Array.Copy(i_PreviousBoardStatus, boardStateWithUpdatedDirection, i_PreviousBoardStatus.Length);
            boardStateWithUpdatedDirection[i_Row, i_Col] = i_Symbol;

            int possibleNumberOfTokensReplaced = 0;
            bool AriiveTOBorder;
            eSignMarks previousTokenValue = eSignMarks.Blank;
            int row = i_Row;
            int col = i_Col;
            do
            {
                advanceInMatrix(i_MoveDirection, ref row, ref col, out AriiveTOBorder);
                if (!AriiveTOBorder)
                {
                    previousTokenValue = boardStateWithUpdatedDirection[row, col];
                    boardStateWithUpdatedDirection[row, col] = i_Symbol;
                }
                ++possibleNumberOfTokensReplaced;
            }
            while (previousTokenValue != i_Symbol && previousTokenValue != eSignMarks.Blank && !AriiveTOBorder);

            if (!AriiveTOBorder && previousTokenValue != eSignMarks.Blank && possibleNumberOfTokensReplaced > 1)
            {
                o_NumberOfTokenReplaced = possibleNumberOfTokensReplaced;
                o_NewBoardStatus = boardStateWithUpdatedDirection;
            }
        }

        public void GetTokensBalance(out int o_NumberOfCircles, out int o_NumberOfCrosses)
        {
            o_NumberOfCircles = 0;
            o_NumberOfCrosses = 0;

            foreach (eSignMarks token in CurrentTableState)
            {
                o_NumberOfCircles += (token == eSignMarks.PlayerTwoToken) ? 1 : 0;
                o_NumberOfCrosses += (token == eSignMarks.PlayerOneToken) ? 1 : 0;
            }
        }

        private void advanceInMatrix(eMoveDirection i_Direction, ref int io_CurrentRow, ref int io_CurrentCol, out bool o_ReachedEnd)
        {
            o_ReachedEnd = false;
            int currentRow = io_CurrentRow;
            int currentCol = io_CurrentCol;

            switch (i_Direction)
            {
                case eMoveDirection.Up:
                    o_ReachedEnd = --currentRow < 0;
                    break;
                case eMoveDirection.UpRight:
                    o_ReachedEnd = --currentRow < 0 || ++currentCol >= BoardSize;
                    break;
                case eMoveDirection.Right:
                    o_ReachedEnd = ++currentCol >= BoardSize;
                    break;
                case eMoveDirection.DownRight:
                    o_ReachedEnd = ++currentRow >= BoardSize || ++currentCol >= BoardSize;
                    break;
                case eMoveDirection.Down:
                    o_ReachedEnd = ++currentRow >= BoardSize;
                    break;
                case eMoveDirection.DownLeft:
                    o_ReachedEnd = ++currentRow >= BoardSize || --currentCol < 0;
                    break;
                case eMoveDirection.Left:
                    o_ReachedEnd = --currentCol < 0;
                    break;
                case eMoveDirection.UpLeft:
                    o_ReachedEnd = --currentCol < 0 || --currentRow < 0;
                    break;
            }

            if (!o_ReachedEnd)
            {
                io_CurrentRow = currentRow;
                io_CurrentCol = currentCol;
            }
        }

        public int BoardSize
        {
            get { return CurrentTableState.GetLength(0); }
        }

        public eSignMarks this[int i_Row, int i_Col]
        {
            get { return CurrentTableState[i_Row, i_Col]; }
        }

        public GameBoard(int i_SizeOfTable)
        {
            CurrentTableState = new eSignMarks[i_SizeOfTable, i_SizeOfTable];
            CurrentTableState[(i_SizeOfTable / 2) - 1, (i_SizeOfTable / 2) - 1] = eSignMarks.PlayerTwoToken;
            CurrentTableState[(i_SizeOfTable / 2) - 1, (i_SizeOfTable / 2)] = eSignMarks.PlayerOneToken;
            CurrentTableState[(i_SizeOfTable / 2), (i_SizeOfTable / 2) - 1] = eSignMarks.PlayerOneToken;
            CurrentTableState[(i_SizeOfTable / 2), (i_SizeOfTable / 2)] = eSignMarks.PlayerTwoToken;
        }

        public eSignMarks[,] CurrentTableState { get; set; }
    }
}
