namespace Ex05.CheckersLogic
{
    /// <summary>
    /// HumanPlayer inherits from the abstract Player class. The PlayTurn method is being passed down from the UserInterface class
    /// When we call PlayTurn we essently call the OnUserInput method
    /// </summary>
    public class HumanPlayer : Player
    {
        public HumanPlayer(string i_Name, eCheckerColor i_Color) : base(i_Name, i_Color)
        {
        }

        public override void PlayTurn()
        {
            // Do nothing. Since we are a human player, a turn will be decided by the player clicking on the form
        }
    }
}
