﻿using System;
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
        private Point south;
        private Point east;
        private Point west;

        //private bool isSelected;

        public int X { get { return x; } set { x = value; NotifyPropertyChanged("X"); NotifyPropertyChanged("CanvasCenterX"); NotifyPropertyChanged("North"); NotifyPropertyChanged("South"); NotifyPropertyChanged("East"); NotifyPropertyChanged("West"); } }
        public int Y { get { return y; } set { y = value; NotifyPropertyChanged("Y"); NotifyPropertyChanged("CanvasCenterY");  NotifyPropertyChanged("North"); NotifyPropertyChanged("South"); NotifyPropertyChanged("East"); NotifyPropertyChanged("West"); } }
        public int Height { get { return height; } set { height = value; NotifyPropertyChanged("Height");  NotifyPropertyChanged("North"); NotifyPropertyChanged("South"); NotifyPropertyChanged("East"); NotifyPropertyChanged("West");  } }
        public int Width { get { return width; } set { width = value; NotifyPropertyChanged("Width");  NotifyPropertyChanged("North"); NotifyPropertyChanged("South"); NotifyPropertyChanged("East"); NotifyPropertyChanged("West");  } }

        // Derived properties, returns the points for node's edge anchor points
        public Point North { get { north.X = X + Width / 2; north.Y = Y;            return north; } }
        public Point South { get { south.X = X + Width / 2; south.Y = Y + Height;   return south; } }
        public Point East { get { east.X = X + Width; east.Y = Y + Height / 2;      return east; } }
        public Point West { get { west.X = X; west.Y = Y + Height / 2;              return west; } }

        //public bool IsSelected { get { return isSelected; } set { isSelected = value; NotifyPropertyChanged("IsSelected"); } }

        public Node()
        {
            //isSelected = false;

            X = Y = 50;
            Width = Height = 100;
        }
    }
}
