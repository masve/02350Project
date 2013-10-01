using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _02350Project.Model
{
    public class Node : NotifyBase
    {
        private int x;
        private int y;
        private int height;
        private int width;

        private Point north;
        private int northX;
        private int northY;

        private Point south;
        private Point east;
        private Point west;

        private bool isSelected;

        public int X { get { return x; } set { x = value; NotifyPropertyChanged("X"); NotifyPropertyChanged("NorthX"); NotifyPropertyChanged("South"); NotifyPropertyChanged("East"); NotifyPropertyChanged("West"); } }
        public int Y { get { return y; } set { y = value; NotifyPropertyChanged("Y"); NotifyPropertyChanged("NorthY"); NotifyPropertyChanged("South"); NotifyPropertyChanged("East"); NotifyPropertyChanged("West"); } }
        public int Height { get { return height; } set { height = value; NotifyPropertyChanged("Height"); NotifyPropertyChanged("North"); NotifyPropertyChanged("South"); NotifyPropertyChanged("East"); NotifyPropertyChanged("West"); } }
        public int Width { get { return width; } set { width = value; NotifyPropertyChanged("Width"); NotifyPropertyChanged("North"); NotifyPropertyChanged("South"); NotifyPropertyChanged("East"); NotifyPropertyChanged("West"); } }

        public Point North { get { return north; } set { north.X = x + width / 2; north.Y = y; NotifyPropertyChanged("North"); } }

        public int NorthX { get { return northX; } set { northX = x + width / 2; NotifyPropertyChanged("NorthX"); } }
        public int NorthY { get { return northY; } set { northY = y; NotifyPropertyChanged("NorthY"); } }



        public Point South { get { return south; } set { south.X = x + width / 2; south.Y = y + height; NotifyPropertyChanged("South"); } }
        public Point East { get { return east; } set { east.X = x + width; east.Y = y + height / 2; NotifyPropertyChanged("East"); } }
        public Point West { get { return west; } set { west.X = x; west.Y = y + height / 2; NotifyPropertyChanged("West"); } }

        public bool IsSelected { get { return isSelected; } set { isSelected = value; NotifyPropertyChanged("IsSelected"); } }

        public Node()
        {
            isSelected = false;
            x = y = 50;
            width = height = 100;
        }
    }
}
