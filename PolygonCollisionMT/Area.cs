using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PolygonCollisionMT
{
    public class Area
    {
        public Size Size { get; set; }
        public int CurrentAreaPosition { get; set; }

        public Area(int width, int height)
        {
            Size = new Size(width, height);
            CurrentAreaPosition = 0;
        }
    }
}
