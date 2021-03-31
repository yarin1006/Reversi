using System;
using System.Windows.Forms;
using System.Drawing;
using Reversi.DTO;

namespace Reversi.GUI
{
    internal delegate void TokenClickEventHandler(object sender, TokenButton.TokenClickedEventArgs e);

    internal class TokenButton : Button
    {
        public const int k_ButtonSize = 50;
        public const int k_ThickBorder = 3;
        public const int k_ThinBorder = 1;

        public event TokenClickEventHandler ClickOccured;

        protected override void OnMouseEnter(EventArgs e)
        {
            OnEnter(e);
            if (Enabled)
            {
                FlatAppearance.BorderColor = Color.Turquoise;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            OnEnter(e);
            if (Enabled)
            {
                FlatAppearance.BorderColor = Color.White;
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            SolidBrush drawBrush = new SolidBrush(ForeColor); 
            if (!Enabled && BackColor != SystemColors.Control)
            {
                pevent.Graphics.DrawString("O", Font, drawBrush, (ClientSize.Width / 2) - Font.Size, (ClientSize.Height / 2) - this.Font.Size);
            }
        }

        public class TokenClickedEventArgs : EventArgs
        {
            public TokenClickedEventArgs(TokenLocation i_TokenLocation)
            {
                m_TokenLocation = i_TokenLocation;
            }

            public TokenLocation TokenLocation
            {
                get { return m_TokenLocation; }
            }

            private TokenLocation m_TokenLocation;
        }

        protected override void OnClick(EventArgs e)
        {
            if (ClickOccured != null)
            {
                ClickOccured(this, m_TokenLocationEventArgs);
            }
        }

        public TokenButton(TokenLocation i_TokenLocation)
        {
            m_TokenLocationEventArgs = new TokenClickedEventArgs(i_TokenLocation);
            ClientSize = new Size(k_ButtonSize, k_ButtonSize);
            SetStyle(ControlStyles.UserPaint, true);
        }

        public void SetButtonStatus(eSignMarks i_Token)
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = k_ThickBorder;
            FlatAppearance.BorderColor = Color.White;

            switch (i_Token)
            {
                case eSignMarks.Blank:
                    BackColor = SystemColors.Control;
                    FlatAppearance.BorderSize = k_ThinBorder;
                    FlatAppearance.BorderColor = Color.Gray;
                    break;
                case eSignMarks.PlayerTwoToken:
                    BackColor = Color.White;
                    ForeColor = Color.Black;
                    break;
                case eSignMarks.PlayerOneToken:
                    BackColor = Color.Black;
                    ForeColor = Color.White;
                    break;
                default:
                    throw new ArgumentException("Unrecognized mark found in board state");
            }

            Enabled = false;
        }

        public void SetAvailableMove()
        {
            Enabled = true;
            BackColor = Color.Green;
            FlatAppearance.BorderSize = k_ThickBorder;
            FlatAppearance.BorderColor = Color.White;
        }

        private readonly TokenClickedEventArgs m_TokenLocationEventArgs; // No need to recreate event args, button location doesn't change. 
    }
}
