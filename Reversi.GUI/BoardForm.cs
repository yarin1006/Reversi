using System;
using System.Windows.Forms;
using System.Drawing;
using Reversi.DTO;
using Reversi.Common.Players.Events;

namespace Reversi.GUI
{
    internal class BoardForm : Form
    {
        public event TokenClickEventHandler PlayerSelected;

        private void token_Click_EventHandler(object sender, TokenButton.TokenClickedEventArgs e)
        {
            if (PlayerSelected != null)
            {
                PlayerSelected(this, e);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitializeComponent();
            InitCustomLayout();
        }

        private void InitializeComponent()
        {
            PlayerTurn = new System.Windows.Forms.Label();
            colorDialog1 = new System.Windows.Forms.ColorDialog();
            SuspendLayout();
            PlayerTurn.AutoSize = true;
            PlayerTurn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)177);
            PlayerTurn.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            PlayerTurn.Location = new System.Drawing.Point(12, 233);
            PlayerTurn.Name = "PlayerTurn";
            PlayerTurn.Size = new System.Drawing.Size(143, 29);
            PlayerTurn.TabIndex = 0;
            PlayerTurn.Text = "Player\'s turn";
            ClientSize = new System.Drawing.Size(282, 262);
            Controls.Add(PlayerTurn);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Name = "BoardForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Othello";
            Load += new System.EventHandler(this.BoardForm_Load);
            ResumeLayout(false);
            PerformLayout();
        }

        private void InitCustomLayout()
        {
            foreach (TokenButton button in m_TokenButtons)
            {
                Controls.Add(button);
            }

            ClientSize = new Size(m_TotalSize, m_TotalSize + 100);
            PlayerTurn.Location = new System.Drawing.Point(12, m_TotalSize + 40);
            PlayerTurn.Text = m_CurrentPlayerName + "'s turn!";
        }

        public BoardForm(int i_BoardSize)
        {
            int gameBoardXAxis = 0;
            int gameBoardYAxis = 0;
            m_TotalSize = 0; 
            m_TokenButtons = new TokenButton[i_BoardSize, i_BoardSize];

            for (int rowN = 0; rowN < i_BoardSize; ++rowN)
            {
                for (int colN = 0; colN < i_BoardSize; ++colN)
                {
                    m_TokenButtons[rowN, colN] = new TokenButton(new TokenLocation(rowN, colN));
                    m_TokenButtons[rowN, colN].ClickOccured += new TokenClickEventHandler(token_Click_EventHandler);
                    m_TokenButtons[rowN, colN].Location = new Point(gameBoardXAxis, gameBoardYAxis);

                    if (colN + 1 == i_BoardSize)
                    {
                        gameBoardXAxis = 0;
                        gameBoardYAxis += m_TokenButtons[rowN, colN].Size.Height + 1;
                    }
                    else
                    {
                        gameBoardXAxis += m_TokenButtons[rowN, colN].Size.Width + 1;
                    }
                }

                m_TotalSize += m_TokenButtons[rowN, 0].Size.Width + 1;
            }

            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Othello";
        }

        public void PlayerChanged(object sender, PlayerChangedEventArgs e)
        {
            m_CurrentPlayerName = e.CurrentPlayer.PlayerName;
            if (PlayerTurn != null)
            {
                if (PlayerTurn.Visible)
                {
                    Animation.Animate(PlayerTurn, Animation.Effect.Roll, 150, 360);
                }

                if (e.HasMoves)
                {
                    PlayerTurn.Text = m_CurrentPlayerName + "'s turn!";
                }
                else
                {
                    PlayerTurn.Text = m_CurrentPlayerName + "              no moves switch turn!";
                }

                Animation.Animate(PlayerTurn, Animation.Effect.Roll, 150, 360);
            }
        }

        public void RedrawBoard(eSignMarks[,] i_BoardState)
        {
            if (m_TokenButtons.LongLength != i_BoardState.LongLength)
            {
                throw new ArgumentOutOfRangeException("i_BoardState", "i_BoardState matrix size is different than UI's m_TokenButtons");
            }

            for (int rowN = 0; rowN < i_BoardState.GetLength(0); ++rowN)
            {
                for (int colN = 0; colN < i_BoardState.GetLength(0); ++colN)
                {
                    m_TokenButtons[rowN, colN].SetButtonStatus(i_BoardState[rowN, colN]);
                }
            }
        }

        public void SetAvailableMoveToken(TokenLocation i_SquareLocation)
        {
            m_TokenButtons[i_SquareLocation.RowN, i_SquareLocation.ColN].SetAvailableMove();
        }

        private TokenButton[,] m_TokenButtons;

        private Label PlayerTurn;
        private int m_TotalSize = 0;
        private ColorDialog colorDialog1;
        private string m_CurrentPlayerName;

        private void BoardForm_Load(object sender, EventArgs e)
        {
        }
    }
}