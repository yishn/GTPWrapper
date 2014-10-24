using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTPWrapper.DataTypes {
    /// <summary>
    /// Represents a Go board.
    /// </summary>
    public class Board {
        private Dictionary<Vertex, Vertex> ChainAnchor = new Dictionary<Vertex, Vertex>();

        /// <summary>
        /// Contains pairs of vertex and sign with invertible sign.
        /// </summary>
        protected Dictionary<Vertex, Sign> Arrangement = new Dictionary<Vertex, Sign>();

        /// <summary>
        /// Gets or sets the size of the board. Usually 19.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Initializes a new instance of the Board class.
        /// </summary>
        /// <param name="size">The size of the board.</param>
        public Board(int size) {
            this.Size = size;
        }

        /// <summary>
        /// Determines whether the given vertex is on the board or not.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        public bool HasVertex(Vertex vertex) {
            return 1 <= Math.Min(vertex.X, vertex.Y) && Math.Max(vertex.X, vertex.Y) <= this.Size;
        }

        /// <summary>
        /// Gets a list of adjacent vertices of a given vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        public Vertex[] GetNeighborhood(Vertex vertex) {
            if (!HasVertex(vertex)) return new Vertex[] { };

            return new Vertex[] {
                new Vertex(vertex.X - 1, vertex.Y),
                new Vertex(vertex.X + 1, vertex.Y),
                new Vertex(vertex.X, vertex.Y - 1),
                new Vertex(vertex.X, vertex.Y + 1)
            }.Where(v => this.HasVertex(v)).ToArray();
        }

        /// <summary>
        /// Gets the corresponding chain of a given vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="result">A list of all chained vertices.</param>
        public List<Vertex> GetChain(Vertex vertex, List<Vertex> result = null) {
            if (!HasVertex(vertex)) return new List<Vertex>();

            if (result == null) {
                // If calculated already, load from ChainAnchor
                if (ChainAnchor.ContainsKey(vertex)) {                
                    return ChainAnchor.Keys.Where(v => ChainAnchor[v] == ChainAnchor[vertex]).ToList();
                }

                result = new List<Vertex>();
                result.Add(vertex);
                ChainAnchor[vertex] = vertex;
            }

            // Recursive depth-first search
            foreach (Vertex v in GetNeighborhood(vertex)) {
                if (GetSign(v) != GetSign(vertex)) continue;
                if (ChainAnchor.ContainsKey(v)) continue;

                ChainAnchor[v] = ChainAnchor[vertex];
                result.Add(v);
                GetChain(v, result);
            }

            return result;
        }

        /// <summary>
        /// Gets the sign at the given vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        public Sign GetSign(Vertex vertex) {
            return !Arrangement.ContainsKey(vertex) ? 0 : Arrangement[vertex];
        }
        /// <summary>
        /// Sets the sign at the given vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="sign">The sign.</param>
        public void SetSign(Vertex vertex, Sign sign) {
            Arrangement[vertex] = sign;
            ChainAnchor.Clear();
        }

        #region Operators
        
        public static Board operator +(Board b1, Board b2) {
            Board board = new Board(Math.Max(b1.Size, b2.Size));

            foreach (Vertex v in b1.Arrangement.Keys.Union(b2.Arrangement.Keys)) {
                board.SetSign(v, b1.GetSign(v) + b2.GetSign(v));
            }

            return board;
        }

        public static Board operator -(Board b) {
            Board board = new Board(b.Size);

            foreach (Vertex v in b.Arrangement.Keys) {
                board.SetSign(v, -b.GetSign(v));
            }

            return board;
        }

        public static Board operator -(Board b1, Board b2) {
            return -b2 + b1;
        }

        #endregion
    }
}
