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

        /// <summary>
        /// Returns whether a property with the given identifier exists.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        public bool HasProperty(string identifier) {
            return Properties.Where(x => x.Identifier == identifier).Count() != 0;
        }

        public SgfProperty this[string identifier] {
            get { return Properties.First(x => x.Identifier == identifier); }
            set {
                SgfProperty property = Properties.FirstOrDefault(x => x.Identifier == identifier);
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