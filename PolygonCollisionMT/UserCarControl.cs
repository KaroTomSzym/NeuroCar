using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace PolygonCollisionMT
{
    public partial class UserCarControl : UserControl
    {
        Graphics _canvas;
        Point mousePositionOnClick;
        Point currentMousePosition;
        MyVector controlVector;
        Car _car;
        double speed;
        double angle;
        bool mouseDown;

        public UserCarControl()
        {
            InitializeComponent();
            _canvas = panel1.CreateGraphics();
        }


        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                currentMousePosition = this.panel1.PointToClient(MousePosition);
                int Xvalue = (currentMousePosition.X -mousePositionOnClick.X);
                int Yvalue = -(currentMousePosition.Y -mousePositionOnClick.Y);

                
                
                MyVector markedVector = new MyVector(Xvalue, Yvalue)*(-1);
                MyVector controlVec=  MyMath.polarRepresentation(markedVector);                double vecLength = MyMath.normVector(markedVector);
                speed = controlVec[0] * 0.01;
                angle = ((Math.PI - controlVec[1]) % (2 * Math.PI))*0.01;

                textBox1.Text = controlVec[0].ToString();
                textBox2.Text = angle.ToString();

                drawOnScreen();
                
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            mousePositionOnClick = this.panel1.PointToClient(MousePosition);
            currentMousePosition = this.panel1.PointToClient(MousePosition);
            currentMousePosition.X += 1;
            drawPoint(mousePositionOnClick);
                    
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            clear();
        }

        public void setCar(Car car)
        {
            _car = car;
        }

        private void clear()
        {
            _canvas.Clear(panel1.BackColor);
        }

        private void drawPoint(Point p)
        {
            int x = p.X - 3;
            int y = p.Y - 3;
            int xsize = 6;
            int ysize = 6;
            Rectangle rectangle = new Rectangle(x, y, xsize, ysize);
            _canvas.FillEllipse(new SolidBrush(Color.Blue), rectangle);   
        }

        private void drawLine(Point p1, Point p2)
        {
            _canvas.DrawLine(new Pen(Color.Blue), p1, p2);
        }

        private void drawOnScreen()
        {
            clear();
            drawPoint(mousePositionOnClick);
            drawPoint(currentMousePosition);
            drawLine(mousePositionOnClick, currentMousePosition);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (mouseDown)
            {
                _car.control(angle, speed);
            }
        }

    }
}
