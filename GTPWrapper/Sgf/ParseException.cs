using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTPWrapper.Sgf {
    /// <summary>
    /// The exception that is thrown when there is an error in the parsing process of a SGF string.
    /// </summary>
    public class ParseException : Exception {
        /// <summary>
        /// Initializes a new instance of the ParseException class.
        /// </summary>
        /// <param name="message">An optional error message.</param>
        public ParseException(string message = "") : base(message) { }
    }
}
