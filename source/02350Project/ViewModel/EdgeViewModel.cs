using _02350Project.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace _02350Project.ViewModel
{
    public class EdgeViewModel : ViewModelBase
    {
        private Edge edge;

        private NodeViewModel vMEndA;
        private NodeViewModel vMEndB;

        private bool dash;

        public enum ANCHOR { NORTH, SOUTH, WEST, EAST };

        private ANCHOR anchorA;
        private ANCHOR anchorB;

        private double aX, aY, bX, bY;

        private PointCollection pointyArrow = new PointCollection();

        public PointCollection actualArrow = new PointCollection();

        public EdgeViewModel(Node fromNode, Node toNode, NodeViewModel fromVM, NodeViewModel toVM)
        {
            edge = new Edge();
            EndA = fromNode;
            EndB = toNode;
            VMEndA = fromVM;
            VMEndB = toVM;
            Other.ConsolePrinter.Write("Edge VM");
            AX = AY = 200;
            BX = BY = 300;
            Other.ConsolePrinter.Write(fromNode.Name + " + " + toNode.Name);
            PointyArrowTemplate();
        }

        private void PointyArrowTemplate()
        {
            Point p1 = new Point(10.0, 5.0);
            Point p2 = new Point(10.0, -5.0);
            pointyArrow.Add(p1);
            pointyArrow.Add(p2);
        }

        public NodeViewModel VMEndA { get { return vMEndA; } set { vMEndA = value; RaisePropertyChanged("VMEndA"); RaisePropertyChanged("AnchorA"); } }
        public NodeViewModel VMEndB { get { return vMEndB; } set { vMEndB = value; RaisePropertyChanged("VMEndB"); RaisePropertyChanged("AnchorB"); } }
        public Node EndA { get { return edge.EndA; } set { edge.EndA = value; RaisePropertyChanged("EndA"); RaisePropertyChanged("AnchorA"); } }
        public Node EndB { get { return edge.EndB; } set { edge.EndB = value; RaisePropertyChanged("EndB"); RaisePropertyChanged("AnchorB"); } }

        public Edge.typeEnum Type { get { return edge.Type; } set { edge.Type = value; RaisePropertyChanged("Type"); } }

        public bool Dash { get { return dash; } set { dash = value; RaisePropertyChanged("Dash"); } }

        public ANCHOR AnchorA { get { return anchorA; } set { anchorA = value; RaisePropertyChanged("AnchorA"); RaisePropertyChanged("AX"); RaisePropertyChanged("AY"); } }
        public ANCHOR AnchorB { get { return anchorB; } set { anchorB = value; RaisePropertyChanged("AnchorB"); RaisePropertyChanged("BX"); RaisePropertyChanged("BY"); } }

        public double AX { get { return aX; } set { aX = value; RaisePropertyChanged("AX"); } }
        public double AY { get { return aY; } set { aY = value; RaisePropertyChanged("AY"); } }
        public double BX { get { return bX; } set { bX = value; RaisePropertyChanged("BX"); } }
        public double BY { get { return bY; } set { bY = value; RaisePropertyChanged("BY"); } }

        public PointCollection ActualArrow { get { return actualArrow; } set { actualArrow = value; RaisePropertyChanged("ActualArrow"); } }

        private PointCollection PointyArrow { get { return pointyArrow; } }

        private double calculateAngle()
        {
            double deltaX = BX - AX;
            double deltaY = BY - AY;

            return Math.Atan2(deltaY, deltaX);
        }

        public PointCollection rotatePoint(Point centerPoint, PointCollection points, double angle)
        {
            double cosTheta = Math.Cos(angle + (180 * (Math.PI / 180)));
            double sinTheta = Math.Sin(angle + (180 * (Math.PI / 180)));

            PointCollection newPoints = new PointCollection();

            foreach (Point p in points)
            {
                double x = (int)(cosTheta * (p.X - centerPoint.X) - sinTheta * (p.Y - centerPoint.Y) + centerPoint.X);
                double y = (int)(sinTheta * (p.X - centerPoint.X) + cosTheta * (p.Y - centerPoint.Y) + centerPoint.Y);

                newPoints.Add(new Point(x, y));
            }

            return newPoints;
        }

        public PointCollection OffsetTemplate(Point centerPoint, PointCollection pc)
        {
            PointCollection temp = new PointCollection();
            foreach (Point p in pc)
            {
                temp.Add(new Point(p.X + centerPoint.X, p.Y + centerPoint.Y));
            }
            return temp;
        }

        public void ArrowControl()
        {
            PointCollection temp = rotatePoint((new Point(BX, BY)), OffsetTemplate(new Point(BX, BY), PointyArrow), calculateAngle());
            for (int i = 0; i < temp.Count(); i++)
            {
                if (i == 1)
                {
                    ActualArrow.Add(new Point(BX, BY));
                }
                ActualArrow.Add(temp[i]);
            }
            // Other.ConsolePrinter.Write(ActualArrow.ToString());
        }

    }
}
