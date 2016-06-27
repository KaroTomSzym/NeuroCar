using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PolygonCollisionMT
{
    public class Road
    {
        private List<Polygon> _polygons;
        public List<Polygon> VisiblePolygons { get; set; }
        public int Length { get; set; }
        public Car Car { get; set; }
        public Area VisibleArea { get; set; }
        private Random _random;

        public Road(int length)
        {
            _polygons = new List<Polygon>();
            VisiblePolygons = new List<Polygon>();
            VisibleArea = new Area(300, 400);
            Length = length;
            Car = new Car();
            _random = new Random();
        }

        public void addPolygon(Polygon p)
        {
            _polygons.Add(p);
        }

        public List<Polygon> getVisiblePolygons()
        {
            checkWhichPolygonsAreVisible();
            return VisiblePolygons;
        }

        private void checkWhichPolygonsAreVisible()
        {
            VisiblePolygons.Clear();
            foreach (Polygon polygon in _polygons)
            {
                if (polygonIsVisible(polygon))
                {
                    VisiblePolygons.Add(polygon);
                }
            }
        }

        private bool polygonIsVisible(Polygon polygon)
        {
            Point[] points = polygon.getPointsTable();
            for (int i = 0; i < points.Length; i++)
            {
                if (pointIsInsideVisibleArea(points[0]))
                    return true;
            }
            return false;
        }

        private bool pointIsInsideVisibleArea(Point point)
        {
            if (point.Y < VisibleArea.CurrentAreaPosition + VisibleArea.Size.Height && point.Y > VisibleArea.CurrentAreaPosition)
                return true;
            return false;
        }

        public void generatePolygons(int n)
        {
            for (int i = 0; i < n; i++)
            {
                addRandomPolygonToList(0,300,0,Length);
            }
        }

        public void addRandomPolygonToList(int minPositionX, int maxPositionX, int minPositionY, int maxPositionY)
        {
            PointVector pv = new PointVector();
            double r1, tempX, tempY;
            int avgSideLength = 50;
            int x1 = _random.Next(minPositionX, maxPositionX);
            int x2 = _random.Next(minPositionY, maxPositionY);

            pv.Add(new MyVector(x1, x2));
            r1 = _random.Next(0, 360);
            x1 = x1 + (int)(avgSideLength * Math.Cos(r1));
            x2 = x2 + (int)(avgSideLength * Math.Sin(r1));
            pv.Add(new MyVector(x1, x2));

            MyVector tempVector = new MyVector(x1, x2);
            double distance = 0;
            while (distance < avgSideLength * 0.8 || distance > avgSideLength * 1.2)
            {
                r1 = _random.Next(0, 360);
                tempX = x1 + (int)(avgSideLength * Math.Cos(r1));
                tempY = x2 + (int)(avgSideLength * Math.Sin(r1));
                tempVector = new MyVector(tempX, tempY);
                distance = MyMath.distanceBetweenPoints(pv[0], tempVector);
            }
            pv.Add(tempVector);

            this.addPolygon(new Triangle(pv, new MyVector(0, 0)));
        }
    }
}