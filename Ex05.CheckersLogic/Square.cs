namespace Ex05.CheckersLogic
{
    /// <summary>
    /// A square represents one square inside the Board Matrix. It holds the information of the current state of that specific square
    /// Notice we all use the enum 
    /// type to easily seperate each entity we might have here.
    /// </summary>
    public class Square
    {
        private eSquareType m_Type
        {
            get
            {
                eSquareType squareType = eSquareType.Empty;
                switch (this.Color)
                {
                    case eCheckerColor.Empty:
                        squareType = eSquareType.Empty;
                        break;
                    case eCheckerColor.Red:
                        if (King)
                        {
                            squareType = eSquareType.RedKing;
                        }
                        else
                        {
                            squareType = eSquareType.RedSoldier;
                        }

                        break;
                    case eCheckerColor.Black:
                        if (King)
                        {
                            squareType = eSquareType.BlackKing;
                        }
                        else
                        {
                            squareType = eSquareType.BlackSoldier;
                        }

                        break;
                }

                return squareType;
            }
        }

        private eCheckerColor m_Color;
        private bool m_King = false;

        public eSquareType Type
        {
            get
            {
                return m_Type;
            }
        }

        public eCheckerColor Color
        {
            get
            {
                return m_Color;
            }

            set
            {
                m_Color = value;
            }
        }

        public bool King
        {
            get
            {
                return m_King;
            }

            set
            {
                m_King = value;
            }
        }

        public Square(eCheckerColor i_Color)
        {
            this.m_Color = i_Color;
            this.m_King = false;
        }
    }

    public enum eSquareType
    {
        Empty,
        RedSoldier,
        RedKing,
        BlackSoldier,
        BlackKing
    }

    public enum eCheckerColor
    {
        Empty,
        Red,
        Black
    }
}
