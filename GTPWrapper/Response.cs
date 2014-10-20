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
        public Command Command { get; private set; }
        public bool IsError { get; private set; }
        public string Result { get; private set; }

        public Response(Command command, bool isError, string result) {
            this.Command = command;
            this.IsError = isError;
            this.Result = result;
        }

        /// <summary>
        /// Converts the response to a GTP-compliant string.
        /// </summary>
        public override string ToString() {
            string result = "";

            result += this.IsError ? "?" : "=";
            result += this.Command.Id.HasValue ? this.Command.Id.ToString() : "";
            result += " " + this.Result;

            return result;
        }
    }
}
