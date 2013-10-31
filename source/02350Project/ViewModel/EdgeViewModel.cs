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
        #region Private Fields
        private Edge edge;

        private NodeViewModel vMEndA;
        private NodeViewModel vMEndB;

        private bool dash;
        private Brush color;
        private bool filled;

        private ANCHOR anchorA;
        private ANCHOR anchorB;

        private double aX, aY, bX, bY;

        private PointCollection pointyArrow = new PointCollection();
        private PointCollection rhombusArrow = new PointCollection();
        private PointCollection actualArrow = new PointCollection();
        #endregion

        #region Public Fields
        public enum ANCHOR { NORTH, SOUTH, WEST, EAST };
        #endregion

        #region Constructor
        public EdgeViewModel(Node fromNode, Node toNode, NodeViewModel fromVM, NodeViewModel toVM, string type)
        {
            /*
             * The EdgeViewModel Constructor takes both Node and NodeViewModel.
             * Because as it is not the Edge Models need to know directly which
             * Nodes it is connecting. As we cannot give the Edge Model the 
             * ViewModels to not break the MVVM pattern.
             */

            edge = new Edge();
            EndA = fromNode;
            EndB = toNode;
            VMEndA = fromVM;
            VMEndB = toVM;
            AX = AY = 200;
            BX = BY = 300;

            Type = typeConverter(type);
            initPointyArrowTemplate();
            initRhombusArrowTemplate();

            setFlags();
        }
        #endregion

        #region Arrow Templates
        /// <summary>
        /// Initializes the PointCollection template used for Association 
        /// and Dependency type edges.
        /// </summary>
        private void initPointyArrowTemplate()
        {
            Point p1 = new Point(10.0, 5.0);
            Point p2 = new Point(10.0, -5.0);
            pointyArrow.Add(p1);
            pointyArrow.Add(p2);

            if (Edge.typeEnum.GEN == Type)
                pointyArrow.Add(p1);
        }

        /// <summary>
        /// Initializes the PointCollection template used for Aggregation 
        /// and Composition type edges.
        /// </summary>
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
        #endregion

        #region Properties
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
        #endregion

        #region Private Helpers
        private PointCollection PointyArrow { get { return pointyArrow; } }
        private PointCollection RhombusArrow { get { return rhombusArrow; } }

        /// <summary>
        /// Converts the type given from the constructor
        /// to the enum that is used to represent the edge type in 
        /// the EdgeViewModel and model.
        /// </summary>
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

        /// <summary>
        /// Sets the flags that define the properties of an edge and its head.
        /// The visual representation; fill, color, dash.
        /// </summary>
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
                    Color = Brushes.Transparent;
                    Dash = true;
                    break;
                case Edge.typeEnum.ASS:
                    Filled = false;
                    Color = Brushes.Transparent;
                    Dash = false;
                    break;
            }
        }

        /// <summary>
        /// Calculates the angle, in radians, between an the edge's start and end nodes.
        /// </summary>
        /// <returns></returns>
        private double calculateAngle()
        {
            double deltaX = BX - AX;
            double deltaY = BY - AY;

            return Math.Atan2(deltaY, deltaX);
        }

        /// <summary>
        /// Rotates a PointCollection from a reference point.
        /// </summary>
        /// <param name="refPoint"></param>
        /// <param name="points"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public PointCollection rotatePoint(Point refPoint, PointCollection points, double angle)
        {
            double cosTheta = Math.Cos(angle + (180 * (Math.PI / 180)));
            double sinTheta = Math.Sin(angle + (180 * (Math.PI / 180)));

            PointCollection newPoints = new PointCollection();

            foreach (Point p in points)
            {
                double x = (int)(cosTheta * (p.X - refPoint.X) - sinTheta * (p.Y - refPoint.Y) + refPoint.X);
                double y = (int)(sinTheta * (p.X - refPoint.X) + cosTheta * (p.Y - refPoint.Y) + refPoint.Y);

                newPoints.Add(new Point(x, y));
            }

            return newPoints;
        }

        /// <summary>
        /// Calculates the positional offset given a reference point and an arrow template.
        /// </summary>
        /// <param name="centerPoint"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public PointCollection OffsetTemplate(Point refPoint, PointCollection arrowTemplate)
        {
            PointCollection offsetCollection = new PointCollection();
            foreach (Point p in arrowTemplate)
            {
                offsetCollection.Add(new Point(p.X + refPoint.X, p.Y + refPoint.Y));
            }
            return offsetCollection;
        }

        /// <summary>
        /// Determines the type of arrow head should be set for the edge.
        /// </summary>
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

                /*
                 * Adds the reference point to as index[1] in the temp2 PointCollection
                 */
                for (int i = 0; i < temp.Count(); i++)
                {
                    if (i == 1)
                        temp2.Add(refPoint);

                    temp2.Add(temp[i]);
                }
                ActualArrow = temp2;
            }
        #endregion
        }

    }
}
