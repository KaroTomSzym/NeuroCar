using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PolygonCollisionMT
{
    public class Triangle : Polygon
    {

        public override MyVector MassCentre
        {
            get
            {
                return _points.GeometricCentre;
            }
        }

        public override double Mass
        {
            get
            {
                return MyMath.triangleSurface(_points)*0.01;
            }
        }

        public override int VerticesNumber
        {
            get
            {
                return _points.Length;
            }
        }

        public Triangle(Polygon previousTriangle)
        {
            _points = new PointVector(previousTriangle._points);
            _velocity = new MyVector(previousTriangle._velocity);
            _angularVelocity = previousTriangle._angularVelocity;
        }

        public Triangle(PointVector pv, MyVector velocity)
        {
            this._points = pv;
            this._velocity = velocity;
            this._angularVelocity = 0.0;
        }

        public Triangle(MyVector vertexA, MyVector vertexB, MyVector vertexC)
        {
            PointVector pv = new PointVector();
            pv.Add(vertexA);
            pv.Add(vertexB);
            pv.Add(vertexC);
            MyVector velocity = new MyVector(0, 0);

            _velocity = velocity;
            _points = pv;
            _angularVelocity = 0;

        }

        public override Point[] getPointsTable()
        {
            Point[] pt = new Point[_points.Length + 1];
            Point[] contour = _points.getPointsTable();
            for (int i = 0; i < _points.Length; i++)
            {
                pt[i] = contour[i];
            }
            pt[_points.Length] = _points[0].getPoint();

            return pt;
        }

        public override void shift()
        {
            _points = _points + _velocity;
            _velocity *= 0.97;
        }

        public override void rotate()
        {
            _points = _points.rotate(_angularVelocity, MassCentre);
            _angularVelocity *= 0.9; //friction
        }

        public override MyVector boundaryCollison(double minX, double maxX, double minY, double maxY)
        {

            MyVector minXPoint = _points.minCoordinatePoint(0) + _velocity;
            MyVector maxXPoint = _points.maxCoordinatePoint(0) + _velocity;
            MyVector minYPoint = _points.minCoordinatePoint(1) + _velocity;
            MyVector maxYPoint = _points.maxCoordinatePoint(1) + _velocity;

            if (minX > minXPoint[0])
            {
                return minXPoint;
            }
            if (maxX < maxXPoint[0])
            {
                return maxXPoint;
            }
            if (minY > minYPoint[1])
            {
                return minYPoint;
            }
            if (maxY < maxYPoint[1])
            {
                return maxYPoint;
            }

            MyVector noCollison = new MyVector(-1, 0);
            return noCollison;
        }

        public override bool isPointInside(MyVector point)
        {
            double controlSurface = MyMath.triangleSurface(_points);

            MyVector nextPoint = point + _velocity;
            double splitedSurface = MyMath.splitedTriangleSurface(_points, nextPoint);

            if (controlSurface == splitedSurface)
                return true;
            else
                return false;

        }

        public override MyVector polygonCollision(Polygon polygon)
        {

            bool maxXBoundaryInside = polygon.maxCoordinatePoint(0)[0] > this.minCoordinatePoint(0)[0]
                && polygon.maxCoordinatePoint(0)[0] < this.maxCoordinatePoint(0)[0];
            bool maxYBoundaryInside = polygon.maxCoordinatePoint(1)[1] > this.minCoordinatePoint(1)[1]
                && polygon.maxCoordinatePoint(1)[1] < this.maxCoordinatePoint(1)[1];
            bool minXBoundaryInside = polygon.minCoordinatePoint(0)[0] > this.minCoordinatePoint(0)[0]
                && polygon.minCoordinatePoint(0)[0] < this.maxCoordinatePoint(0)[0];
            bool minYBoundaryInside = polygon.minCoordinatePoint(1)[1] > this.minCoordinatePoint(1)[1]
                && polygon.minCoordinatePoint(1)[1] < this.maxCoordinatePoint(1)[1];
            bool isWholeXInside = polygon.minCoordinatePoint(0)[0] < this.minCoordinatePoint(0)[0]
                && polygon.maxCoordinatePoint(0)[0] > this.maxCoordinatePoint(0)[0];
            bool isWholeYInside = polygon.minCoordinatePoint(1)[1] < this.minCoordinatePoint(1)[1]
                && polygon.maxCoordinatePoint(1)[1] > this.maxCoordinatePoint(1)[1];

            bool boundsIntersection = maxXBoundaryInside || maxYBoundaryInside || minXBoundaryInside
                || minYBoundaryInside || isWholeXInside || isWholeYInside;

            if (boundsIntersection)
            {
                for (int i = 0; i < polygon.VerticesNumber; i++)
                {
                    if (isPointInside(polygon[i]))
                    {
                        return polygon[i];
                    }                
                }
            }
            
            MyVector noCollison = new MyVector(-1, -1);
            return noCollison;
        }

        public override MyVector getForceVector(MyVector contactPoint)
        {
            MyVector forceVector = new MyVector(_velocity);
            MyVector massRadius = contactPoint - MassCentre;
            MyVector angularVector = MyMath.normalVector(massRadius);
            angularVector *= _angularVelocity;

            forceVector = (forceVector + angularVector) * Mass;

            return _velocity*Mass;
        }

        public override void actForce(MyVector contactPoint, MyVector forceVector)
        {
            MyVector massRadius = MassCentre - contactPoint;
            MyVector rotationVec = MyMath.normalVector(massRadius);
            double rotationRadius = MyMath.normVector(rotationVec)/100;

            massRadius = MyMath.normalizeVector(massRadius);
            rotationVec = MyMath.normalizeVector(rotationVec);

            double rotationOrientation = Math.Sign(MyMath.orientationBetweenVectors(massRadius, _velocity));

            _velocity = forceVector*(1/Mass);
            _angularVelocity = rotationRadius*0.09 * MyMath.dotProduct(rotationVec, forceVector) / Mass;
            _angularVelocity = Math.Abs(_angularVelocity);
            _angularVelocity *= -rotationOrientation;
            
        }

        public override void forward(double speed)
        {
            MyVector frontVector = MyMath.normalizeVector(this[0] - (this[2] + this[1])*0.5);
            frontVector *= speed;

            _velocity += frontVector;
            //_points += velocity;
        }

        public override Polygon getClone()
        {
            PointVector pointsClone = new PointVector(_points);
            MyVector velocityClone = new MyVector(_velocity);
            Polygon returnPolygon = new Triangle(pointsClone, velocityClone);
            return returnPolygon;
        }
    }
}
