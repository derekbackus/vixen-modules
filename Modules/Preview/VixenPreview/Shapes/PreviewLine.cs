﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace VixenModules.Preview.VixenPreview.Shapes
{
    [DataContract]
    public class PreviewLine: PreviewBaseShape
    {
        [DataMember]
        private List<PreviewPoint> _points = new List<PreviewPoint>();

        private PreviewPoint p1Start, p2Start;

        public PreviewLine(PreviewPoint point1, PreviewPoint point2, int lightCount)
        {
            AddPoint(point1);
            AddPoint(point2);

            // Just add the pixels, they will get layed out next
            for (int lightNum = 0; lightNum < lightCount; lightNum++)
            {
                //Console.WriteLine("Added: " + lightNum.ToString());
                PreviewPixel pixel = AddPixel(10, 10);
                pixel.PixelColor = Color.White;
            }
            // Lay out the pixels
            Layout();

            DoResize += new ResizeEvent(OnResize);
        }

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            Layout();
        }

        public void SetPoint0(int X, int Y)
        {
            _points[0].X = X;
            _points[0].Y = Y;
        }

        public void SetPoint1(int X, int Y)
        {
            _points[1].X = X;
            _points[1].Y = Y;
        }

        [CategoryAttribute("Position"),
        DisplayName("Point 1"),
        DescriptionAttribute("Lines are defined by 2 points. This is point 1.")]
        public Point Point1
        {
            get 
            {
                Point p = new Point(_points[0].X, _points[0].Y);
                return p; 
            }
            set 
            {
                _points[0].X = value.X;
                _points[0].Y = value.Y;
                Layout();
            }
        }

        [CategoryAttribute("Position"),
        DisplayName("Point 2"),
        DescriptionAttribute("Lines are defined by 2 points. This is point 2.")]
        public Point Point2
        {
            get
            {
                Point p = new Point(_points[1].X, _points[1].Y);
                return p;
            }
            set
            {
                _points[1].X = value.X;
                _points[1].Y = value.Y;
                Layout();
            }
        }

        public void AddPoint(PreviewPoint point)
        {
            _points.Add(point);
        }

        [CategoryAttribute("Settings"),
        DisplayName("Light Count"),
        DescriptionAttribute("Number of pixels or lights in the string.")]
        public int PixelCount
        {
            get { return Pixels.Count; }
            set {
                while (Pixels.Count > value)
                {
                    Pixels.RemoveAt(Pixels.Count - 1);
                }
                while (Pixels.Count < value)
                {
                    PreviewPixel pixel = new PreviewPixel(10, 10, PixelSize);
                    Pixels.Add(pixel);
                }
                Layout();
            }
        }

        public override void Layout()
        {
            double xSpacing = (double)(_points[0].X - _points[1].X) / (double)(PixelCount - 1);
            double ySpacing = (double)(_points[0].Y - _points[1].Y) / (double)(PixelCount - 1);
            double x = _points[0].X;
            double y = _points[0].Y;
            foreach (PreviewPixel pixel in Pixels)
            {
                pixel.X = (int)Math.Round(x);
                pixel.Y = (int)Math.Round(y);
                x -= xSpacing;
                y -= ySpacing;
                //Console.WriteLine(pixel.X.ToString());
            }
        }

        public override void MouseMove(int x, int y, int changeX, int changeY) 
        {
            // See if we're resizing
            if (_selectedPoint != null)
            {
                _selectedPoint.X = x;
                _selectedPoint.Y = y;
                Layout();
                SelectDragPoints();
            }
            // If we get here, we're moving
            else
            {
                _points[0].X = p1Start.X + changeX;
                _points[0].Y = p1Start.Y + changeY;
                _points[1].X = p2Start.X + changeX;
                _points[1].Y = p2Start.Y + changeY;
                Layout();
            }
        }

        private void OnResize(EventArgs e)
        {
            Layout();
        }

        public override void Select() 
        {
            base.Select();
            SelectDragPoints();
        }

        private void SelectDragPoints()
        {
            if (_points.Count >= 2)
            {
                List<PreviewPoint> selectPoints = new List<PreviewPoint>();
                selectPoints.Add(_points[0]);
                selectPoints.Add(_points[1]);
                SetSelectPoints(selectPoints, null);
            }
        }

        public override bool PointInShape(PreviewPoint point)
        {
            foreach (PreviewPixel pixel in Pixels)
            {
                Rectangle r = new Rectangle(pixel.X - (SelectPointSize / 2), pixel.Y - (SelectPointSize / 2), SelectPointSize + PixelSize, SelectPointSize + PixelSize);
                if (point.X >= r.X && point.X <= r.X + r.Width && point.Y >= r.Y && point.Y <= r.Y + r.Height)
                {
                    return true;
                }
            }
            return false;
        }

        public override void SetSelectPoint(PreviewPoint point)
        {
            if (point == null)
            {
                p1Start = new PreviewPoint(_points[0].X, _points[0].Y);
                p2Start = new PreviewPoint(_points[1].X, _points[1].Y);
            }
            _selectedPoint = point;
        }

        public override void SelectDefaultSelectPoint()
        {
            _selectedPoint = _points[1];
        }

        public override object Clone()
        {
            PreviewLine newLine = (PreviewLine)this.MemberwiseClone();

            newLine._pixels = new List<PreviewPixel>();

            foreach (PreviewPixel pixel in _pixels)
            {
                newLine.AddPixel(pixel.X, pixel.Y);
            }
            return newLine;
        }

    }
}