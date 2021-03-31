using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Reversi.GUI
{
    internal class GameSettingsForm : Form
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            initializeComponent();
            InitCustomLayout();
        }

        internal GameSettingsForm()
        {
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void SetBoardSizeButtonText()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameSettingsForm));
            string resourceString = resources.GetString("m_ButtonBoardSize.Text");
            m_ButtonBoardSize.Text = string.Format(resourceString, m_RequestedBoardSize);
        }

        private void m_ButtonBoardSize_Click(object sender, EventArgs e)
        {
            if (m_RequestedBoardSize >= 12)
            {
                m_RequestedBoardSize = 6;
            }
            else
            {
                m_RequestedBoardSize += 2;
            }

            SetBoardSizeButtonText();
        }

        private void m_ButtonStartGame_Click(object sender, EventArgs e)
        {
            Button buttonClicked = sender as Button;
            IsComputerOpponent = buttonClicked.Name == "m_ButtonPlayAgainstComputer";
            DialogResult = DialogResult.OK;
            Close();
        }

        private void initializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameSettingsForm));
            m_ButtonBoardSize = new System.Windows.Forms.Button();
            m_ButtonPlayAgainstComputer = new System.Windows.Forms.Button();
            m_ButtonPlayAgainstHuman = new System.Windows.Forms.Button();
            SuspendLayout();
            resources.ApplyResources(m_ButtonBoardSize, "m_ButtonBoardSize");
            m_ButtonBoardSize.Name = "m_ButtonBoardSize";
            m_ButtonBoardSize.Click += new System.EventHandler(m_ButtonBoardSize_Click);
            resources.ApplyResources(m_ButtonPlayAgainstComputer, "m_ButtonPlayAgainstComputer");
            m_ButtonPlayAgainstComputer.Name = "m_ButtonPlayAgainstComputer";
            m_ButtonPlayAgainstComputer.Click += new System.EventHandler(m_ButtonStartGame_Click);
            resources.ApplyResources(m_ButtonPlayAgainstHuman, "m_ButtonPlayAgainstHuman");
            m_ButtonPlayAgainstHuman.Name = "m_ButtonPlayAgainstHuman";
            m_ButtonPlayAgainstHuman.Click += new System.EventHandler(m_ButtonStartGame_Click);
            resources.ApplyResources(this, "$this");
            Controls.Add(m_ButtonBoardSize);
            Controls.Add(m_ButtonPlayAgainstComputer);
            Controls.Add(m_ButtonPlayAgainstHuman);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "GameSettingsForm";
            ResumeLayout(false);
        }

        private void InitCustomLayout()
        {
            m_RequestedBoardSize = 6;
            SetBoardSizeButtonText();
        }

        public int RequestedBoardSize
        {
            get
            {
                return m_RequestedBoardSize;
            }
        }

        private Button m_ButtonBoardSize;
        private Button m_ButtonPlayAgainstComputer;
        private Button m_ButtonPlayAgainstHuman;
        private int m_RequestedBoardSize;

        public bool IsComputerOpponent
        {
            get;
            private set;
        }
    }
}
