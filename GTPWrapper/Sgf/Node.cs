using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTPWrapper.Sgf {
    /// <summary>
    /// Represents a SGF node.
    /// </summary>
    public class Node {
        /// <summary>
        /// The property list of the node.
        /// </summary>
        public List<SgfProperty> Properties { get; set; }

        /// <summary>
        /// Initializes a new Node class.
        /// </summary>
        /// <param name="properties">The property list of the node.</param>
        public Node(List<SgfProperty> properties) {
            this.Properties = properties;
        }
        /// <summary>
        /// Initializes a new Node class.
        /// </summary>
        public Node() : this(new List<SgfProperty>()) { }

        public SgfProperty this[string ident] {
            get { return Properties.FirstOrDefault(x => x.Identifier == ident); }
            set {
                SgfProperty property = Properties.FirstOrDefault(x => x.Identifier == ident);
                if (property != null) Properties.Remove(property);
                Properties.Add(value);
            }
        }

        /// <summary>
        /// Returns a string which represents the object.
        /// </summary>
        public override string ToString() {
            return ";" + string.Join("", this.Properties);
        }
    }
}