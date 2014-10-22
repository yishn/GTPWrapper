using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTPWrapper.DataTypes {
    /// <summary>
    /// Represents a Go board coordinate.
    /// </summary>
    public struct Vertex {
        /// <summary>
        /// Gets or sets the x coordinate of the point.
        /// </summary>
        public int X;
        /// <summary>
        /// Gets or sets the y coordinate of the point.
        /// </summary>
        public int Y;

        /// <summary>
        /// Initializes a new instance of the Vertex class with the given coordinates.
        /// </summary>
        /// <param name="x">The horizontal position of the point.</param>
        /// <param name="y">The vertical position of the point.</param>
        public Vertex(int x, int y) {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the Vertex class with the given coordinate.
        /// </summary>
        /// <param name="vertex">The board coordinate consisting of one letter and one number.</param>
        public Vertex(string vertex) {
            this.X = 0;
            this.Y = 0;
        }
    }
}
