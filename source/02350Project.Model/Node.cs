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

        //private bool isSelected;

        public int X { get { return x; } set { x = value; NotifyPropertyChanged("X"); NotifyPropertyChanged("North");  NotifyPropertyChanged("South"); NotifyPropertyChanged("East"); NotifyPropertyChanged("West");  } }
        public int Y { get { return y; } set { y = value; NotifyPropertyChanged("Y"); NotifyPropertyChanged("North");  NotifyPropertyChanged("South"); NotifyPropertyChanged("East"); NotifyPropertyChanged("West");  } }
        public int Height { get { return height; } set { height = value; NotifyPropertyChanged("Height");  NotifyPropertyChanged("North"); NotifyPropertyChanged("South"); NotifyPropertyChanged("East"); NotifyPropertyChanged("West");  } }
        public int Width { get { return width; } set { width = value; NotifyPropertyChanged("Width");  NotifyPropertyChanged("North"); NotifyPropertyChanged("South"); NotifyPropertyChanged("East"); NotifyPropertyChanged("West");  } }

        // Returns the points for node's edge anchor points
        public Point North { get { return new Point(X + Width / 2,Y); } }
        public Point South { get { return new Point(X + Width / 2, Y + Height); } }
        public Point East { get { return new Point(X + Width, Y + Height / 2); } }
        public Point West { get { return new Point(X, Y + Height / 2); } }

        //public bool IsSelected { get { return isSelected; } set { isSelected = value; NotifyPropertyChanged("IsSelected"); } }

        public Node()
        {
            //isSelected = false;
            X = Y = 50;
            Width = Height = 100;
        }
    }
}
