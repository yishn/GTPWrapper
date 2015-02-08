using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTPWrapper.DataTypes {
    /// <summary>
    /// Represents a move.
    /// </summary>
    public struct Move {
        /// <summary>
        /// The color of the move.
        /// </summary>
        public Color Color;
        /// <summary>
        /// The generator vertex of the move.
        /// </summary>
        public Vertex Vertex;

        /// <summary>
        /// Create a new instance of the Move class.
        /// </summary>
        /// <param name="color">The color of the move.</param>
        /// <param name="vertex">The generating vertex of the move.</param>
        public Move(Color color, Vertex vertex) {
            this.Color = color;
            this.Vertex = vertex;
        }
    }
}
