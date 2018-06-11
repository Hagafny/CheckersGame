using System;
using System.Linq;
using System.Windows.Forms;
using Ex05.CheckersLogic;

namespace Ex05.CheckersApp
{
    public partial class GameSettingsForm : Form
    {
        public GameSettingsForm()
        {
            InitializeComponent();
        }

        private void player2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool PlayerVSCompMode = (sender as CheckBox).Checked;
            if (PlayerVSCompMode)
            {
                textBoxPlayer2.Enabled = true;
                textBoxPlayer2.Text = string.Empty;
            }
            else
            {
                textBoxPlayer2.Enabled = false;
                textBoxPlayer2.Text = "Computer";
            }
        }

        private void settingsDoneButton_Click(object sender, EventArgs e)
        {
            BoardSize boardSize = (BoardSize)getSelectedBoardSizeRadioButton().Tag;
            GameMode gameMode = checkBoxPlayer2.Checked ? GameMode.PVP : GameMode.PVC;
            string player1Name = textBoxPlayer1.Text,
                   player2Name = textBoxPlayer2.Text;

            // Validation
            if (!isValidPlayerName(player1Name) || !isValidPlayerName(player2Name))
            {
                MessageBox.Show("Invalid Name! Name should be 20 characters or shorter and should not contain blank spaces", "Wrong Input!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GameSettings gameSettings = new GameSettings(boardSize, gameMode, player1Name, player2Name);

            this.Hide();
            Damka damka = new Damka(gameSettings);
            damka.Closed += (s, args) => this.Close();
            damka.Show();
        }

        private RadioButton getSelectedBoardSizeRadioButton()
        {
            RadioButton selectedRadioButton;
            if (radioBoardSize6.Checked)
            {
                selectedRadioButton = radioBoardSize6;
            }
            else if (radioBoardSize8.Checked)
            {
                selectedRadioButton = radioBoardSize8;
            }
            else
            {
                selectedRadioButton = radioBoardSize10;
            }

            return selectedRadioButton;
        }

        /// <summary>
        /// These validators are passed as delegates to our input functions.
        /// </summary>
        /// <param name="i_NameInput"></param>
        /// <returns></returns>
        private bool isValidPlayerName(string i_NameInput)
        {
            bool stringIsLessThan21Chars = i_NameInput.Length <= 20;
            bool stringDoesNotContainSpaces = !i_NameInput.Any(char.IsWhiteSpace);
            return stringIsLessThan21Chars && stringDoesNotContainSpaces;
        }
    }
}
