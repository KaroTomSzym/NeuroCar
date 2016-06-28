using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCollisionMT
{
    public class NeuralNetwork
    {
        private List<MyMatrix> _layers;
        private MyVector _currentAnswer;
        private double _coefficientActivationFunction;

        public List<MyMatrix> GetLayers
        {
            get
            {
                return _layers;
            }
        }

        public MyVector CurrentAnswer
        {
            get
            {
                return _currentAnswer;
            }
            set
            {
                _currentAnswer = value;
            }
        }

        public NeuralNetwork()
        {
            _layers = new List<MyMatrix>();
            _coefficientActivationFunction = 1;
        }

        public NeuralNetwork(NeuralNetwork nn)
        {
            _layers = new List<MyMatrix>();
            _layers = nn._layers;
            _coefficientActivationFunction = nn._coefficientActivationFunction;
        }

        public NeuralNetwork(int firstLayerSize, int innerLayerSize, int lastLayerSize, int innerLayerNumber, double coefficientActivationFunction)
        {
            _coefficientActivationFunction = coefficientActivationFunction;
            _layers = new List<MyMatrix>();

            MyMatrix firstMatrix = new MyMatrix(firstLayerSize + 1, innerLayerSize, true);
            _layers.Add(firstMatrix);
            for (int i = 0; i < innerLayerNumber; i++)
            {
                MyMatrix tempMatrix = new MyMatrix(innerLayerSize + 1, innerLayerSize, true);
                _layers.Add(tempMatrix);
            }
            MyMatrix lastMatrix = new MyMatrix(innerLayerSize + 1, lastLayerSize, true);
            _layers.Add(lastMatrix);
        }

        public MyVector answer(MyVector inputVector)
        {
            inputVector.Add(1);

            if (inputVector.Length != _layers[0].RowsNumber)
            {
                throw new Exception();
            }
            else
            {
                MyVector tempV = inputVector * _layers[0];
                tempV = MyMath.activationFunction(tempV, _coefficientActivationFunction);
                tempV.Add(1);
                int i;
                for (i = 1; i < _layers.Count() - 1; i++)
                {
                    tempV = tempV * _layers[i];
                    tempV = MyMath.activationFunction(tempV, _coefficientActivationFunction);
                    tempV.Add(1);
                }
                _currentAnswer = tempV * _layers[i];
                return _currentAnswer;
            }
        }

        public void Add(MyMatrix value)
        {
            _layers.Add(value);
        }

        public void clear()
        {
            _layers.Clear();
        }

        public string showLayers()
        {
            string layers = "";
            foreach (var l in _layers)
            {
                layers += l.ToString() + Environment.NewLine + Environment.NewLine;
            }
            return layers;
        }
    }
}
