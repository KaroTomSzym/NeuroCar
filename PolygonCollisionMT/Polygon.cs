using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PolygonCollisionMT
{
    public abstract class Polygon
    {
        public PointVector _points { get; set; }
        public MyVector _velocity { get; set; }
        public double _angularVelocity { get; set; }

        public virtual double Mass
        {
            get
            {
                return Mass;
            }
        }
        public virtual MyVector MassCentre
        {
            get
            {
                return MassCentre;
            }
        }
        public virtual MyVector this[int index]
        {
            get
            {
                if(index > _points.Length)
                    throw new IndexOutOfRangeException();

                return _points[index];
            }
        }
        public virtual int VerticesNumber
        {
            get
            {
                return VerticesNumber;
            }
        }
        public virtual MyVector maxCoordinatePoint(int dimensionIndex)
        {
            return _points.maxCoordinatePoint(dimensionIndex);
        }
        public virtual MyVector minCoordinatePoint(int dimensionIndex)
        {
            return _points.minCoordinatePoint(dimensionIndex);
        }


        public abstract Point[] getPointsTable();
        public abstract void shift();
        public abstract void rotate();

        public abstract bool isPointInside(MyVector point);

        //Jeśli brak kolizji to zwróci (-1,?)
        public abstract MyVector boundaryCollison(double minX, double maxX, double minY, double maxY);
        public abstract MyVector polygonCollision(Polygon polygon);

        public abstract MyVector getForceVector(MyVector contactPoint);
        public abstract void actForce(MyVector contactPoint, MyVector forceVector);
        public abstract void forward(double speed);

        public abstract Polygon getClone();
    }
}
