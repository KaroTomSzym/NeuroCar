using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCollisionMT
{
    public class Car
    {
        public Triangle carShape { get; set; }
        private MyVector front;
        RangeFinder rangeFinder;
        PolygonManager polygonManager;
        
        double x1 = 100, x2 = 130, x3, y1 = 280, y2 = 280, y3 = 240;

        public Car()
        {
            x3 = (x1 + x2) / 2;
            PointVector pv = new PointVector(x1, y1, x2, y2, x3, y3);
            MyVector velocity = new MyVector(0, 0);

            carShape = new Triangle(pv, velocity);
            front = carShape[2];

            rangeFinder = new RangeFinder(10, 150, 20);

        }

        public void setPolygonManager(PolygonManager pm)
        {
            polygonManager = pm;
        }

        public void control(double rotationAngle, double speed)
        {

            carShape.forward(speed);

            carShape._angularVelocity += rotationAngle;
        }

        public MyVector getDistanceVector()
        {
            MyVector position = carShape.MassCentre;
            List<Polygon> polygonsList = polygonManager.getPolygonList();

            return rangeFinder.rangeInAllDirections(position, polygonsList);

        }
    }
}
