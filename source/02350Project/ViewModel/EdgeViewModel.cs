using _02350Project.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using _02350Project.Other;

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



        public PointCollection LinePoints {
            
            get {
                PointCollection linepoints;

                linepoints = CalcPoly();

                return linepoints;
            }
        
        }

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

            VMEndA.PropertyChanged += VMEndA_PropertyChanged;
            VMEndB.PropertyChanged += VMEndA_PropertyChanged;

            Type = typeConverter(type);
            initPointyArrowTemplate();
            initRhombusArrowTemplate();
            setFlags();
        }

        void VMEndA_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "X":
                case "Y":
                case "Width":
                case "Height":
                    RaisePropertyChanged("LinePoints");
                    break;
            }
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

            if (EdgeType.GEN == Type)
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
        public NodeViewModel VMEndA { get { return vMEndA; } set { vMEndA = value; } }
        public NodeViewModel VMEndB { get { return vMEndB; } set { vMEndB = value; } }
        public Node EndA { get { return edge.EndA; } set { edge.EndA = value; } }
        public Node EndB { get { return edge.EndB; } set { edge.EndB = value; } }

        public EdgeType Type { get { return edge.Type; } set { edge.Type = value; } }
        public Brush Color { get { return color; } set { color = value; RaisePropertyChanged("Color"); } }
        public bool Filled { get { return filled; } set { filled = value; RaisePropertyChanged("Filled"); } }
        public bool Dash { get { return dash; } set { dash = value; RaisePropertyChanged("Dash"); } }


        public ANCHOR AnchorA { get { return anchorA; } set { anchorA = value; } }
        public ANCHOR AnchorB { get { return anchorB; } set { anchorB = value; } }

        public double AX { get { return aX; } set { aX = value; RaisePropertyChanged("AX"); } }
        public double AY { get { return aY; } set { aY = value; RaisePropertyChanged("AY"); } }
        public double BX { get { return bX; } set { bX = value; RaisePropertyChanged("BX"); } }
        public double BY { get { return bY; } set { bY = value; RaisePropertyChanged("BY"); } }

        private bool isSelected;
        public bool IsSelected { get { return isSelected; } set { isSelected = value; RaisePropertyChanged("Opacity"); } }
        public double Opacity { get { return IsSelected ? 0.7 : 0.0; } }

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
        private EdgeType typeConverter(string type)
        {
            switch (type)
            {
                case "ASS":
                    return EdgeType.ASS;
                case "AGG":
                    return EdgeType.AGG;
                case "COM":
                    return EdgeType.COM;
                case "GEN":
                    return EdgeType.GEN;
                case "DEP":
                    return EdgeType.DEP;
                default:
                    return EdgeType.ASS;
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
                case EdgeType.GEN:
                    Filled = true;
                    Color = Brushes.White;
                    Dash = false;
                    break;
                case EdgeType.COM:
                    Filled = true;
                    Color = Brushes.Black;
                    Dash = false;
                    break;
                case EdgeType.AGG:
                    Filled = true;
                    Color = Brushes.White;
                    Dash = false;
                    break;
                case EdgeType.DEP:
                    Filled = false;
                    Color = Brushes.Transparent;
                    Dash = true;
                    break;
                case EdgeType.ASS:
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
        private double calculateAngle(double midX, double midY)
        {
            double deltaX = BX - midX;
            double deltaY = BY - midY;

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
            double cosTheta = Math.Cos(angle + Math.PI );
            double sinTheta = Math.Sin(angle + Math.PI );

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
        public void ArrowControl(double midX, double midY)
        {
            if (EdgeType.ASS != Type)
            {
                Point refPoint = new Point(BX, BY);
                PointCollection temp2 = new PointCollection();
                PointCollection temp3 = new PointCollection();
                switch (Type)
                {
                    case EdgeType.DEP:
                    case EdgeType.GEN:
                        temp3 = OffsetTemplate(refPoint, PointyArrow);
                        break;
                    case EdgeType.AGG:
                    case EdgeType.COM:
                        temp3 = OffsetTemplate(refPoint, RhombusArrow);
                        break;
                }
                PointCollection temp = rotatePoint(refPoint, temp3, calculateAngle(midX,midY));

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

        private PointCollection CalcPoly()
        {
            VMEndA.setEnds(this);
            Point pointEndA = new Point(AX, AY);
            Point pointEndB = new Point(BX, BY);
            Point mid1 = new Point();
            Point mid2 = new Point();
            PointCollection p = new PointCollection();
            if (VMEndA.Orientation == 0)
            {
                mid2 = new Point(BX, Math.Abs((AY + BY) / 2));
                mid1 = new Point(AX, Math.Abs((AY + BY) / 2));
            }
            else if (VMEndA.Orientation == 1) 
            {
                mid2 = new Point(Math.Abs((AX + BX)/2), BY);
                mid1 = new Point(Math.Abs((AX + BX)/2), AY);
            }
            p.Add(pointEndA);
            p.Add(mid1);
            p.Add(mid2);
            p.Add(pointEndB);
            ArrowControl(mid2.X, mid2.Y);
            return p;
        }

        /// <summary>
        /// Gets the Edge for the ViewModel. Should only be used for serializing.
        /// </summary>
        /// <returns></returns>
        public Edge getEdge() { return edge; }

    }
}
