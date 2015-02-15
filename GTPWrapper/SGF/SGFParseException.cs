using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTPWrapper.SGF {
    /// <summary>
    /// The exception that is thrown when there is an error in the parsing process of a SGF string.
    /// </summary>
    public class SGFParseException : Exception {
        public SGFParseException(string message = "") : base(message) { }
    }
}
