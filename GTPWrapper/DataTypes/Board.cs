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
        private Dictionary<Vertex, Sign> arrangement = new Dictionary<Vertex, Sign>();

        /// <summary>
        /// The letters in a vertex string, i.e. the letters A to Z, excluding I.
        /// </summary>
        public static string Letters = "ABCDEFGHJKLMNOPQRSTUVWXYZ";

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
        /// Gets the sign at the given vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        public Sign GetSign(Vertex vertex) {
            return !arrangement.ContainsKey(vertex) ? 0 : arrangement[vertex];
        }
        /// <summary>
        /// Sets the sign at the given vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="sign">The sign.</param>
        public void SetSign(Vertex vertex, Sign sign) {
            arrangement[vertex] = sign;
        }
    }
}
