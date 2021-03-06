﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace PolygonCollisionMT
{
    public class PolygonManager
    {
        private List<Polygon> _polygons;
        private List<Polygon> _polygonsBeforeMove;
        private int _boundaryX;
        private int _boundaryY;
        private Thread PolygonManagerThread;
        private Random _random;
        private bool running;
        public Polygon carPolygon;
        public Polygon carPolygonBeforeMove;
        //
        public MyVector lastForce;
        //
        public PolygonManager(int boundaryX, int boundaryY,Polygon car)
        {
            _polygons = new List<Polygon>();
            _boundaryX = boundaryX;
            _boundaryY = boundaryY;
            startThread();
            running = true;
            //
            carPolygon = car;
            lastForce = new MyVector(0,0);
            //
            _random = new Random();
        }

        public void threadAction(object data)
        {
            while (true)
            {
                if (running)
                movePolygons();

                Thread.Sleep(20);
            }
        }

        public void startThread()
        {
            PolygonManagerThread = new Thread(this.threadAction);
            PolygonManagerThread.Start();
        }

        public void stopThread()
        {
            PolygonManagerThread.Abort();
        }

        public void addPolygon(Polygon p)
        {
            _polygons.Add(p);
        }

        public List<Point[]> getPolygonsPointsList()
        {
            List<Point[]> polygonsPointsList = new List<Point[]>();

            try
            {
                foreach (Polygon p in _polygons)
                {
                    polygonsPointsList.Add(p.getPointsTable());
                }
                polygonsPointsList.Add(carPolygon.getPointsTable());
            }
            catch (InvalidOperationException)
            {
                return polygonsPointsList;
            }
            
            return polygonsPointsList;
        }

        public List<Point[]> getPolygonsBeforeMovePointsList()
        {
            List<Point[]> polygonsPointsList = new List<Point[]>();

            try
            {
                foreach (Polygon p in _polygonsBeforeMove)
                {
                    polygonsPointsList.Add(p.getPointsTable());
                }
                //polygonsPointsList.Add(carPolygonBeforeMove.getPointsTable());
            }
            catch (InvalidOperationException)
            {
                return polygonsPointsList;
            }

            return polygonsPointsList;
        }

        public void movePolygons()
        {
            
            checkBoundaryCollisions();
            checkObstacleCollison();
            try
            {
                _polygonsBeforeMove = new List<Polygon>(_polygons);
                foreach (Polygon p in _polygons)
                {
                    p.shift();
                    p.rotate();
                }
                carPolygonBeforeMove = new Triangle(carPolygon);
                _polygonsBeforeMove.Add(carPolygonBeforeMove);
                carPolygon.shift();
                carPolygon.rotate();
            } 
            catch(InvalidOperationException){

            }
            
        }

        public void checkObstacleCollison()
        {
            foreach (Polygon p in _polygons)
            {
                MyVector contactPoint = carPolygon.polygonCollision(p);
                if (contactPoint[0] != -1)
                {
                    MyVector massCentre = carPolygon.MassCentre - p.MassCentre;
                    MyVector force = carPolygon.getForceVector(contactPoint);
                    Double forceValue = MyMath.normVector(force);
                    force =  MyMath.normalizeVector(force+massCentre) * forceValue;
                    carPolygon.actForce(contactPoint, force);
                    //p.actForce(contactPoint, force*-1);
                }
                else
                {
                    MyVector contactPoint2 = p.polygonCollision(carPolygon);
                    if (contactPoint2[0] != -1)
                    {
                        MyVector massCentre = carPolygon.MassCentre - p.MassCentre;
                        MyVector force = carPolygon.getForceVector(contactPoint2);
                        Double forceValue = MyMath.normVector(force);
                        force = MyMath.normalizeVector(force + massCentre) * forceValue;
                        carPolygon.actForce(contactPoint2, force);
                        //p.actForce(contactPoint2, force * -1);
                    }
                }
            }
        }

        public void checkBoundaryCollisions()
        {

            MyVector contactPoint = carPolygon.boundaryCollison(0, _boundaryX, 0, _boundaryY);
            //boundary collison zwraca punkt (-1,0) jeśli brak kolizji
            if (contactPoint[0] != -1)
            {
                //x=0 lub x = max
                if (contactPoint[0] < 0 || contactPoint[0] > _boundaryX)
                {
                    MyVector force = carPolygon.getForceVector(contactPoint);
                    force[0] *= -1;
                    carPolygon.actForce(contactPoint, force);
                    lastForce = force;
                }
                //y=0 lub y = max
                if (contactPoint[1] < 0 || contactPoint[1] > _boundaryY)
                {
                    MyVector force = carPolygon.getForceVector(contactPoint);
                    force[1] *= -1;
                    carPolygon.actForce(contactPoint, force);
                    lastForce = force;
                }
            }
            //foreach (Polygon p in _polygons)
            //{
            //    MyVector contactPoint = p.boundaryCollison(0, _boundaryX, 0, _boundaryY);
            //    //boundary collison zwraca punkt (-1,0) jeśli brak kolizji
            //    if (contactPoint[0] != -1)
            //    {
            //        //x=0 lub x = max
            //        if (contactPoint[0] < 0 || contactPoint[0] > _boundaryX)
            //        {
            //            MyVector force = p.getForceVector(contactPoint);
            //            force[0] *= -1;
            //            p.actForce(contactPoint, force);
            //            lastForce = force;
            //        }
            //        //y=0 lub y = max
            //        if (contactPoint[1] < 0 || contactPoint[1] > _boundaryY)
            //        {
            //            MyVector force = p.getForceVector(contactPoint);
            //            force[1] *= -1;
            //            p.actForce(contactPoint, force);
            //            lastForce = force;
            //        }
            //    }
            //}        
        }

        public void addRandomPolygonToList(int minPosition, int maxPosition)
        {
            PointVector pv = new PointVector();
            double r1, tempX, tempY;
            int avgSideLength = 50;
            int x1 = _random.Next(minPosition, maxPosition);
            int x2 = _random.Next(minPosition, maxPosition);

            pv.Add(new MyVector(x1, x2));
            r1 = _random.Next(0, 360);
            x1 = x1 + (int)(avgSideLength * Math.Cos(r1));
            x2 = x2 + (int)(avgSideLength * Math.Sin(r1));
            pv.Add(new MyVector(x1, x2));

            MyVector tempVector = new MyVector(x1, x2);
            double distance = 0;
            while(distance < avgSideLength*0.8 || distance > avgSideLength*1.2)
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
