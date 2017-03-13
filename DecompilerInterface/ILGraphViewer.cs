using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Graphs.Visualizer;
using Graphs;
using Teh.Decompiler.Graphs;
using Mono.Cecil.Cil;

namespace DecompilerInterface {
    public partial class ILGraphViewer : UserControl {
        public ILGraph Graph { get; }
        private List<VertexSettings> vertices;

        public new float Padding { get; set; } = 10f;
        public float Spacing { get; set; } = 25f;

        public ILGraphViewer(ILGraph graph) {
            InitializeComponent();

            this.Graph = graph;

            // Defaults
            this.BackColor = Color.White;

            // Create the vertex settings
            Dictionary<Instruction, VertexSettings> vertices = new Dictionary<Instruction, VertexSettings>();
            Dictionary<Instruction, HashSet<Instruction>> targets = graph.Targets;
            float x = Padding;
            float y = Padding;
            foreach (Instruction instruction in graph.Code) {
                if (!vertices.ContainsKey(instruction)) {
                    vertices[instruction] = new VertexSettings(instruction, x, y) {
                        Targets = targets[instruction].Select(i => {
                            if (!vertices.ContainsKey(i))
                                vertices[i] = new VertexSettings(i, 0, 0);
                            return vertices[i];
                        })
                    };
                }

                vertices[instruction].Center = new PointF(x, y);
                x += Spacing;
            }
            this.vertices = vertices.Values.ToList();

            this.Paint += (sender, e) => Redraw();
            this.MouseMove += (sender, e) => Redraw();
        }

        public void Redraw() {
            Bitmap buffer = new Bitmap(this.Width, this.Height);
            Graphics graphics = Graphics.FromImage(buffer);
            graphics.Clear(this.BackColor);
            Point mousePos = this.PointToClient(MousePosition);
            string hoverText = null;

            // Draw edges
            foreach (VertexSettings vertex in vertices) {
                bool hovered = vertex.GetBounds().Contains(mousePos);
                Pen edgePen = hovered ? vertex.EdgeHovered : vertex.Edge;
                foreach (VertexSettings target in vertex.Targets) {
                    if (vertex.Vertex is Instruction instruction && instruction.Next == target.Vertex) {
                        graphics.DrawLine(edgePen, vertex.Center, target.Center);
                    } else {
                        PointF pivot = new PointF((vertex.Center.X + target.Center.X) / 2, vertex.Center.Y + Spacing * 2);
                        graphics.DrawLine(edgePen, vertex.Center, pivot);
                        graphics.DrawLine(edgePen, pivot, target.Center);
                    }
                }
            }

            // Draw vertices
            foreach (VertexSettings vertex in vertices) {
                bool hovered = vertex.GetBounds().Contains(mousePos);
                Brush vertexBrush = hovered ? vertex.FillHovered : vertex.Fill;
                graphics.FillEllipse(vertexBrush, vertex.GetBounds());
                if (hovered) hoverText = vertex.Vertex.ToString();
            }

            if (hoverText != null) {
                SizeF hoverSize = graphics.MeasureString(hoverText, this.Font);
                RectangleF hoverRect = new RectangleF(mousePos + Cursor.Size, hoverSize);
                graphics.FillRectangle(Brushes.Gold, hoverRect);
                graphics.DrawString(hoverText, this.Font, new SolidBrush(this.ForeColor), hoverRect.Location);
            }

            this.CreateGraphics().DrawImage(buffer, 0, 0);
        }
    }
}
