using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTPWrapper.Sgf {
    /// <summary>
    /// Represents a SGF property.
    /// </summary>
    public class SgfProperty {
        /// <summary>
        /// The ident of the property.
        /// </summary>
        public string Ident { get; set; }
        /// <summary>
        /// The value list of the property.
        /// </summary>
        public List<string> Values { get; set; }
        /// <summary>
        /// A fixed, canonical value.
        /// </summary>
        public string Value { get { return Values[0]; } set { Values[0] = value; } }

        /// <summary>
        /// Initializes a new instance of the SgfProperty class.
        /// </summary>
        /// <param name="ident">The ident.</param>
        /// <param name="values">A list of values.</param>
        public SgfProperty(string ident, List<string> values) {
            this.Ident = ident;
            this.Values = values;
        }
        /// <summary>
        /// Initializes a new instance of the SgfProperty class.
        /// </summary>
        /// <param name="ident">The ident.</param>
        /// <param name="value">The value.</param>
        public SgfProperty(string ident, string value) : this(ident, new string[] { value }.ToList()) { }
    }
}