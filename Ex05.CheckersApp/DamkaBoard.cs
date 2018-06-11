using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Ex05.CheckersLogic;

namespace Ex05.CheckersApp
{
    /// <summary>
    /// This is the Control that is in charge of the actual checker board. All interaction between the board and 
    /// the checker buttons are done here
    /// </summary>
    internal class DamkaBoard : Control
    {
        private Board m_GameBoard;
        private CheckerButton[,] m_CheckerButtons;

        public DamkaBoard(Board i_Board, int i_CellWidthAndHeight, EventHandler i_CheckerBtn_Click)
        {
            this.m_GameBoard = i_Board;
            int boardDimenstions = m_GameBoard.BoardSize * i_CellWidthAndHeight;
            this.Size = new Size(boardDimenstions, boardDimenstions);

            setupButtonBoard(i_CellWidthAndHeight, i_CheckerBtn_Click);
        }

        internal void UnclickallButtons()
        {
            foreach (CheckerButton currentCheckerButton in this.Controls)
            {
                currentCheckerButton.Clicked = false;
            }
        }

        internal void ResetAvailableMoves()
        {
            foreach (CheckerButton currentCheckerButton in this.Controls)
            {
                currentCheckerButton.IsAnAvailableMove = false;
            }
        }

        internal void ShowAvailableMoves(Move[] i_AvailableMovs)
        {
            CheckersLogic.Point[] availableMovesDestinationPoints = i_AvailableMovs.Select(move => move.Destination).ToArray();
            CheckerButton[] availableMovesBtns = getCheckerButtonsByCoordinates(availableMovesDestinationPoints);

            foreach (CheckerButton availableMoveCheckerBtn in availableMovesBtns)
            {
                availableMoveCheckerBtn.IsAnAvailableMove = true;
            }
        }

        /// <summary>
        /// Every time the GameManager notifies us on a change on the board, we update this board to show case it
        /// to the user
        /// </summary>
        internal void DamkaBoard_BoardChanged()
        {
            byte boardSize = m_GameBoard.BoardSize;

            for (byte i = 0; i < boardSize; i++)
            {
                for (byte j = 0; j < boardSize; j++)
                {
                    m_CheckerButtons[i, j].Square = m_GameBoard.Matrix[j, i];
                }
            }
        }

        /// <summary>
        /// Initialize all the checker buttons. This is done only once when we start playing
        /// </summary>
        /// <param name="i_CellWidthAndHeight">The Height and Width of every cell</param>
        /// <param name="i_CheckerBtn_Click">A pointer to a method that will be called once a checker button is clicked</param>
        private void setupButtonBoard(int i_CellWidthAndHeight, EventHandler i_CheckerBtn_Click)
        {
            byte boardSize = m_GameBoard.BoardSize;
            m_CheckerButtons = new CheckerButton[boardSize, boardSize];

            for (byte i = 0; i < boardSize; i++)
            {
                for (byte j = 0; j < boardSize; j++)
                {
                    bool buttonIsEnabled = (i + j) % 2 != 0;
                    CheckerButton checkerBtn = new CheckerButton(i_CellWidthAndHeight, buttonIsEnabled);
                    checkerBtn.Location = new System.Drawing.Point(i_CellWidthAndHeight * i, i_CellWidthAndHeight * j);
                    checkerBtn.Square = m_GameBoard.Matrix[j, i];
                    checkerBtn.LocationOnMatrix = new Ex05.CheckersLogic.Point(i, j);

                    checkerBtn.Click += i_CheckerBtn_Click;

                    m_CheckerButtons[i, j] = checkerBtn;
                    this.Controls.Add(checkerBtn);
                }
            }
        }

        private CheckerButton[] getCheckerButtonsByCoordinates(CheckersLogic.Point[] availableMovesDestinationPoints)
        {
            CheckerButton[] destinationButtons = new CheckerButton[availableMovesDestinationPoints.Length];
            byte counter = 0;

            foreach (CheckerButton currentCheckerButton in this.Controls)
            {
                foreach (CheckersLogic.Point destinationPoint in availableMovesDestinationPoints)
                {
                    if (currentCheckerButton.LocationOnMatrix == destinationPoint)
                    {
                        destinationButtons[counter] = currentCheckerButton;
                        counter++;
                    }
                }
            }

            return destinationButtons;
        }
    }
}
