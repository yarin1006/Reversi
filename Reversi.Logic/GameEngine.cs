using System.Collections.Generic;
using System;
using Reversi.DTO;
using Reversi.Common.Board;
using Reversi.Common.Players;

namespace Reveersi.Logic
{
    public sealed class GameEngine
    {
        public GameEngine(int i_TableSize)
        {
            m_Board = new GameBoard(i_TableSize);
        }

        public static int GetRandom(int min, int max)
        {
            lock (s_RandLock)
            {
                return s_RandMechanisem.Next(min, max);
            }
        }

        public void GetPossibleMoves(
            eSignMarks i_Token,
            out List<TokenLocation> o_AvailableMoves)
        {
            o_AvailableMoves = null;
            for (int rowN = 0; rowN < m_Board.BoardSize; ++rowN)
            {
                for (int colN = 0; colN < m_Board.BoardSize; ++colN)
                {
                    if (m_Board[rowN, colN] == eSignMarks.Blank)
                    {
                        int numberOfTokensReplaced;
                        eSignMarks[,] boardStateAfterPlacingToken;
                        m_Board.LocationToken(i_Token, rowN, colN, out boardStateAfterPlacingToken, out numberOfTokensReplaced);
                        if (numberOfTokensReplaced > 0)
                        {
                            if (o_AvailableMoves == null)
                            {
                                o_AvailableMoves = new List<TokenLocation>();
                            }

                            o_AvailableMoves.Add(new TokenLocation(rowN, colN));
                        }
                    }
                }
            }
        }

        public void GetBestComputerMove(
            eSignMarks i_Token,
            out eSignMarks[,] o_BoardStateInBestMove)
        {
            int numberOfTokensReplacedInBestMove = 0;
            o_BoardStateInBestMove = null;
            for (int rowN = 0; rowN < m_Board.BoardSize; ++rowN)
            {
                for (int colN = 0; colN < m_Board.BoardSize; ++colN)
                {
                    if (m_Board[rowN, colN] == eSignMarks.Blank)
                    {
                        int numberOfTokensReplaced;
                        eSignMarks[,] boardStateAfterPlacingToken;
                        m_Board.LocationToken(i_Token, rowN, colN, out boardStateAfterPlacingToken, out numberOfTokensReplaced);

                        if (moveIsBetter(numberOfTokensReplaced, numberOfTokensReplacedInBestMove))
                        {
                            o_BoardStateInBestMove = boardStateAfterPlacingToken;
                            numberOfTokensReplacedInBestMove = numberOfTokensReplaced;
                        }
                    }
                }
            }
        }

        private bool moveIsBetter(int numberOfTokensReplaced, int numberOfTokensReplacedInBestMove)
        {
            return (numberOfTokensReplaced > numberOfTokensReplacedInBestMove) ||
                    ((numberOfTokensReplaced == numberOfTokensReplacedInBestMove) &&
                    (numberOfTokensReplaced > 0) &&
                    (GetRandom(0, 1) == 0));
        }      
             
        public void UpdatePresentBoardStatus(eSignMarks[,] i_NewBoardState)
        {
            m_Board.CurrentTableState = i_NewBoardState;
        }

        public void GetHumanPlayerMoveSelection(
            Player i_HumanPlayer,
            TokenLocation i_SelectedLocation,
            out eSignMarks[,] o_NewBoardState,
            out int o_NumberOfTokensReplaced)
        {
            i_HumanPlayer.GetPlayerMove(m_Board, i_SelectedLocation, out o_NewBoardState, out o_NumberOfTokensReplaced);
        }

        public void GetCurrentScore(Player i_PlayerOne, Player i_PlayerTwo, out int i_PlayerOneScore, out int i_PlayerTwoScore)
        {
            int numberOfCircles;
            int numberOfCrosses;
            m_Board.GetTokensBalance(out numberOfCircles, out numberOfCrosses);

            if (i_PlayerOne.TokenMark == eSignMarks.PlayerTwoToken)
            {
                i_PlayerOneScore = numberOfCircles;
                i_PlayerTwoScore = numberOfCrosses;
            }
            else
            {
                i_PlayerOneScore = numberOfCrosses;
                i_PlayerTwoScore = numberOfCircles;
            }
        }

        public eSignMarks[,] CurrentTableState
        {
            get { return m_Board.CurrentTableState; }
        }

        private static readonly object s_RandLock = new object();
        private static readonly Random s_RandMechanisem = new Random();
        private GameBoard m_Board = null;
    }
}
