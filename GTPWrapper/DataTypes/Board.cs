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
        private Dictionary<Vertex, IEnumerable<Vertex>> Liberties = new Dictionary<Vertex, IEnumerable<Vertex>>();

        /// <summary>
        /// Contains pairs of vertex and signs.
        /// </summary>
        protected Dictionary<Vertex, Sign> Arrangement = new Dictionary<Vertex, Sign>();
        /// <summary>
        /// Gets the number of captured stones by given color.
        /// </summary>
        public Dictionary<Color, int> Captures;

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
            this.Captures = new Dictionary<Color, int>() {
                { Color.Black, 0 },
                { Color.White, 0 }
            };
        }

        /// <summary>
        /// Returns whether the board is empty or not.
        /// </summary>
        public bool IsEmpty() {
            return Arrangement.Where(pair => pair.Value != 0).Count() == 0;
        }

        /// <summary>
        /// Determines whether the given vertex is on the board or not.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        public bool HasVertex(Vertex vertex) {
            return vertex == Vertex.Pass || 1 <= Math.Min(vertex.X, vertex.Y) && Math.Max(vertex.X, vertex.Y) <= this.Size;
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
        public IEnumerable<Vertex> GetChain(Vertex vertex, List<Vertex> result = null) {
            if (!HasVertex(vertex)) return new List<Vertex>();

            if (result == null) {
                // If calculated already, load from ChainAnchor
                if (ChainAnchor.ContainsKey(vertex)) {                
                    return ChainAnchor.Keys.Where(v => ChainAnchor[v] == ChainAnchor[vertex]);
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
        /// Gets the list of liberties of the chain represented by the given vertex.
        /// </summary>
        /// <param name="vertex">A vertex which represents the chain.</param>
        public IEnumerable<Vertex> GetLiberties(Vertex vertex) {
            if (GetSign(vertex) == 0) return new List<Vertex>();

            // If calculated already, load from Liberties
            if (ChainAnchor.ContainsKey(vertex) && Liberties.ContainsKey(ChainAnchor[vertex])) {
                return Liberties[ChainAnchor[vertex]];
            }

            IEnumerable<Vertex> chain = GetChain(vertex);
            IEnumerable<Vertex> liberties = new List<Vertex>();

            foreach (Vertex c in chain) {
                liberties = liberties.Union(GetNeighborhood(c).Where(x => GetSign(x) == 0));
            }

            Liberties[ChainAnchor[vertex]] = liberties;
            return Liberties[ChainAnchor[vertex]];
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
        /// <exception cref="System.InvalidOperationException">Thrown when the vertex is not on the board.</exception>
        public void SetSign(Vertex vertex, Sign sign) {
            if (!HasVertex(vertex)) throw new InvalidOperationException("Vertex is not on the board.");

            Arrangement[vertex] = sign;
            ChainAnchor.Clear();
            Liberties.Clear();
        }

        /// <summary>
        /// Get a list of all vertices on the board except the pass vertex.
        /// </summary>
        public IEnumerable<Vertex> GetVertices() {
            for (int i = 1; i <= this.Size; i++) {
                for (int j = 1; j <= this.Size; j++) {
                    yield return new Vertex(i, j);
                }
            }
        }
        /// <summary>
        /// Get a list of vertices with the given sign.
        /// </summary>
        /// <param name="sign">The sign.</param>
        public IEnumerable<Vertex> GetVertices(Sign sign) {
            if (sign != 0)
                return Arrangement.Where(pair => pair.Value == sign).Select(pair => pair.Key);

            return this.GetVertices().Where(v => this.GetSign(v) == 0);
        }

        /// <summary>
        /// Clears the board.
        /// </summary>
        public void Clear() {
            this.Arrangement.Clear();
            this.Captures[Color.Black] = this.Captures[Color.White] = 0;
        }

        /// <summary>
        /// Returns a new Board that represents the given Move.
        /// </summary>
        /// <param name="move">The corresponding move.</param>
        /// <param name="allowSuicide">Determines whether suicide is allowed or not.</param>
        public Board MakeMove(Move move, bool allowSuicide = true) {
            Board diff = new Board(this.Size);
            Vertex vertex = move.Vertex;
            Sign sign = move.Color;

            if (vertex == Vertex.Pass) return this + new Board(this.Size);
            if (!this.HasVertex(vertex) || this.GetSign(vertex) != 0) throw new InvalidOperationException("Illegal move.");

            diff.SetSign(vertex, sign);
            bool suicide = true;

            foreach (Vertex v in this.GetNeighborhood(vertex)) {
                if (this.GetSign(v) != -sign) continue;
                if (this.GetLiberties(v).Count() != 1) continue;
                if (!this.GetLiberties(v).Contains(vertex)) continue;

                foreach (Vertex c in this.GetChain(v)) {
                    diff.SetSign(v, sign);
                }

                suicide = false;
            }

            // Detect suicide
            if (suicide) {
                this.SetSign(vertex, sign);
                IEnumerable<Vertex> chain = this.GetChain(vertex);
                suicide = this.GetLiberties(vertex).Count() == 0;
                this.SetSign(vertex, 0);

                if (suicide) {
                    if (!allowSuicide) throw new InvalidOperationException("Suicidal move.");
                    foreach (Vertex v in chain) {
                        diff.SetSign(v, -this.GetSign(v));
                    }
                }
            }
            
            Board result = this + diff;
            result.Captures[(Color)sign] += diff.Arrangement
                .Where(pair => pair.Value == sign && pair.Key != vertex)
                .Count();
            if (suicide) result.Captures[(Color)(-sign)] += diff.Arrangement
                .Where(pair => pair.Value == -sign || pair.Key == vertex)
                .Count();

            return result;
        }

        #region Operators

        public Sign this[Vertex v] {
            get { return this.GetSign(v); }
            set { this.SetSign(v, value); }
        }
        
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

        /// <summary>
        /// A string that represents the current board position.
        /// </summary>
        public override string ToString() {
            string result = "";
            List<Vertex> hoshi = GetHandicapPlacement(9);

            for (int y = this.Size + 1; y >= 0; y--) {
                if (y == 0 || y == this.Size + 1) {
                    result += "\n   ";
                    for (int i = 0; i < this.Size; i++) {
                        result += " " + Vertex.Letters[i];
                    }
                    continue;
                }

                result += "\n";
                for (int x = 0; x <= this.Size + 1; x++) {
                    if (x == 0 || x == this.Size + 1) {
                        result += x != 0 || y >= 10 ? " " : "  ";
                        result += y.ToString();
                        continue;
                    }

                    char c = GetSign(new Vertex(x, y)) == 1 ? 'X' : GetSign(new Vertex(x, y)) == -1 ? 'O' : '.';
                    if (c == '.' && hoshi.Contains(new Vertex(x, y))) c = '+';
                    result += " " + c;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets fixed placement of handicap stones.
        /// </summary>
        /// <param name="count">The number of handicap stones. Between 2 and 9.</param>
        public List<Vertex> GetHandicapPlacement(int count) {
            if (this.Size < 6 || count < 2) return new List<Vertex>();

            int near = this.Size >= 13 ? 4 : 3;
            int far = this.Size + 1 - near;

            List<Vertex> result = new List<Vertex>(new Vertex[] {
                new Vertex(near, near), new Vertex(far, far),
                new Vertex(near, far), new Vertex(far, near)
            });

            if (this.Size % 2 != 0) {
                int middle = (this.Size + 1) / 2;
                if (count == 5) result.Add(new Vertex(middle, middle));
                result.AddRange(new Vertex[] { 
                    new Vertex(near, middle), new Vertex(far, middle) 
                });
                if (count == 7) result.Add(new Vertex(middle, middle));
                result.AddRange(new Vertex[] { 
                    new Vertex(middle, near), new Vertex(middle, far), new Vertex(middle, middle) 
                });
            }

            return result.Take(count).ToList();
        }
    }
}