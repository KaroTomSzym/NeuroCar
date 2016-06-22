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
        Graphics canvas;
        PolygonManager PolygonMng;
        Car Car;

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
            canvas = splitContainer1.Panel2.CreateGraphics();
            int boundaryX = splitContainer1.Panel2.Width;
            int boundaryY = splitContainer1.Panel2.Height;
            Car = new Car();
            //
            PointVector pv = new PointVector((double)numericUpDown1.Value, (double)numericUpDown2.Value, (double)numericUpDown3.Value, (double)numericUpDown4.Value, (double)numericUpDown5.Value, (double)numericUpDown6.Value);

            MyVector velocity = new MyVector(1,-1);

            Car.carShape = new Triangle(pv, velocity);
            //
            PolygonMng = new PolygonManager(boundaryX, boundaryY, Car.carShape);

            PolygonMng.addRandomPolygonToList(0, 200);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            canvas.Clear(Color.White);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PointVector pv = new PointVector((double)numericUpDown1.Value, (double)numericUpDown2.Value, (double)numericUpDown3.Value, (double)numericUpDown4.Value, (double)numericUpDown5.Value, (double)numericUpDown6.Value);

            MyVector velocity = new MyVector(0,0);

            Car.carShape = new Triangle(pv, velocity);
            //PolygonMng.addPolygon(Car);
            //CarControl = new CarControler(Car);

            MyVector obstacleVec = new MyVector(0, 69);
            pv += obstacleVec;
            PolygonMng.addPolygon(new Triangle(pv, velocity));
            pv += obstacleVec; pv += obstacleVec;
            pv += obstacleVec;
            PolygonMng.addPolygon(new Triangle(pv, velocity));

            obstacleVec = new MyVector(69, 0);
            pv += obstacleVec;
            PolygonMng.addPolygon(new Triangle(pv, velocity));

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
            PolygonMng.addPolygon(tetragon);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PolygonMng.addRandomPolygonToList(0, 200);
            PolygonMng.movePolygons();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            drawPolygons();
            textBox1.Text = PolygonMng.lastForce.ToString();
            Invalidate();
        }

        private void drawPolygons()
        {
            try
            {
                List<Point[]> polygonsToDraw = PolygonMng.getPolygonsPointsList();
                List<Point[]> polygonsToErase = PolygonMng.getPolygonsBeforeMovePointsList();
                //canvas.Clear(Color.White);
                for (int i = 0; i < polygonsToDraw.Count; i++)
                {
                    Point[] polygonToDraw = polygonsToDraw[i];
                    Point[] polygonToErase = polygonsToErase[i];
                    
                    if (checkIfMoved(polygonToDraw, polygonToErase))
                    {
                        canvas.DrawLines(new Pen(Color.White), polygonToErase);
                    }
                    canvas.DrawLines(new Pen(Color.Red), polygonToDraw);
                }
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
            PolygonMng.stopThread();
        }

        private void button2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
