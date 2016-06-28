using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCollisionMT
{
    public static class FileComander
    {
        private static XmlWriter _xmlWriter;
        public static NeuralNetwork readFile(string strXml)
        {
            NeuralNetwork nn = new NeuralNetwork();
            MyVector tempMv = new MyVector();
            MyMatrix tempMm = new MyMatrix();

            using (XmlReader _xmlReader = XmlReader.Create(new StringReader(strXml)))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(_xmlReader);

                XmlNodeList xmlNodes = xmlDocument.GetElementsByTagName("layers");

                foreach (XmlNode layers in xmlNodes)
                {
                    foreach (XmlNode layer in layers)
                    {
                        tempMm = new MyMatrix();
                        foreach (XmlNode row in layer)
                        {
                            tempMv = new MyVector();
                            foreach (XmlNode column in row)
                            {
                                tempMv.Add(Convert.ToDouble(column.InnerText));
                            }
                            tempMm.Add(tempMv);
                        }
                        nn.Add(tempMm);
                    }
                }
                xmlNodes = xmlDocument.GetElementsByTagName("answer");

                tempMv = new MyVector();
                foreach (XmlNode column in xmlNodes[0])
                {
                    tempMv.Add(Convert.ToDouble(column.InnerText));
                }
                nn.CurrentAnswer = tempMv;
            }
            return nn;
        }

        public static void writeXmlFile(string name, NeuralNetwork nn)
        {
            if (nn != null)
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "\t";

                using (_xmlWriter = XmlWriter.Create(name, settings))
                {
                    _xmlWriter.WriteStartDocument();
                    _xmlWriter.WriteStartElement("NeuralNetwork");
                    _xmlWriter.WriteStartElement("layers");
                    foreach (var l in nn.GetLayers)
                    {
                        _xmlWriter.WriteStartElement("layer");
                        foreach (var mv in l.GetMatrix)
                        {
                            _xmlWriter.WriteStartElement("row");
                            foreach (var d in mv.GetVector)
                            {
                                _xmlWriter.WriteStartElement("column");
                                _xmlWriter.WriteString(d.ToString());
                                _xmlWriter.WriteEndElement();
                            }
                            _xmlWriter.WriteEndElement();
                        }
                        _xmlWriter.WriteEndElement();
                    }
                    _xmlWriter.WriteEndElement();
                    _xmlWriter.WriteStartElement("answer");
                    foreach (var d in nn.CurrentAnswer.GetVector)
                    {
                        _xmlWriter.WriteStartElement("column");
                        _xmlWriter.WriteString(d.ToString());
                        _xmlWriter.WriteEndElement();
                    }
                    _xmlWriter.WriteEndElement();
                    _xmlWriter.WriteEndDocument();
                }
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(name);
            }
        }
    }
}
