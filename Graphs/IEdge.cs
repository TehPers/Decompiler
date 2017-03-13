using System;
using System.Linq;

namespace Graphs {
    /// <summary>An edge containing a source node and target node</summary>
    /// <typeparam name="TVertex">The vertex (or node) type</typeparam>
    public interface IEdge<TVertex> where TVertex : class {

        /// <summary>The source node in the graph</summary>
        TVertex Source { get; }

        /// <summary>The target node in the graph</summary>
        TVertex Target { get; }

    }
}
