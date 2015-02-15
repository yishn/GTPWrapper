using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTPWrapper.Sgf {
    /// <summary>
    /// Represents a SGF node.
    /// </summary>
    public class SgfNode {
        /// <summary>
        /// The property list of the node.
        /// </summary>
        public List<SgfProperty> Properties { get; set; }

        /// <summary>
        /// Initializes a new SgfNode class.
        /// </summary>
        /// <param name="properties">The property list of the node.</param>
        public SgfNode(List<SgfProperty> properties) {
            this.Properties = properties;
        }
        /// <summary>
        /// Initializes a new SgfNode class.
        /// </summary>
        public SgfNode() : this(new List<SgfProperty>()) { }

        public string this[string ident] {
            get {
                SgfProperty property = Properties.FirstOrDefault(x => x.Ident == ident);
                if (property != null) return property.Value;
                throw new InvalidOperationException();
            }
            set {
                SgfProperty property = Properties.FirstOrDefault(x => x.Ident == ident);
                if (property != null) property.Value = value;
                else Properties.Add(new SgfProperty(ident, value));
            }
        }
    }
}