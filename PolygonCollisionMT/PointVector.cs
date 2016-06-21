using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PolygonCollisionMT
{
    public class PointVector
    {
        private List<MyVector> _points;

        public int Length
        {
            get
            {
                return _points.Count;
            }
        }

        public int PointDimension
        {
            get
            {
                return _points[0].Length;
            }
        }

        public MyVector GeometricCentre
        {
            get
            {
                MyVector tempMv = new MyVector();
                for (int i = 0; i < PointDimension; i++)
                {
                    tempMv.Add(0);
                }
                for (int i = 0; i < Length; i++)
                {
                    tempMv[0] += _points[i][0] / Length;
                    tempMv[1] += _points[i][1] / Length;
                }
                return tempMv;
            }
        }

        public PointVector()
        {
            _points = new List<MyVector>();
        }

        public PointVector(MyVector a, MyVector b, MyVector c)
        {
            _points = new List<MyVector>();
            _points.Add(a);
            _points.Add(b);
            _points.Add(c);
        }

        public PointVector(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            _points = new List<MyVector>();
            MyVector mv1 = new MyVector();

            mv1.Add(x1);
            mv1.Add(y1);

            MyVector mv2 = new MyVector();
            mv2.Add(x2);
            mv2.Add(y2);

            MyVector mv3 = new MyVector();
            mv3.Add(x3);
            mv3.Add(y3);

            _points.Add(mv1);
            _points.Add(mv2);
            _points.Add(mv3);
        }

        public MyVector this[int index]
        {
            get
            {
                return _points[index];
            }
        }

        public void Add(MyVector myVec)
        {
            _points.Add(myVec);
        }

        public Point[] getPointsTable()
        {
            Point[] pt = new Point[_points.Count];
            for (int i = 0; i < _points.Count; i++)
            {
                pt[i] = _points[i].getPoint();
            }
            return pt;
        }

        public MyVector maxCoordinatePoint(int dimensionIndex)
        {
            if (dimensionIndex >= PointDimension)
                throw new IndexOutOfRangeException();

            double max = _points[0][dimensionIndex];
            int index = 0;
            for (int i = 1; i < Length; i++)
            {
                if (max < _points[i][dimensionIndex])
                {
                    max = _points[i][dimensionIndex];
                    index = i;
                }
            }
            return _points[index];
        }

        public MyVector minCoordinatePoint(int dimensionIndex)
        {
            if (dimensionIndex >= PointDimension)
                throw new IndexOutOfRangeException();

            double min = _points[0][dimensionIndex];
            int index = 0;
            for (int i = 1; i < Length; i++)
            {
                if (min > _points[i][dimensionIndex])
                {
                    min = _points[i][dimensionIndex];
                    index = i;
                }
            }
            return _points[index];
        }

        public static PointVector operator +(PointVector pv, MyVector mv)
        {
            if (pv.PointDimension != mv.Length)
                throw new Exception();

            PointVector tempPv = new PointVector();

            for (int i = 0; i < pv.Length; i++)
            {
                MyVector tempMv = new MyVector();
                for (int j = 0; j < pv.PointDimension; j++)
                {
                    tempMv.Add(pv[i][j] + mv[j]);
                }
                tempPv.Add(tempMv);
            }

            return tempPv;

        }

        public static PointVector operator -(PointVector pv, MyVector mv)
        {
            if (pv.PointDimension != mv.Length)
                throw new Exception();

            PointVector tempPv = new PointVector();

            for (int i = 0; i < pv.Length; i++)
            {
                MyVector tempMv = new MyVector();
                for (int j = 0; j < pv.PointDimension; j++)
                {
                    tempMv.Add(pv[i][j] - mv[j]);
                }
                tempPv.Add(tempMv);
            }

            return tempPv;

        }

        private PointVector polarRepresentation(MyVector centre)
        {
            PointVector coordsRelativeToCenter = this - centre;
            PointVector polarCoords = new PointVector();
            for (int i = 0; i < this.Length; i++)
            {
                polarCoords.Add(MyMath.polarRepresentation(coordsRelativeToCenter[i]));
            }
            return polarCoords;
        }

        public PointVector rotate(double angle, MyVector rotationCentre)
        {
            PointVector polarRepresentation = this.polarRepresentation(rotationCentre);
            for (int i = 0; i < polarRepresentation.Length; i++)
            {
                polarRepresentation[i][1] += angle;
                polarRepresentation[i][1] %= 2 * Math.PI;
            }

            PointVector cartesianRepresentation = new PointVector();
            for (int i = 0; i < polarRepresentation.Length; i++)
            {
                MyVector cartesianPoint = MyMath.cartesianRepresentation(polarRepresentation[i], rotationCentre);
                cartesianRepresentation.Add(cartesianPoint);
            }

            return cartesianRepresentation;
        }

    }
}
