using GTPWrapper.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTPWrapper.Sgf {
    public static class GoExtensions {
        public static string VertexToSgf(this Board board, Vertex vertex) {
            return (Vertex.Letters[vertex.X - 1].ToString() + Vertex.Letters[board.Size - vertex.Y - 1].ToString()).ToLower();
        }

        public static Vertex SgfToVertex(this Board board, string sgfVertex) {
            return new Vertex(Vertex.Letters.IndexOf(sgfVertex[0]) + 1, board.Size - Vertex.Letters.IndexOf(sgfVertex[1]));
        }
    }
}
