using System.Drawing;
using System.Windows.Forms;
using Ex05.CheckersLogic;

namespace Ex05.CheckersApp
{
    internal class CheckerLabel : Label
    {
        public CheckerLabel(eCheckerColor i_Color)
        {
            this.ForeColor = i_Color == eCheckerColor.Black ? Color.Black : Color.FromArgb(182, 0, 0);
            this.Font = new Font("Impact", 15);
        }
    }
}
