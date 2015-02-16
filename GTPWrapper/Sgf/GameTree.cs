using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Diagnostics;

namespace GTPWrapper.Sgf {
    /// <summary>
    /// Represents a SGF game tree.
    /// </summary>
    public class GameTree {
        /// <summary>
        /// The node list of the game tree.
        /// </summary>
        public List<Node> Nodes { get; set; }
        /// <summary>
        /// The list of all subtrees.
        /// </summary>
        public List<GameTree> GameTrees { get; set; }

        /// <summary>
        /// Initializes a new instance of the GameTree class.
        /// </summary>
        public GameTree() {
            this.Nodes = new List<Node>();
            this.GameTrees = new List<GameTree>();
        }

        /// <summary>
        /// Returns a string which represents the object.
        /// </summary>
        public override string ToString() {
            return string.Join("\n", this.Nodes) + (this.Nodes.Count == 0 || this.GameTrees.Count == 0 ? "" : "\n") +
                (this.GameTrees.Count == 0 ? "" : "(" + string.Join(")\n(", this.GameTrees) + ")");
        }
    }
}