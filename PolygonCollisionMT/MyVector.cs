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
        public List<double> GetVector
        {
            get
            {
                return _vector;
            }

        }
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

        public MyVector(List<double> vec)
        {
            _vector = new List<double>(vec);
        }

        public MyVector(MyVector previousVector)
        {
            _vector = new List<double>();
            for (int i = 0; i < previousVector.Length; i++)
            {
                _vector.Add(previousVector[i]);
            }
        }

        public MyVector(double x, double y)
        {
            _vector = new List<double>();
            _vector.Add(x);
            _vector.Add(y);
        }

        public MyVector(int n, bool isRandom)//losowy wektor
        {
            _vector = new List<double>();
            if (isRandom)
            {
                for (int i = 0; i < n; i++)
                {
                    _vector.Add(MyMath.randInt(-3, 3));
                }
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    _vector.Add(0);
                }
            }
        }

        public void Add(double value)
        {
            _vector.Add(value);
        }

        public void clear()
        {
            _vector.Clear();
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

        public double this[int index]
        {
            get
            {
                if (index >= Length)
                    throw new IndexOutOfRangeException();

                return this._vector[index];
            }
            set
            {
                if (index >= Length)
                    throw new IndexOutOfRangeException();

                this._vector[index] = value;
            }

        }

        public static MyVector operator +(MyVector v1, MyVector v2)
        {
            if (v1.Length != v2.Length)
                throw new FormatException();

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

        public static MyVector operator *(MyVector v, MyMatrix m)
        {
            MyVector tempMv = new MyVector(m[0].Length, false);
            if (v.Length == m.RowsNumber)
            {
                for (int i = 0; i < m[0].Length; i++)
                {
                    for (int j = 0; j < v.Length; j++)
                        tempMv[i] += (v[j] * m[j][i]);
                }
                return tempMv;
            }
            else
            {
                throw new Exception();
            }
        }

        public override string ToString()
        {
            
            string s = "(";
            
            if (Length != 0)
            {
                for (int i = 0; i < Length - 1; i++)
                {
                    s += ((int)(this[i] * 100)) / 100 + ", ";
                }
                s += ((int)(this[Length - 1] * 100)) / 100 + ")";
            }
            return s;
        }


    }
}
