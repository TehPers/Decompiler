using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTest {
    class Program {
        static void Main(string[] args) {
            Graph testGraph = new Graph();
            testGraph.AddNode(1); // 0
            testGraph.AddNode(2, 3); // 1
            testGraph.AddNode(0); // 2
            testGraph.AddNode(4); // 3
            testGraph.AddNode(3); // 4
            testGraph.AddNode(6); // 5
            testGraph.AddNode(7); // 6
            testGraph.AddNode(5); // 7

            // 0 > 1 > 2 (> 0)
            //     V
            //     3 > 4 (> 3)
            // 5 > 6 > 7 (> 5)

            IEnumerable<IEnumerable<Node>> cycles = testGraph.GetCycles();

            foreach (IEnumerable<Node> cycle in cycles)
                Console.WriteLine(string.Join(", ",
                    from node in cycle
                    select testGraph.Nodes.IndexOf(node)
                    ));

            Console.ReadKey();
        }
    }

    public class Graph {
        public List<Node> Nodes { get; set; } = new List<Node>();

        private int _curIndex = 0;

        public Graph() { }

        public Node AddNode(params int[] edges) {
            Node node = new Node() {
                Edges = edges.ToList()
            };
            Nodes.Add(node);
            return node;
        }

        public IEnumerable<IEnumerable<Node>> GetCycles() {
            Stack<Node> stack = new Stack<Node>();
            List<List<Node>> cycles = new List<List<Node>>();

            foreach (Node n in Nodes) {
                if (!n.Searched) {
                    Tarjan(n, cycles, stack);
                }
            }

            return cycles;
        }

        private void Tarjan(Node node, List<List<Node>> cycles, Stack<Node> stack) {
            node.Index = _curIndex;
            node.LowLink = _curIndex;
            node.Searched = true;
            _curIndex++;
            stack.Push(node);

            foreach (int edge in node.Edges) {
                Node other = Nodes[edge];
                if (!other.Searched) {
                    Tarjan(other, cycles, stack);
                    node.LowLink = Math.Min(node.LowLink, other.LowLink);
                } else if (stack.Contains(other)) {
                    node.LowLink = Math.Min(node.LowLink, other.LowLink);
                }
            }

            if (node.LowLink == node.Index) {
                List<Node> cycle = new List<Node>();
                Node cur;
                do {
                    cur = stack.Pop();
                    cycle.Add(cur);
                } while (cur != node);

                cycles.Add(cycle);
            }
        }
    }

    public class Node {
        public int Index { get; set; }
        public int LowLink { get; set; }
        public bool Searched { get; set; } = false;
        public List<int> Edges { get; set; } = new List<int>();

        public Node() {

        }
    }
}
