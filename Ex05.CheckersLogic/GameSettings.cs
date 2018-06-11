namespace Ex05.CheckersLogic
{
    /// <summary>
    /// Game settings is the object that is passed to the Game Manager constructs and is a wrapper for the data we need 
    /// to establish a game of checkers. In our scenerio, the client fills this information through the console.
    /// </summary>
    public class GameSettings
    {
        private GameMode m_Mode;
        private BoardSize m_BoardSize;
        private string m_Player1Name;
        private string m_Player2Name;

        internal GameMode Mode
        {
            get
            {
                return m_Mode;
            }
        }

        internal BoardSize BoardSize
        {
            get
            {
                return m_BoardSize;
            }
        }

        internal string Player1Name
        {
            get
            {
                return m_Player1Name;
            }
        }

        internal string Player2Name
        {
            get
            {
                return m_Player2Name;
            }
        }

        public GameSettings(BoardSize i_BoardSize, GameMode i_Mode, string i_Player1Name, string i_Player2Name)
        {
            this.m_BoardSize = i_BoardSize;
            this.m_Mode = i_Mode;
            this.m_Player1Name = i_Player1Name;
            this.m_Player2Name = i_Player2Name;
        }
    }

    public enum GameMode
    {
        PVP = 1,
        PVC = 2
    }

    public enum BoardSize
    {
        Small = 6,
        Medium = 8,
        Large = 10
    }
}
