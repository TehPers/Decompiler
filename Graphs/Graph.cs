using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Graphs;

namespace Graphs {

    // Maybe implement ISet<IEdge<TVertex>> as well? *shrug*
    public class Graph<TVertex> : ICollection<IEdge<TVertex>> where TVertex : class {
        private HashSet<IEdge<TVertex>> Edges { get; }
        private HashSet<TVertex> Vertices { get; }

        public Graph() {
            this.Edges = new HashSet<IEdge<TVertex>>();
            this.Vertices = new HashSet<TVertex>();
        }

        public Graph(IEnumerable<IEdge<TVertex>> edges) {
            this.Edges = new HashSet<IEdge<TVertex>>(edges);
            this.Vertices = new HashSet<TVertex>(
                from edge in edges
                from vertex in new TVertex[] { edge.Source, edge.Target }
                select vertex
                );
        }

        #region Graph Operations

        /// <summary>Must iterate through all edges</summary>
        public virtual Dictionary<TVertex, HashSet<TVertex>> Targets {
            get {
                Dictionary<TVertex, HashSet<TVertex>> output = new Dictionary<TVertex, HashSet<TVertex>>();

                foreach (IEdge<TVertex> edge in Edges) {
                    if (output.TryGetValue(edge.Source, out HashSet<TVertex> targets)) {
                        targets.Add(edge.Target);
                    } else {
                        output[edge.Source] = new HashSet<TVertex>() { edge.Target };
                    }

                    if (!output.ContainsKey(edge.Target))
                        output[edge.Target] = new HashSet<TVertex>();
                }

                return output;
            }
        }

        /// <summary>Must iterate through all edges</summary>
        public virtual Dictionary<TVertex, HashSet<TVertex>> Sources {
            get {
                Dictionary<TVertex, HashSet<TVertex>> output = new Dictionary<TVertex, HashSet<TVertex>>();

                foreach (IEdge<TVertex> edge in Edges) {
                    if (output.TryGetValue(edge.Target, out HashSet<TVertex> sources)) {
                        sources.Add(edge.Source);
                    } else {
                        output[edge.Target] = new HashSet<TVertex>() { edge.Source };
                    }

                    if (!output.ContainsKey(edge.Source))
                        output[edge.Source] = new HashSet<TVertex>();
                }

                return output;
            }
        }

        public virtual IEnumerable<IEdge<TVertex>> GetEdges() {
            foreach (IEdge<TVertex> edge in this.Edges)
                yield return edge;
        }

        public virtual IEnumerable<TVertex> GetRoots() {
            HashSet<TVertex> containingPrevious = new HashSet<TVertex>();
            foreach (IEdge<TVertex> edge in this.Edges)
                if (this.Vertices.Contains(edge.Target))
                    containingPrevious.Add(edge.Target);

            return this.Vertices.Except(containingPrevious);
        }

        public virtual IEnumerable<IEnumerable<TVertex>> GetCycles() {
            int curIndex = 0;
            List<List<TVertex>> cycles = new List<List<TVertex>>();
            Stack<TVertex> stack = new Stack<TVertex>();
            Dictionary<TVertex, VertexInfo> data = this.Vertices.ToDictionary(
                vertex => vertex,
                vertex => new VertexInfo()
                );

            Dictionary<TVertex, HashSet<TVertex>> targets = this.Targets;

            foreach (TVertex vertex in this.Vertices)
                if (!data[vertex].Searched)
                    Tarjan(vertex);

            return cycles;

            void Tarjan(TVertex vertex)
            {
                VertexInfo curNode = data[vertex];
                curNode.Index = curIndex;
                curNode.LowLink = curIndex;
                curNode.Searched = true;
                curIndex++;
                stack.Push(vertex);

                foreach (TVertex target in targets[vertex]) {
                    VertexInfo targetNode = data[target];
                    if (!targetNode.Searched) {
                        Tarjan(target);
                        curNode.LowLink = Math.Min(curNode.LowLink, targetNode.LowLink);
                    } else if (stack.Contains(target)) {
                        curNode.LowLink = Math.Min(curNode.LowLink, targetNode.LowLink);
                    }
                }

                if (curNode.LowLink == curNode.Index) {
                    List<TVertex> cycle = new List<TVertex>();
                    TVertex cur;
                    do {
                        cur = stack.Pop();
                        cycle.Add(cur);
                    } while (!ReferenceEquals(cur, vertex));

                    // If the cycle has multiple elements or there is an edge from the only element in the cycle to itself
                    if (cycle.Skip(1).Any() || this.Edges.Any(edge => ReferenceEquals(edge.Source, cycle.First()) && ReferenceEquals(edge.Target, edge.Source)))
                        cycles.Add(cycle);
                }
            }
        }

        private class VertexInfo {
            public int Index { get; set; } = -1;
            public int LowLink { get; set; } = -1;
            public bool Searched { get; set; } = false;
        }

        #endregion

        #region Collection Operations
        public int Count => this.Edges.Count;
        public bool IsReadOnly => false;

        public void Add(IEdge<TVertex> item) {
            this.Edges.Add(item);
            this.Vertices.Add(item.Source);
            this.Vertices.Add(item.Target);
        }

        public void Clear() {
            this.Edges.Clear();
            this.Vertices.Clear();
        }

        public bool Contains(IEdge<TVertex> item) {
            return this.Edges.Contains(item);
        }

        public void CopyTo(IEdge<TVertex>[] array, int arrayIndex) {
            this.Edges.CopyTo(array, arrayIndex);
        }

        public bool Remove(IEdge<TVertex> item) {
            return this.Edges.Remove(item);
        }
        #endregion

        #region Enumerators
        public IEnumerator<IEdge<TVertex>> GetEnumerator() {
            return this.Edges.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.Edges.GetEnumerator();
        }
        #endregion
    }
}
