using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using System.Drawing;

namespace PolygonCollisionMT
{
    public class MyVector
    {
        private List<double> _vector;

        public int Length
        {
            get
            {
                return _vector.Count;
            }
        }

        public MyVector()
        {
            _vector = new List<double>();
        }

        public MyVector(MyVector vector)
        {
            _vector = new List<double>();
            for (int i = 0; i < vector.Length; i++)
            {
                _vector.Add(vector[i]);
            }
        }

        public MyVector(double x, double y)
        {
            _vector = new List<double>();
            _vector.Add(x);
            _vector.Add(y);
        }

        public MyVector(List<double> vec)
        {
            _vector = vec;
        }

        public void Add(double value)
        {
            _vector.Add(value);
        }

        public double norm()
        {
            double norm = 0;
            foreach (double d in _vector)
            {
                norm += d * d;
            }
            norm = Math.Sqrt(norm);
            return norm;
        }

        public MyVector normalVector()
        {
            MyVector normalVec = new MyVector();
            double norm = this.norm();
            foreach (double d in _vector)
            {
                normalVec.Add(d / norm);
            }
            return normalVec;
        }

        public Point getPoint()
        {
            if (_vector.Count != 2)
                throw new Exception();

            Point p = new Point((int)_vector[0], (int)_vector[1]);
            return p;
        }

        //outofindex
        public double this[int index]
        {
            get
            {
                return this._vector[index];
            }
            set
            {
                this._vector[index] = value;
            }

        }

        //l1 != l2 exception
        public static MyVector operator +(MyVector v1, MyVector v2)
        {
            MyVector result = new MyVector();
            for (int i = 0; i < v1.Length; i++)
            {
                result.Add(v1[i] + v2[i]);
            }
            return result;
        }

        public static MyVector operator -(MyVector v1, MyVector v2)
        {
            MyVector result = new MyVector();
            for (int i = 0; i < v1.Length; i++)
            {
                result.Add(v1[i] - v2[i]);
            }
            return result;
        }

        public static MyVector operator *(MyVector v1, double k)
        {
            MyVector result = new MyVector();
            for (int i = 0; i < v1.Length; i++)
            {
                result.Add(v1[i] * k);
            }
            return result;
        }

        public override string ToString()
        {
            string s = "(";
            for (int i = 0; i < Length-1; i++)
            {
                s += ((int)(this[i]*100))/100 + ", ";
            }
            s += ((int)(this[Length-1]*100))/100 + ")";
            return s;
        }
    }
}
