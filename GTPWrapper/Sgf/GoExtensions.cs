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
        /// The letters in a SGF vertex string, i.e. the letters a to z.
        /// </summary>
        public static string Letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Converts given vertex on given board into SGF coordinates.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="vertex">The vertex.</param>
        public static string VertexToSgf(this Board board, Vertex vertex) {
            if (vertex == Vertex.Pass || !board.HasVertex(vertex)) return "";
            return Letters[vertex.X - 1].ToString() + Letters[board.Size - vertex.Y].ToString();
        }

        /// <summary>
        /// Converts given SGF coordinates on given board into a vertex.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="sgfVertex">The SGF coordinates.</param>
        public static Vertex SgfToVertex(this Board board, string sgfVertex) {
            if (sgfVertex.Length > 2) throw new FormatException("Wrong SGF vertex format.");
            if (sgfVertex == "" || board.Size <= 19 && sgfVertex == "tt") return Vertex.Pass;
            return new Vertex(
                Letters.IndexOf(sgfVertex[0].ToString().ToLower()) + 1, 
                board.Size - Letters.IndexOf(sgfVertex[1].ToString().ToLower())
            );
        }
    }
}
