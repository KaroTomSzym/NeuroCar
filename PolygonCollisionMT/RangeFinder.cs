using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCollisionMT
{
    public class RangeFinder
    {
        private List<MyVector> _directionVectors;
        private double _radiusLength;
        private int _radiusDivisons;
        public double divisonLength
        {
            get
            {
                return _radiusLength / _radiusDivisons;
            }
        }

        public RangeFinder(int radiusNumber, double radiusLength, int radiusDivisons )
        {
            _radiusLength = radiusLength;
            _radiusDivisons = radiusDivisons;
            _directionVectors = new List<MyVector>();

            double angleIncrement = (2*Math.PI)/radiusNumber;
            for (int i = 0; i < radiusNumber; i++)
            {
                MyVector directionVec = new MyVector(Math.Sin(angleIncrement*i),Math.Cos(angleIncrement*i));
            }
        }

        public int rangeInDirection(MyVector directionVector, MyVector positionPoint, List<Polygon> polygons)
        {
            
            for (int i = 0; i < _radiusDivisons; i++)
            {
                MyVector pointToCheck = positionPoint + directionVector*divisonLength*i;
                foreach (Polygon p in polygons)
                {
                    if(p.isPointInside(pointToCheck))
                    {
                        return i;
                    }
                }
            }

            return _radiusDivisons;
        }

        public MyVector rangeInAllDirections(MyVector positionPoint, List<Polygon> polygons)
        {
            MyVector rangeVector = new MyVector();
            foreach (MyVector direction in _directionVectors)
            {
                rangeVector.Add(rangeInDirection(direction, positionPoint, polygons));
            }

            return rangeVector;
        }

    }
}
