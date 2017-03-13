using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Graphs.Visualizer {
    public class GraphRenderer<TVertex> where TVertex : class {

        public Graph<TVertex> Graph { get; }

        // Drawing settings
        // TODO: Customizable vertex and edge color options for each one individually
        public int Padding { get; set; } = 10;
        public int VertexSpacing { get; set; } = 25;
        public Color ArrowColor { get; set; } = Color.Black;

        public GraphRenderer(Graph<TVertex> graph) {
            this.Graph = graph;
        }

        // TODO: Draw edges too
        public IEnumerable<VertexSettings> Render() {
            // Find all targets in the graph
            Dictionary<TVertex, HashSet<TVertex>> targets = new Dictionary<TVertex, HashSet<TVertex>>();
            foreach (IEdge<TVertex> edge in Graph.GetEdges()) {
                if (!targets.ContainsKey(edge.Source)) targets[edge.Source] = new HashSet<TVertex>();
                targets[edge.Source].Add(edge.Target);
            }

            // Fill in layers
            Dictionary<TVertex, VertexSettings> drawSettings = targets.Keys.ToDictionary(
                v => v,
                v => new VertexSettings(v, 0, 0)
                );
            HashSet<VertexSettings> drawnVertices = new HashSet<VertexSettings>();
            if (Graph.GetEdges().Any()) {
                // Get roots
                IEnumerable<TVertex> roots = Graph.GetRoots();
                if (roots.Skip(1).Any())
                    throw new NotSupportedException("Cannot draw graphs with multiple roots");

                // Add vertices, basically by doing a BFS starting at the roots
                float x = 0;
                HashSet<TVertex> visited = new HashSet<TVertex>();
                HashSet<TVertex> curLayer = new HashSet<TVertex>(roots);
                while (curLayer.Any()) {
                    HashSet<TVertex> nextLayer = new HashSet<TVertex>();
                    HashSet<VertexSettings> vertices = new HashSet<VertexSettings>();
                    float nextX = x;
                    float y = 0;
                    foreach (TVertex v in curLayer) {
                        // Add connected vertices to the next layer
                        if (targets.ContainsKey(v)) {
                            foreach (TVertex target in targets[v]) {
                                if (!visited.Contains(target)) {
                                    nextLayer.Add(target);
                                    visited.Add(target);
                                }
                            }
                        }

                        // Get or create the vertex settings object and set its center
                        if (!drawSettings.TryGetValue(v, out VertexSettings vertex)) {
                            vertex = new VertexSettings(v, x, y);
                            drawSettings[v] = vertex;
                        } else {
                            vertex.Center = new PointF(x, y);
                        }

                        // Get or create the targets' vertex settings
                        if (targets.ContainsKey(v)) {
                            vertex.Targets = targets[v].Select(vert => {
                                if (drawSettings.TryGetValue(vert, out VertexSettings vertSettings)) {
                                    return vertSettings;
                                }
                                vertSettings = new VertexSettings(vert, PointF.Empty);
                                drawSettings[vert] = vertSettings;
                                return vertSettings;
                            });
                        }

                        // Adjust the coordinates for the next vertex
                        // TODO: only add spacing if it isn't the last vertex
                        nextX = Math.Max(nextX, x + 2 * vertex.Radius + VertexSpacing);
                        y += vertex.Radius * 2 + VertexSpacing;
                        vertices.Add(vertex);
                    }
                    x = nextX;
                    curLayer = nextLayer;

                    // Center the layer and add the vertices to vertexSettings
                    float h = vertices.Last().GetBounds().Bottom - vertices.First().GetBounds().Top;
                    foreach (VertexSettings vertex in vertices) {
                        vertex.Center = new PointF(vertex.Center.X, vertex.Center.Y - h / 2);
                        drawnVertices.Add(vertex);
                    }
                }
            }

            // Adjust vertex y values and get the size
            var _xs = drawnVertices.Select(v => v.GetBounds().Left).OrderBy(n => n);
            float minX = _xs.First();
            var _ys = drawnVertices.Select(v => v.GetBounds().Top).OrderBy(n => n);
            float minY = _ys.First();
            float width = 0f;
            float height = 0f;
            foreach (VertexSettings vertex in drawnVertices) {
                vertex.Center = new PointF(vertex.Center.X - minX, vertex.Center.Y - minY);
                width = Math.Max(width, vertex.GetBounds().Right);
                height = Math.Max(height, vertex.GetBounds().Bottom);
            }
            
            return drawnVertices;
        }
    }
}
