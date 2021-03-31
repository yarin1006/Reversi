using System;
using System.Collections.Generic;
using Reversi.DTO;
using Reversi.Common.Board.Events;
using Reversi.Common.Players;
using Reversi.Common.Players.Events;

namespace Reveersi.Logic
{
    public sealed class Othello
    {
        public const int k_MaxMatrixSize = 12;
        public const int k_MinMatrixSize = 6;
        private const int k_NumberOfPlayers = 2;

        // The Events for update the display
        public delegate void BoardChangedDelegate(object sender, BoardChangedEventArgs e);

        public delegate void CurrentPlayerChangedDelegate(object sender, PlayerChangedEventArgs e);

        public delegate void GameFinishedChangedDelegate(object sender, GameFinishedEventArgs e);

        public BoardChangedDelegate m_BoardChangedDelegate;

        public CurrentPlayerChangedDelegate m_CurrentPlayerChangedDelegate;

        public GameFinishedChangedDelegate m_GameFinishedDelegate;

        private void onBoardChanged(List<TokenLocation> i_AvailableMoves)
        {
            if (m_BoardChangedDelegate != null)
            {
                BoardChangedEventArgs boardChangedArgs = new BoardChangedEventArgs(m_GameEngine.CurrentTableState, i_AvailableMoves);
                m_BoardChangedDelegate.Invoke(this, boardChangedArgs);
            }
        }

        private void onCurrentPlayerChanged(Player i_CurrentPlayer, bool i_HasAvailableMoves)
        {
            if (m_CurrentPlayerChangedDelegate != null)
            {
                PlayerChangedEventArgs playerChangedArgs = new PlayerChangedEventArgs(i_CurrentPlayer, i_HasAvailableMoves);
                m_CurrentPlayerChangedDelegate.Invoke(this, playerChangedArgs);
            }
        }

        private void onGameFinished(Player i_PlayerOne, int i_PlayerOneCurrentGameScore, Player i_PlayerTwo, int i_PlayerTwoCurrentGameScore)
        {
            if (m_GameFinishedDelegate != null)
            {
                GameFinishedEventArgs gameFinishedArgs = new GameFinishedEventArgs(i_PlayerOne, i_PlayerOneCurrentGameScore, i_PlayerTwo, i_PlayerTwoCurrentGameScore);
                m_GameFinishedDelegate.Invoke(this, gameFinishedArgs);
            }
        }

        public class GameFinishedEventArgs : EventArgs
        {
            public GameFinishedEventArgs(Player i_PlayerOne, int i_PlayerOneCurrentGameScore, Player i_PlayerTwo, int i_PlayerTwoCurrentGameScore)
            {
                PlayerOne = i_PlayerOne;
                PlayerOneCurrentGameScore = i_PlayerOneCurrentGameScore;
                PlayerTwo = i_PlayerTwo;
                PlayerTwoCurrentGameScore = i_PlayerTwoCurrentGameScore;
            }

            public readonly Player PlayerOne;

            public int PlayerOneCurrentGameScore
            {
                get;
                private set;
            }

            public readonly Player PlayerTwo;

            public int PlayerTwoCurrentGameScore
            {
                get;
                private set;
            }
        }

        public Othello()
        {
            m_Players = new Player[k_NumberOfPlayers];
        }

        public void SetInitialSettings(int i_BoardSize, bool IsComputer, string i_PlayerOneName, string i_PlayerTwoName)
        {
            m_BoardSize = i_BoardSize;
            string firstPlayerName = (i_PlayerOneName == null) ? "Black" : i_PlayerOneName;
            string secondPlayerName = (i_PlayerTwoName == null) ? "White" : i_PlayerTwoName;
            m_Players[0] = new HumanVsPlayer(firstPlayerName, eSignMarks.PlayerOneToken);
            if (IsComputer)
            {
                m_Players[1] = new ComputerVsPlayer(secondPlayerName, eSignMarks.PlayerTwoToken);
            }
            else
            {
                m_Players[1] = new HumanVsPlayer(secondPlayerName, eSignMarks.PlayerTwoToken);
            }

            StartNewGame();
        }

        public void StartNewGame()
        {
            m_GameEngine = new GameEngine(this.m_BoardSize);
            m_PresentPlayerTurn = Reveersi.Logic.GameEngine.GetRandom(0, 1) == 0 ? ePlayers.PlayerOne : ePlayers.PlayerTwo;
            playRound();
        }

        public void PlayHumanRound(TokenLocation i_SelectedLocation)
        {
            if (this.m_Players[(int)m_PresentPlayerTurn] is ComputerVsPlayer)
            {
                throw new InvalidOperationException("This method should only be called for human players.");
            }

            int numberOfTokensReplaced;
            eSignMarks[,] newBoardState;
            m_GameEngine.GetHumanPlayerMoveSelection(
                m_Players[(int)m_PresentPlayerTurn],
                (TokenLocation)i_SelectedLocation,
                out newBoardState,
                out numberOfTokensReplaced);
            updateBoardState(newBoardState, null);
            playRound();
        }

        private void getNextPlayer()
        {
            bool hasAvailableMoves;
            changePlayer(out hasAvailableMoves);
            if (hasAvailableMoves)
            {
                m_PresentGameFinished = false;
            }
            else
            {
                changePlayer(out hasAvailableMoves);
                m_PresentGameFinished = !hasAvailableMoves;
            }
        }

        private void changePlayer(out bool o_HasAvailableMoves)
        {
            List<TokenLocation> availableMoves = null;
            m_PresentPlayerTurn = m_PresentPlayerTurn == ePlayers.PlayerOne ? ePlayers.PlayerTwo : ePlayers.PlayerOne;
            m_GameEngine.GetPossibleMoves(m_Players[(int)m_PresentPlayerTurn].TokenMark, out availableMoves);
            o_HasAvailableMoves = availableMoves != null;
            onCurrentPlayerChanged(m_Players[(int)m_PresentPlayerTurn], o_HasAvailableMoves);
        }

        private void playRound()
        {
            getNextPlayer();
            while (!m_PresentGameFinished && m_Players[(int)m_PresentPlayerTurn] is ComputerVsPlayer)
            {
                eSignMarks[,] newBoardState;
                m_GameEngine.GetBestComputerMove(m_Players[(int)m_PresentPlayerTurn].TokenMark, out newBoardState);
                updateBoardState(newBoardState, null);
                getNextPlayer();
            }

            if (m_PresentGameFinished)
            {
                endGame();
            }
            else
            {
                updateMovesForHumanPlayer();
            }
        }

        private void endGame()
        {
            int playerOneScore;
            int playerTwoScore;
            m_GameEngine.GetCurrentScore(m_Players[(int)ePlayers.PlayerOne], m_Players[(int)ePlayers.PlayerTwo], out playerOneScore, out playerTwoScore);
            m_Players[(int)ePlayers.PlayerOne].TotalGamesScore += playerOneScore > playerTwoScore ? 1 : 0;
            m_Players[(int)ePlayers.PlayerTwo].TotalGamesScore += playerTwoScore > playerOneScore ? 1 : 0;
            m_PresentGameFinished = false;
            onGameFinished(m_Players[(int)ePlayers.PlayerOne], playerOneScore, m_Players[(int)ePlayers.PlayerTwo], playerTwoScore);
        }

        private void updateMovesForHumanPlayer()
        {
            List<TokenLocation> availableMoves = null;
            m_GameEngine.GetPossibleMoves(m_Players[(int)m_PresentPlayerTurn].TokenMark, out availableMoves);
            updateBoardState(m_GameEngine.CurrentTableState, availableMoves);
        }

        private void updateBoardState(eSignMarks[,] newBoardState, List<TokenLocation> i_AvailableMoves)
        {
            m_GameEngine.UpdatePresentBoardStatus(newBoardState);
            onBoardChanged(i_AvailableMoves);
        }

        private int m_BoardSize;
        private GameEngine m_GameEngine;
        private Player[] m_Players;
        private ePlayers m_PresentPlayerTurn;
        private bool m_PresentGameFinished;
    }
}
