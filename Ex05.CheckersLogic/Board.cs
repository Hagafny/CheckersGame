namespace Ex05.CheckersLogic
{
    /// <summary>
    /// This class is in charge of representing the Board which is the state of the game.
    /// </summary>
    public class Board
    {
        private Square[,] m_Matrix;
        private byte m_BoardSize;
    
        public Square[,] Matrix
        {
            get
            {
                return m_Matrix;
            }
        }

        public byte BoardSize
        {
            get
            {
                return m_BoardSize;
            }

            set
            {
                m_BoardSize = value;
            }
        }

        public Board(BoardSize i_BoardSize)
        {
            BoardSize = (byte)i_BoardSize;
            m_Matrix = new Square[BoardSize, BoardSize];
            InitializeBoard();
        }

        /// <summary>
        /// Set the pieces in their initial location.
        /// </summary>
        public void InitializeBoard()
        {
            byte numOfRowsToFill = (byte)((BoardSize - 2) / 2);
            bool isEvenSquare;
            eCheckerColor squareColor;

            for (byte i = 0; i < numOfRowsToFill; i++)
            {
                for (byte j = 0; j < BoardSize; j++)
                {
                    isEvenSquare = isEvenLocationSquare(i, j);
                    squareColor = isEvenSquare ? eCheckerColor.Empty : eCheckerColor.Red;
                    Matrix[i, j] = new Square(squareColor);
                }
            }

            for (int i = numOfRowsToFill; i < numOfRowsToFill + 2; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    Matrix[i, j] = new Square(eCheckerColor.Empty);
                }
            }

            numOfRowsToFill += 2;

            for (byte i = numOfRowsToFill; i < BoardSize; i++)
            {
                for (byte j = 0; j < BoardSize; j++)
                {
                    isEvenSquare = isEvenLocationSquare(i, j);
                    if (isEvenSquare)
                    {
                        squareColor = eCheckerColor.Empty;
                    }
                    else
                    {
                        squareColor = eCheckerColor.Black;
                    }

                    Matrix[i, j] = new Square(squareColor);
                }
            }
        }

        private bool isEvenLocationSquare(byte i_i, byte i_j)
        {
            return (i_i + i_j) % 2 == 0;
        }
    }
}
