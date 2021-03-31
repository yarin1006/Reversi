using System;

namespace Reversi.Common.Players.Events
{
    public class PlayerChangedEventArgs : EventArgs
    {
        public PlayerChangedEventArgs(Player i_Player, bool i_HasAvailableMoves)
        {
            CurrentPlayer = i_Player;
            HasMoves = i_HasAvailableMoves;
        }

        public readonly Player CurrentPlayer;
        public readonly bool HasMoves;
    }
}
