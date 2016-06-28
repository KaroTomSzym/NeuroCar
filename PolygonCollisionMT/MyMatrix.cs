using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCollisionMT
{
    public class MyMatrix
    {
        private List<MyVector> _matrix;

        public List<MyVector> GetMatrix
        {
            get
            {
                return _matrix;
            }

        }
        public int RowsNumber
        {
            get
            {
                return _matrix.Count();
            }
        }

        public int ColumnsNumber
        {
            get
            {
                return _matrix[0].Length;
            }
        }

        public MyMatrix()
        {
            _matrix = new List<MyVector>();
        }

        public MyMatrix(List<MyVector> m)
        {
            _matrix = m;
        }

        public MyMatrix(int n, bool isRandom)
        {
            _matrix = new List<MyVector>();
            if (isRandom)
            {
                for (int i = 0; i < n; i++)
                {
                    _matrix.Add(new MyVector(n, true));
                }
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    _matrix.Add(new MyVector(n, false));
                }
            }
        }

        public MyMatrix(int rows, int columns, bool isRandom) // wektor długości c, lista długości rows
        {
            _matrix = new List<MyVector>();
            if (isRandom)
            {
                for (int i = 0; i < rows; i++)
                {
                    _matrix.Add(new MyVector(columns, true));
                }
            }
            else
            {
                for (int i = 0; i < rows; i++)
                {
                    _matrix.Add(new MyVector(columns, false));
                }
            }
        }

        public void Add(MyVector value)
        {
            MyVector tempMv = new MyVector(value.GetVector);
            _matrix.Add(tempMv);
        }

        public void clear()
        {
            _matrix.Clear();
        }

        public MyVector this[int index]
        {
            get
            {
                return this._matrix[index];
            }

        }

        public override string ToString()
        {
            string matrix = "";
            for (int i = 0; i < this.RowsNumber; i++)
            {
                matrix += this[i].ToString() + Environment.NewLine;
            }
            return matrix;
        }
    }
}
