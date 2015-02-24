using GTPWrapper.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTPWrapper.Sgf {
    /// <summary>
    /// Provides SGF extension methods for the game of Go.
    /// </summary>
    public static class GoExtensions {
        /// <summary>
        /// Converts given vertex on given board into SGF coordinates.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="vertex">The vertex.</param>
        public static string VertexToSgf(this Board board, Vertex vertex) {
            if (vertex == Vertex.Pass || !board.HasVertex(vertex)) return "";
            return (Vertex.Letters[vertex.X - 1].ToString() + Vertex.Letters[board.Size - vertex.Y - 1].ToString()).ToLower();
        }

        /// <summary>
        /// Converts given SGF coordinates on given board into a vertex.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="sgfVertex">The SGF coordinates.</param>
        public static Vertex SgfToVertex(this Board board, string sgfVertex) {
            if (sgfVertex == "" || board.Size <= 19 && sgfVertex == "tt") return Vertex.Pass;
            return new Vertex(
                Vertex.Letters.IndexOf(sgfVertex[0].ToString().ToUpper()) + 1, 
                board.Size - Vertex.Letters.IndexOf(sgfVertex[1].ToString().ToUpper())
            );
        }
    }
}
