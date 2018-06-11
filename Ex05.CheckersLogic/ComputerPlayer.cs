using System;

namespace Ex05.CheckersLogic
{
    /// <summary>
    /// Computer PLayer inherits from the abstract Player Class and only implements the PlayTurn method
    /// In our case, his PlayTurn method is passed down to him by the GameManager that handles the AI.
    /// </summary>
    internal class ComputerPlayer : Player
    {
        private Action m_ComputerTurn;

        public ComputerPlayer(Action i_ComputerTurn) : base("Computer", eCheckerColor.Red)
        {
            this.m_ComputerTurn = i_ComputerTurn;
        }

        public override void PlayTurn()
        {
            m_ComputerTurn();
        }
    }
}
