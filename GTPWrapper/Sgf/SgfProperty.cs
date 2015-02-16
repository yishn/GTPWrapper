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
        /// The identifier of the property.
        /// </summary>
        public string Identifier { get; set; }
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
        /// <param name="identifier">The identifier.</param>
        /// <param name="values">A list of values.</param>
        public SgfProperty(string identifier, List<string> values) {
            this.Identifier = identifier;
            this.Values = values;
        }
        /// <summary>
        /// Initializes a new instance of the SgfProperty class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="value">The value.</param>
        public SgfProperty(string identifier, string value) : this(identifier, new string[] { value }.ToList()) { }

        /// <summary>
        /// Escapes a minimal set of characters (\, ]) by replacing them with their escape codes.
        /// </summary>
        /// <param name="input">The input string.</param>
        public static string Escape(string input) {
            return input.Replace(@"\", @"\\").Replace(@"]", @"\]");
        }

        /// <summary>
        /// Returns a string which represents the object.
        /// </summary>
        public override string ToString() {
            return this.Identifier + "[" + string.Join("][", this.Values.Select(x => SgfProperty.Escape(x))) + "]";
        }
    }
}