using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs.Visualizer {
    public class VertexSettings {
        public object Vertex { get; }
        public PointF Center { get; set; } = PointF.Empty;
        public float Radius { get; set; } = 5;
        public Brush Fill { get; set; } = Brushes.Black;
        public Pen Edge { get; set; } = Pens.Black;
        public Brush FillHovered { get; set; } = Brushes.Red;
        public Pen EdgeHovered { get; set; } = Pens.Red;
        public IEnumerable<VertexSettings> Targets { get; set; } = Enumerable.Empty<VertexSettings>();

        public VertexSettings(object vertex, float x, float y) : this(vertex, new PointF(x, y)) { }
        public VertexSettings(object vertex, PointF center) {
            this.Vertex = vertex;
            this.Center = center;
        }

        public RectangleF GetBounds() {
            return new RectangleF(this.Center.X - this.Radius, this.Center.Y - this.Radius, this.Radius * 2, this.Radius * 2);
        }
    }
}
