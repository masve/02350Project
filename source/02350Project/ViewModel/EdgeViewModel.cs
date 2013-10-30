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
        private Brush color;
        private bool filled;

        public enum ANCHOR { NORTH, SOUTH, WEST, EAST };

        private ANCHOR anchorA;
        private ANCHOR anchorB;

        private double aX, aY, bX, bY;

        private PointCollection pointyArrow = new PointCollection();
        private PointCollection rhombusArrow = new PointCollection();

        public PointCollection actualArrow = new PointCollection();




        public EdgeViewModel(Node fromNode, Node toNode, NodeViewModel fromVM, NodeViewModel toVM, string type)
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

            Type = typeConverter(type);

            initPointyArrowTemplate();
            initRhombusArrowTemplate();

            setFlags();
        }

        private Edge.typeEnum typeConverter(string type)
        {
            switch (type)
            {
                case "ASS":
                    return Edge.typeEnum.ASS;
                case "AGG":
                    return Edge.typeEnum.AGG;
                case "COM":
                    return Edge.typeEnum.COM;
                case "GEN":
                    return Edge.typeEnum.GEN;
                case "DEP":
                    return Edge.typeEnum.DEP;
                default:
                    return Edge.typeEnum.ASS;
            }
        }

        private void setFlags()
        {
            switch (Type)
            {
                case Edge.typeEnum.GEN:
                    Filled = true;
                    Color = Brushes.White;
                    Dash = false;
                    break;
                case Edge.typeEnum.COM:
                    Filled = true;
                    Color = Brushes.Black;
                    Dash = false;
                    break;
                case Edge.typeEnum.AGG:
                    Filled = true;
                    Color = Brushes.White;
                    Dash = false;
                    break;
                case Edge.typeEnum.DEP:
                    Filled = false;
                    Dash = true;
                    Color = Brushes.Transparent;
                    break;
                case Edge.typeEnum.ASS:
                    Filled = false;
                    Dash = false;
                    Color = Brushes.Transparent;
                    break;
            }
        }

        private void initPointyArrowTemplate()
        {
            Point p1 = new Point(10.0, 5.0);
            Point p2 = new Point(10.0, -5.0);
            pointyArrow.Add(p1);
            pointyArrow.Add(p2);

            if (Edge.typeEnum.GEN == Type)
                pointyArrow.Add(p1);
        }

        private void initRhombusArrowTemplate()
        {
            Point p1 = new Point(10.0, 5.0);
            Point p2 = new Point(10.0, -5.0);
            Point p3 = new Point(20.0, 0.0);
            Point p4 = new Point(10.0, 5.0);
            rhombusArrow.Add(p1);
            rhombusArrow.Add(p2);
            rhombusArrow.Add(p3);
            rhombusArrow.Add(p4);
        }

        public NodeViewModel VMEndA { get { return vMEndA; } set { vMEndA = value; RaisePropertyChanged("VMEndA"); RaisePropertyChanged("AnchorA"); } }
        public NodeViewModel VMEndB { get { return vMEndB; } set { vMEndB = value; RaisePropertyChanged("VMEndB"); RaisePropertyChanged("AnchorB"); } }
        public Node EndA { get { return edge.EndA; } set { edge.EndA = value; RaisePropertyChanged("EndA"); RaisePropertyChanged("AnchorA"); } }
        public Node EndB { get { return edge.EndB; } set { edge.EndB = value; RaisePropertyChanged("EndB"); RaisePropertyChanged("AnchorB"); } }

        public Edge.typeEnum Type { get { return edge.Type; } set { edge.Type = value; RaisePropertyChanged("Type"); } }
        public Brush Color { get { return color; } set { color = value; RaisePropertyChanged("Color"); } }
        public bool Filled { get { return filled; } set { filled = value; RaisePropertyChanged("Filled"); } }

        public bool Dash { get { return dash; } set { dash = value; RaisePropertyChanged("Dash"); } }

        public ANCHOR AnchorA { get { return anchorA; } set { anchorA = value; RaisePropertyChanged("AnchorA"); RaisePropertyChanged("AX"); RaisePropertyChanged("AY"); } }
        public ANCHOR AnchorB { get { return anchorB; } set { anchorB = value; RaisePropertyChanged("AnchorB"); RaisePropertyChanged("BX"); RaisePropertyChanged("BY"); } }

        public double AX { get { return aX; } set { aX = value; RaisePropertyChanged("AX"); } }
        public double AY { get { return aY; } set { aY = value; RaisePropertyChanged("AY"); } }
        public double BX { get { return bX; } set { bX = value; RaisePropertyChanged("BX"); } }
        public double BY { get { return bY; } set { bY = value; RaisePropertyChanged("BY"); } }

        public PointCollection ActualArrow { get { return actualArrow; } set { actualArrow = value; RaisePropertyChanged("ActualArrow"); } }


        private PointCollection PointyArrow { get { return pointyArrow; } }
        private PointCollection RhombusArrow { get { return rhombusArrow; } }

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
            if (Edge.typeEnum.ASS != Type)
            {
                Point refPoint = new Point(BX, BY);
                PointCollection temp2 = new PointCollection();
                PointCollection temp3 = new PointCollection();
                switch (Type)
                {
                    case Edge.typeEnum.DEP:
                        temp3 = OffsetTemplate(refPoint, PointyArrow);
                        break;
                    case Edge.typeEnum.GEN:
                        temp3 = OffsetTemplate(refPoint, PointyArrow);
                        break;
                    case Edge.typeEnum.AGG:
                        temp3 = OffsetTemplate(refPoint, RhombusArrow);
                        break;
                    case Edge.typeEnum.COM:
                        temp3 = OffsetTemplate(refPoint, RhombusArrow);
                        break;
                }
                PointCollection temp = rotatePoint(refPoint, temp3, calculateAngle());
                for (int i = 0; i < temp.Count(); i++)
                {
                    if (i == 1)
                        temp2.Add(new Point(BX, BY));

                    temp2.Add(temp[i]);
                }
                ActualArrow = temp2;
            }






        }

    }
}
