using System.Collections.Generic;

namespace Ex05.CheckersLogic
{
    /// <summary>
    /// A move holds the information we need for a specific turn. Besides holding a source and destination points,
    /// Move also holds a list of points called CheckersEaten. CheckersEaten are the checkers that were eaten during said move.
    /// </summary>
    public class Move
    {
        private Point m_source = new Point(-1, -1);
        private Point m_destination = new Point(-1, -1);
        private List<Point> m_CheckersEaten = new List<Point>();

        public Move(Point i_Source, Point i_Destination)
        {
            this.m_source = i_Source;
            this.m_destination = i_Destination;
        }

        // Convenience constructors
        public Move(Point i_Source, int i_DestinationX, int i_DestinationY)
            : this(i_Source, new Point(i_DestinationX, i_DestinationY))
        {
        }

        public Move(int i_SourceX, int i_SourceY, int i_DestinationX, int i_DestinationY)
                : this(new Point(i_SourceX, i_SourceY), new Point(i_DestinationX, i_DestinationY))
        {
        }

        public Point Source
        {
            get { return this.m_source; }
            set { this.m_source = value; }
        }

        public Point Destination
        {
            get { return this.m_destination; }
            set { this.m_destination = value; }
        }

        public List<Point> CheckersEaten
        {
            get { return m_CheckersEaten; }
        }
    }

    public struct Point
    {
        private int m_X;
        private int m_Y;

        public int X
        {
            get
            {
                return m_X;
            }
        }

        public int Y
        {
            get
            {
                return m_Y;
            }
        }

        public Point(int i_P1, int i_P2)
        {
            m_X = i_P1;
            m_Y = i_P2;
        }

        public static bool operator ==(Point i_P1, Point i_P2)
        {
            return i_P1.Equals(i_P2);
        }

        public static bool operator !=(Point i_X, Point i_Y)
        {
            return !(i_X == i_Y);
        }

        public override bool Equals(object i_Obj)
        {
            bool isEqual = false;

            if (!(i_Obj is Point))
            {
                return false;
            }

            Point other = (Point)i_Obj;

            if (this.X == other.X && this.Y == other.Y)
            {
                isEqual = true;
            }
            else
            {
                isEqual = false;
            }

            return isEqual;
        }

        public override int GetHashCode()
        {
            return X ^ Y;
        }
    }
}
