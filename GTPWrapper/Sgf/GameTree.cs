using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using GTPWrapper.DataTypes;

namespace GTPWrapper.Sgf {
    /// <summary>
    /// Represents a SGF game tree.
    /// </summary>
    public class GameTree : ListTree<Node> {
        /// <summary>
        /// Initializes a new instance of the GameTree class.
        /// </summary>
        public GameTree() : base() { }
        /// <summary>
        /// Initializes a new instance of the GameTree class with the given node list.
        /// </summary>
        /// <param name="list"></param>
        public GameTree(List<Node> list) : base(list) { }

        /// <summary>
        /// Returns a string which represents the object.
        /// </summary>
        public override string ToString() {
            return string.Join("\n", this.Elements) + (this.Elements.Count == 0 || this.SubTrees.Count == 0 ? "" : "\n") +
                (this.SubTrees.Count == 0 ? "" : "(" + string.Join(")\n(", this.SubTrees) + ")");
        }
    }
}