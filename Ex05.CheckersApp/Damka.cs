using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ex05.CheckersLogic;

namespace Ex05.CheckersApp
{
    internal partial class Damka : Form
    {
        private const int CELL_WIDTH_AND_HEIGHT = 70;
        private const int TOP_OFFSET = 80;
        private GameManager m_GameManager;
        private CheckerButton m_CheckedCheckerButton;
        private DamkaBoard m_Board;
        private Move[] m_AvailableMoves;
        private CheckerLabel m_BlackPlayerNameLabel,
                      m_BlackPlayerScoreLabel,
                      m_RedPlayerNameLabel,
                      m_RedPlayerScoreLabel,
                      m_ActivePlayerLabel;

        private BoardSize m_BoardSize
        {
            get
            {
                return (BoardSize)GameManager.Board.BoardSize;
            }
        }
        private GameMode m_GameMode
        {
            get
            {
                return GameManager.RedPlayer is HumanPlayer ? GameMode.PVP : GameMode.PVC;
            }
        }


        internal GameManager GameManager
        {
            get
            {
                return m_GameManager;
            }

            set
            {
                m_GameManager = value;
            }
        }

        internal Move[] AvailableMoves
        {
            get
            {
                return m_AvailableMoves;
            }

            set
            {
                m_AvailableMoves = value;
                m_Board.ResetAvailableMoves();

                if (m_AvailableMoves != null)
                {
                    m_Board.ShowAvailableMoves(m_AvailableMoves);
                }
            }
        }

        internal CheckerLabel BlackPlayerNameLabel
        {
            get
            {
                return m_BlackPlayerNameLabel;
            }

            set
            {
                m_BlackPlayerNameLabel = value;
            }
        }

        internal CheckerLabel BlackPlayerScoreLabel
        {
            get
            {
                return m_BlackPlayerScoreLabel;
            }

            set
            {
                m_BlackPlayerScoreLabel = value;
            }
        }

        internal CheckerLabel RedPlayerNameLabel
        {
            get
            {
                return m_RedPlayerNameLabel;
            }

            set
            {
                m_RedPlayerNameLabel = value;
            }
        }

        internal CheckerLabel RedPlayerScoreLabel
        {
            get
            {
                return m_RedPlayerScoreLabel;
            }

            set
            {
                m_RedPlayerScoreLabel = value;
            }
        }

        internal CheckerLabel ActivePlayerLabel
        {
            get
            {
                return m_ActivePlayerLabel;
            }

            set
            {
                m_ActivePlayerLabel = value;
            }
        }

        internal CheckerButton CheckedCheckerButton
        {
            get
            {
                return m_CheckedCheckerButton;
            }

            set
            {
                if (value == null && m_CheckedCheckerButton != null)
                {
                    m_CheckedCheckerButton.Clicked = false;
                    AvailableMoves = null;
                }

                m_CheckedCheckerButton = value;

                if (m_CheckedCheckerButton != null)
                {
                    m_CheckedCheckerButton.Clicked = true;
                    AvailableMoves = GameManager.GetPossibleMovesForChecker(m_CheckedCheckerButton.LocationOnMatrix);
                }
            }
        }




        public Damka(GameSettings i_Settings)
        {
            InitializeComponent();
            setupGame(i_Settings);
        }

        private void setupGame(GameSettings i_Settings)
        {
            GameManager = new GameManager(i_Settings);
            setupFormSize();
            setupLabels();
            setupBoard();
            subscribeToEvents();
        }

        private void subscribeToEvents()
        {
            GameManager.BoardChanged += m_Board.DamkaBoard_BoardChanged;
            GameManager.TurnChanged += Damka_TurnChanged;
            GameManager.GameEnded += Damka_GameEnded;
        }

        private void Damka_TurnChanged()
        {
            if (m_GameMode == GameMode.PVP)
            {
                updateActivePlayerLabel();
            }

            GameManager.ActivePlayer.PlayTurn();
        }

        private void updateActivePlayerLabel()
        {
            ActivePlayerLabel.Text = string.Format("{0}'s turn", GameManager.ActivePlayer.Name);
            ActivePlayerLabel.ForeColor = GameManager.ActivePlayer.Color == eCheckerColor.Black ? Color.Black : Color.FromArgb(182, 0, 0);
        }

        private void Damka_GameEnded(Player i_WinningPlayer)
        {
            // Game is done so we can remove all markers on the board.
            CheckedCheckerButton = null;

            StringBuilder messageBoxContent = new StringBuilder();
            messageBoxContent.AppendLine(GameManager.State == GameState.Tie ? "Tie!" : string.Format("{0} Won and received {1} points!", i_WinningPlayer.Name, GameManager.CalculatePlayerWinningPoints(i_WinningPlayer)));

            messageBoxContent.AppendLine("Another round?");

            DialogResult dialogResult = MessageBox.Show(messageBoxContent.ToString(), "Game Ended", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            if (dialogResult == DialogResult.Yes)
            {
                updatePlayerScoresLabels();
                GameManager.Reset();
            }
            else if (dialogResult == DialogResult.No)
            {
                Application.Exit();
            }
        }

        private void setupBoard()
        {
            byte boardSize = (byte)this.m_BoardSize;
            this.m_Board = new DamkaBoard(GameManager.Board, CELL_WIDTH_AND_HEIGHT, CheckerBtn_Click);
            this.m_Board.Location = new System.Drawing.Point(0, TOP_OFFSET);
            this.Controls.Add(this.m_Board);
        }

        private void CheckerBtn_Click(object sender, EventArgs e)
        {
            CheckerButton checkerBtn = sender as CheckerButton;
            bool clickedState = checkerBtn.Clicked,
                 playerIsClickingOnASquareThatHasHisSoldier = checkerBtn.Square.Color == GameManager.ActivePlayer.Color,
                 playerIsclickingAnAvailableMoveSquare = checkerBtn.IsAnAvailableMove;

            // First, make sure you are able to click that button
            if (!playerIsClickingOnASquareThatHasHisSoldier && !playerIsclickingAnAvailableMoveSquare)
            {
                return;
            }

            if (playerIsClickingOnASquareThatHasHisSoldier)
            {
                // Unclick all the other buttons.
                m_Board.UnclickallButtons();
                clickedState = !clickedState; // We have to toggle the state now.

                // This line also triggeres the Available Moves part. Please take a look at the CheckerCheckerButton Setter for more information
                CheckedCheckerButton = clickedState ? checkerBtn : null;
            }
            else
            {
                // Player has clicked on an available move square
                Move move = AvailableMoves.First(m => m.Destination == checkerBtn.LocationOnMatrix);
                CheckedCheckerButton = null;
                GameManager.HandleMove(move);
            }
        }

        private void setupFormSize()
        {
            byte multipier = (byte)this.m_BoardSize;
            this.ClientSize = new Size(CELL_WIDTH_AND_HEIGHT * multipier, (CELL_WIDTH_AND_HEIGHT * multipier) + TOP_OFFSET);
        }

        private void updatePlayerScoresLabels()
        {
            BlackPlayerScoreLabel.Text = GameManager.BlackPlayer.Points.ToString();
            RedPlayerScoreLabel.Text = GameManager.RedPlayer.Points.ToString();
        }

        private void setupLabels()
        {
            int sideOffset = 30,
                topOffset = 20;

            BlackPlayerNameLabel = new CheckerLabel(eCheckerColor.Black);
            BlackPlayerNameLabel.Text = GameManager.BlackPlayer.Name + ": ";
            BlackPlayerNameLabel.Location = new System.Drawing.Point(sideOffset, topOffset);

            RedPlayerNameLabel = new CheckerLabel(eCheckerColor.Red);
            RedPlayerNameLabel.Text = GameManager.RedPlayer.Name + ": ";
            RedPlayerNameLabel.Location = new System.Drawing.Point(sideOffset, topOffset + BlackPlayerNameLabel.Height);

            BlackPlayerScoreLabel = new CheckerLabel(eCheckerColor.Black);
            BlackPlayerScoreLabel.Width = 50;
            RedPlayerScoreLabel = new CheckerLabel(eCheckerColor.Red);
            RedPlayerScoreLabel.Width = 50;

            if (RedPlayerNameLabel.Width >= BlackPlayerNameLabel.Width)
            {
                BlackPlayerScoreLabel.Location = new System.Drawing.Point(sideOffset + RedPlayerNameLabel.Width, topOffset);
                RedPlayerScoreLabel.Location = new System.Drawing.Point(sideOffset + RedPlayerNameLabel.Width, topOffset + BlackPlayerNameLabel.Height);
            }
            else
            {
                BlackPlayerScoreLabel.Location = new System.Drawing.Point(sideOffset + BlackPlayerNameLabel.Width, topOffset);
                RedPlayerScoreLabel.Location = new System.Drawing.Point(sideOffset + BlackPlayerNameLabel.Width, topOffset + BlackPlayerNameLabel.Height);
            }

            updatePlayerScoresLabels();

            Controls.Add(BlackPlayerNameLabel);
            Controls.Add(BlackPlayerScoreLabel);
            Controls.Add(RedPlayerNameLabel);
            Controls.Add(RedPlayerScoreLabel);

            // No need to show who's turn it is if we're playing against a computer because every turn is our turn
            if (m_GameMode == GameMode.PVP)
            {
                ActivePlayerLabel = new CheckerLabel(eCheckerColor.Black);
                ActivePlayerLabel.Width = 400;
                ActivePlayerLabel.Location = new System.Drawing.Point(ClientSize.Width - 150, topOffset + 5);

                updateActivePlayerLabel();
                Controls.Add(ActivePlayerLabel);
            }
        }
    }
}
