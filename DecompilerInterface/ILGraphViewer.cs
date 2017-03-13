using Graphs.Visualizer;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Teh.Decompiler.Graphs;

namespace DecompilerInterface {
    public partial class ILGraphViewer : PictureBox {
        private SortedSet<VertexSettings> vertices = new SortedSet<VertexSettings>(new VertexComparer());

        public ILGraph Graph { get; }
        public new float Padding { get; set; } = 10f;
        public float Spacing { get; set; } = 25f;
        public VertexSettings Selected { get; set; } = null;
        public VertexSettings Hovered { get; set; } = null;
        public Point ClientMousePosition => this.PointToClient(MousePosition);

        public ILGraphViewer(ILGraph graph) {
            InitializeComponent();

            this.Graph = graph;

            // Defaults
            this.BackColor = Color.White;

            // Create the vertex settings
            Dictionary<Instruction, VertexSettings> vertices = new Dictionary<Instruction, VertexSettings>();
            Dictionary<Instruction, HashSet<Instruction>> targets = graph.Targets;
            float x = this.Padding;
            float y = this.Padding;
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
                x += this.Spacing;
            }
            this.vertices = new SortedSet<VertexSettings>(vertices.Values, new VertexComparer());

            this.Size = this.GetPreferredSize(this.Size);

            this.Paint += (sender, e) => Redraw();
            this.MouseMove += (sender, e) => {
                VertexSettings prevHovered = this.Hovered;
                this.Hovered = this.vertices.Where(vertex => vertex.GetBounds().Contains(ClientMousePosition)).LastOrDefault();
                if (prevHovered != this.Hovered) Redraw();
            };
            this.Click += (sender, e) => Select(ClientMousePosition);
        }

        public void Select(float x, float y) => Select(new PointF(x, y));
        public void Select(PointF point) {
            VertexSettings lastSelected = this.Selected;
            this.Selected = this.vertices.Where(vertex => vertex.GetBounds().Contains(point)).LastOrDefault();
            if (this.Selected != lastSelected) Redraw();
        }

        public override Size GetPreferredSize(Size proposedSize) {
            Size s = proposedSize;
            if (Parent != null) {
                s.Width = (int) Math.Max(s.Width, Parent.Width);
                s.Height = (int) Math.Max(s.Height, Parent.Height);
            }
            if (vertices.Any()) {
                s.Width = (int) Math.Max(s.Width, 2 * Padding + vertices.Last().Center.X);
                s.Height = (int) Math.Max(s.Height, (
                    from vertex in vertices
                    where vertex.Vertex is Instruction
                    let vx = vertex.Center.X
                    from target in vertex.Targets
                    where (vertex.Vertex as Instruction).Next != target.Vertex
                    let tx = target.Center.X
                    select 2 * Padding + vx + Math.Abs(tx - vx) / 2
                    ).LastOrDefault());
            }
            return s;
        }

        public void Redraw() {
            Stopwatch drawClock = new Stopwatch();
            Stopwatch tmpClock = new Stopwatch();
            long edgeTime = 0;
            long vertexTime = 0;
            long hoverTime = 0;

            drawClock.Start();
            Bitmap buffer = new Bitmap(this.Width, this.Height);
            using (Graphics graphics = Graphics.FromImage(buffer)) {
                Point mousePos = PointToClient(MousePosition);

                // Draw edges
                tmpClock.Start();
                foreach (VertexSettings vertex in this.vertices) {
                    Pen edgePen;
                    if (Hovered == vertex || Selected == vertex)
                        edgePen = vertex.EdgeHovered;
                    else if (Selected == null)
                        edgePen = vertex.Edge;
                    else
                        edgePen = vertex.EdgeDelected;
                    foreach (VertexSettings target in vertex.Targets) {
                        if (vertex.Vertex is Instruction instruction && instruction.Next == target.Vertex) {
                            graphics.DrawLine(edgePen, vertex.Center, target.Center);
                        } else {
                            PointF pivot = new PointF((vertex.Center.X + target.Center.X) / 2, vertex.Center.Y + Math.Abs(target.Center.X - vertex.Center.X) / 2);
                            graphics.DrawLine(edgePen, vertex.Center, pivot);
                            graphics.DrawLine(edgePen, pivot, target.Center);
                        }
                    }
                }
                tmpClock.Stop();
                edgeTime = tmpClock.ElapsedMilliseconds;

                // Draw vertices
                tmpClock.Restart();
                foreach (VertexSettings vertex in this.vertices) {
                    Brush vertexBrush;
                    if (Hovered == vertex || Selected == vertex)
                        vertexBrush = vertex.FillHovered;
                    else if (Selected == null || Selected.Targets.Contains(vertex))
                        vertexBrush = vertex.Fill;
                    else
                        vertexBrush = vertex.FillDelected;
                    graphics.FillEllipse(vertexBrush, vertex.GetBounds());
                }
                tmpClock.Stop();
                vertexTime = tmpClock.ElapsedMilliseconds;

                tmpClock.Restart();
                if (Hovered != null) {
                    string hoverText = Hovered.ToString();
                    SizeF hoverSize = graphics.MeasureString(hoverText, this.Font);
                    RectangleF hoverRect = new RectangleF(mousePos + Cursor.Size, hoverSize);
                    hoverRect.Offset(
                        hoverRect.Right > this.Width ? this.Width - hoverRect.Right : 0,
                        hoverRect.Bottom > this.Height ? this.Height - hoverRect.Bottom : 0
                        );
                    graphics.FillRectangle(Brushes.Gold, hoverRect);
                    graphics.DrawString(hoverText, this.Font, new SolidBrush(this.ForeColor), hoverRect.Location);
                }
                tmpClock.Stop();
                hoverTime = tmpClock.ElapsedMilliseconds;
            }
            drawClock.Stop();
            Console.WriteLine($"IL Graph: Redrew in {drawClock.ElapsedMilliseconds} milliseconds. (Edges: {edgeTime}ms, Vertices: {vertexTime}ms, Hover: {hoverTime}ms)");

            Image old = this.Image;
            this.Image = buffer;
            if (old != null)
                old.Dispose();
        }

        private class VertexComparer : IComparer<VertexSettings> {
            public int Compare(VertexSettings a, VertexSettings b) {
                return a.Center.X.CompareTo(b.Center.X);
            }
        }
    }
}
