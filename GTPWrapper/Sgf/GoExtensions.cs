using GTPWrapper.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTPWrapper.Sgf {
    public static class GoExtensions {
        public static string VertexToSgf(this Board board, Vertex vertex) {
            if (vertex == Vertex.Pass || !board.HasVertex(vertex)) return "";
            return (Vertex.Letters[vertex.X - 1].ToString() + Vertex.Letters[board.Size - vertex.Y - 1].ToString()).ToLower();
        }

        public static Vertex SgfToVertex(this Board board, string sgfVertex) {
            if (sgfVertex == "" || board.Size <= 19 && sgfVertex == "tt") return Vertex.Pass;
            return new Vertex(
                Vertex.Letters.IndexOf(sgfVertex[0].ToString().ToUpper()) + 1, 
                board.Size - Vertex.Letters.IndexOf(sgfVertex[1].ToString().ToUpper())
            );
        }
    }
}
