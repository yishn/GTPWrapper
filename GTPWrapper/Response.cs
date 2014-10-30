using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTPWrapper {
    /// <summary>
    /// Represents an engine response to a command.
    /// </summary>
    public class Response {
        /// <summary>
        /// Gets the associated command.
        /// </summary>
        public Command Command { get; private set; }
        /// <summary>
        /// Determines whether the response is an error or not.
        /// </summary>
        public bool IsError { get; private set; }
        /// <summary>
        /// Gets the content of the response.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Response class.
        /// </summary>
        /// <param name="command">The associated command.</param>
        /// <param name="isError">Determines whether the response is an error or not.</param>
        /// <param name="content">The content of the response.</param>
        public Response(Command command, string content = "", bool isError = false) {
            this.Command = command;
            this.IsError = isError;
            this.Content = content;
        }

        /// <summary>
        /// Converts the response to a GTP-compliant string.
        /// </summary>
        public override string ToString() {
            string result = "";

            result += this.IsError ? "?" : "=";
            result += this.Command.Id.HasValue ? this.Command.Id.ToString() : "";
            result += " " + this.Content;

            return result;
        }
    }
}
