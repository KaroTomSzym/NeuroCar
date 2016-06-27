using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolygonCollisionMT
{
    public partial class Form1 : Form
    {
        Graphics _canvas;
        PolygonManager _polygonMng;
        Car _car;
        List<Point[]> _polygonsToErase;
        Road _road;

        //protected override CreateParams CreateParams // nie miga
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;
        //        return cp;
        //    }
        //}

        public Form1()
        {
            InitializeComponent();
            _canvas = splitContainer1.Panel2.CreateGraphics();
            int boundaryX = splitContainer1.Panel2.Width;
            int roadLength = 10000;
            _car = new Car();
            //
            PointVector pv = new PointVector((double)numericUpDown1.Value, (double)numericUpDown2.Value, (double)numericUpDown3.Value, (double)numericUpDown4.Value, (double)numericUpDown5.Value, (double)numericUpDown6.Value);

            MyVector velocity = new MyVector(1,1);

            _car.carShape = new Triangle(pv, velocity);
            //
            _polygonMng = new PolygonManager(boundaryX, roadLength, _car.carShape);

            _road = new Road(roadLength);
            _road.generatePolygons(100);

            _polygonMng.setPolygonList(_road.getVisiblePolygons());
            _polygonsToErase = new List<Point[]>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _canvas.Clear(Color.White);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PointVector pv = new PointVector((double)numericUpDown1.Value, (double)numericUpDown2.Value, (double)numericUpDown3.Value, (double)numericUpDown4.Value, (double)numericUpDown5.Value, (double)numericUpDown6.Value);

            MyVector velocity = new MyVector(0,0);

            _car.carShape = new Triangle(pv, velocity);
            //PolygonMng.addPolygon(Car);
            //CarControl = new CarControler(Car);

            MyVector obstacleVec = new MyVector(0, 69);
            pv += obstacleVec;
            _polygonMng.addPolygon(new Triangle(pv, velocity));
            pv += obstacleVec; pv += obstacleVec;
            pv += obstacleVec;
            _polygonMng.addPolygon(new Triangle(pv, velocity));

            obstacleVec = new MyVector(69, 0);
            pv += obstacleVec;
            _polygonMng.addPolygon(new Triangle(pv, velocity));

            MyVector p1 = new MyVector(50, 50);
            MyVector p2 = new MyVector(89, 110);
            MyVector p3 = new MyVector(120, 100);
            MyVector p4 = new MyVector(100, 57);
            PointVector tetra = new PointVector();
            tetra.Add(p1);
            tetra.Add(p2);
            tetra.Add(p3);
            tetra.Add(p4);
            Tetragon tetragon = new Tetragon(tetra);
            _polygonMng.addPolygon(tetragon);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _road.addRandomPolygonToList(0, 300, _road.VisibleArea.CurrentAreaPosition, _road.VisibleArea.CurrentAreaPosition + 400);
            _polygonMng.movePolygons();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            _polygonMng.setPolygonList(_road.getVisiblePolygons());
            //drawPolygons();
            updateArea();
            textBox1.Text = _polygonMng.lastForce.ToString();
            Invalidate();
        }

        private void updateArea()
        {
            _road.VisibleArea.CurrentAreaPosition = (int)_polygonMng.carPolygon.MassCentre[1] - 300;//(int)_road.Car.carShape.MassCentre[0];
            drawPolygons();
            Point[] startLine = new Point[2];
            startLine[0] = new Point(0, 0 - _road.VisibleArea.CurrentAreaPosition);
            startLine[1] = new Point(300, 0 - _road.VisibleArea.CurrentAreaPosition);
            _canvas.DrawLines(new Pen(Color.Black), startLine);
        }

        private void drawPolygons()
        {
            try
            {

                List<Point[]> polygonsToDraw = _polygonMng.getPolygonsPointsList();
                //List<Point[]> tempPolygonsToDrawPositions = new List<Point[]>();


                ////ponizej jest kopiowanie tablicy, ale chuja to dziala
                //for (int i = 0; i < polygonsToDraw.Count; i++)
                //{
                //    Point[] tempPolygonPoints = polygonsToDraw[i]; //new Point[polygonsToDraw[i].Length];
                //    for (int j = 0; j < tempPolygonPoints.Length; j++)
                //    {
                //        Point tempPoint = new Point();
                //        tempPoint.X = tempPolygonPoints[j].X;
                //        tempPoint.Y = tempPolygonPoints[j].Y;
                //        tempPolygonPoints[j] = tempPoint;
                //    }
                //    tempPolygonsToDrawPositions.Add(tempPolygonPoints);
                //}

                //List<Point[]> polygonsToErase = PolygonMng.getPolygonsBeforeMovePointsList();
                _canvas.Clear(Color.White);

                foreach (Point[] e in _polygonsToErase)
                {
                    _canvas.DrawLines(new Pen(Color.White), e);
                }

                foreach (Point[] p in polygonsToDraw)
                {
                    for (int i = 0; i < p.Length; i++)
			        {
                        p[i].Y = p[i].Y - _road.VisibleArea.CurrentAreaPosition;// + 400;
			        }
                    _canvas.DrawLines(new Pen(Color.Red), p);
                }

                
                               

                _polygonsToErase = polygonsToDraw;
                
                //foreach (Point[] p in polygonsToErase)
                //{
                //    canvas.DrawLines(new Pen(Color.White), p);
                //}
                //foreach (Point[] p in polygonsToDraw)
                //{
                //    canvas.DrawLines(new Pen(Color.Red), p);
                //}
            }
            catch (Exception)
            {
            }
        }

        private bool checkIfMoved(Point[] polygonPoints, Point[] polygonBeforeMovePoints)
        {
            for (int i = 0; i < polygonPoints.Length; i++)
            {
                if (polygonPoints[i] != polygonBeforeMovePoints[i])
                {
                    return true;
                }
            }
            return false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _polygonMng.stopThread();
        }

        private void button2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
