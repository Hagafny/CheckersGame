using System.Drawing;
using System.Windows.Forms;
using Ex05.CheckersApp.Properties;
using Ex05.CheckersLogic;

namespace Ex05.CheckersApp
{
    /// <summary>
    /// In charge of presenting a CheckerButton. This is the UI version of the "Square" logic component.
    /// Our board will contain a matrix of CheckerButtons
    /// </summary>
    internal class CheckerButton : Button
    {
        private bool m_clicked;
        private bool m_IsAnAvailableMove;
        private Square m_Square;
        private CheckersLogic.Point m_LocationOnMatrix;

        internal bool Clicked
        {
            get
            {
                return m_clicked;
            }

            set
            {
                if (this.Enabled)
                {
                    m_clicked = value;
                    this.BackColor = m_clicked ? Color.CornflowerBlue : Color.Empty;
                }
            }
        }

        internal bool IsAnAvailableMove
        {
            get
            {
                return m_IsAnAvailableMove;
            }

            set
            {
                if (this.Enabled)
                {
                    m_IsAnAvailableMove = value;
                    if (!this.Clicked)
                    {
                        this.BackColor = m_IsAnAvailableMove ? Color.Orange : Color.Empty;
                    }
                }
            }
        }

        internal Square Square
        {
            get
            {
                return m_Square;
            }

            set
            {
                m_Square = value;
                Image = getCheckerImage();
            }
        }

        internal CheckersLogic.Point LocationOnMatrix
        {
            get
            {
                return m_LocationOnMatrix;
            }

            set
            {
                m_LocationOnMatrix = value;
            }
        }

        public CheckerButton(int i_size, bool i_ButtonIsEnabled)
        {
            this.Enabled = i_ButtonIsEnabled;

            if (this.Enabled)
            {
                this.BackColor = Color.Empty;
            }
            else
            {
                this.BackColor = Color.DarkGray;
            }

            this.Size = new System.Drawing.Size(i_size, i_size);
            this.Font = new Font(new FontFamily(System.Drawing.Text.GenericFontFamilies.Monospace), 20);
        }

        private Image getCheckerImage()
        {
            Image checkerImage;
            switch (this.Square.Type)
            {
                default:
                case eSquareType.Empty:
                    checkerImage = null;
                    break;
                case eSquareType.RedSoldier:
                    checkerImage = Resources.checkerred;
                    break;
                case eSquareType.RedKing:
                    checkerImage = Resources.checkerredking;
                    break;
                case eSquareType.BlackSoldier:
                    checkerImage = Resources.checkerblack;
                    break;
                case eSquareType.BlackKing:
                    checkerImage = Resources.checkerblackking;
                    break;
            }

            return checkerImage;
        }
    }
}
