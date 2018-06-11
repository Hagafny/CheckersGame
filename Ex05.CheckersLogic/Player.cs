namespace Ex05.CheckersLogic
{
    /// <summary>
    /// Game is an abstract class that is not to be instantiated. It holds the base logic of both human and AI.
    /// We decided to use this sort of architecture so we can have complete seperation between both entities.
    /// </summary>
    public abstract class Player
    {
        private string m_Name;
        private eCheckerColor m_Color;
        private int m_Points;

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public eCheckerColor Color
        {
            get
            {
                return m_Color;
            }
        }

        public int Points
        {
            get
            {
                return m_Points;
            }

            set
            {
                m_Points = value;
            }
        }

        public Player(string i_Name, eCheckerColor i_Color)
        {
            this.m_Name = i_Name;
            this.m_Color = i_Color;
        }

        public abstract void PlayTurn();
    }
}
