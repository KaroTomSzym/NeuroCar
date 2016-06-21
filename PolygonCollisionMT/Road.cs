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

        public Road(int length)
        {
            VisibleArea = new Area(300, 400);
            Length = length;
            Car = new Car();
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
    }
}