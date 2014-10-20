using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTPWrapper {
    /// <summary>
    /// Represents a GTP command.
    /// </summary>
    public class Command {
        /// <summary>
        /// Gets an optional command id.
        /// </summary>
        public int? Id { get; private set; }
        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        public string CommandName { get; private set; }
        /// <summary>
        /// Gets a list of arguments.
        /// </summary>
        public List<string> Arguments { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Command class.
        /// </summary>
        /// <param name="input">A string of the form "[id] command_name [arguments]"</param>
        public Command(string input) {
            int id, start;
            string[] inputs = input.Split(' ');

            if (int.TryParse(inputs[0], out id)) {
                start = 1;
                this.Id = id;
            } else {
                start = 0;
                this.Id = null;
            }

            this.CommandName = inputs[start];
            this.Arguments = new List<string>(inputs.Skip(start + 1));
        }
        /// <summary>
        /// Initializes a new instance of the Command class.
        /// </summary>
        /// <param name="id">An optional command id.</param>
        /// <param name="commandName">The name of the command.</param>
        /// <param name="arguments">A list of arguments, separated by spaces.</param>
        public Command(int? id, string commandName, string arguments) {
            this.Id = id;
            this.CommandName = commandName;
            this.Arguments = arguments.Split(' ').ToList<string>();
        }

        /// <summary>
        /// Returns a GTP-compliant string
        /// </summary>
        public override string ToString() {
            string result = "";

            result += this.Id.HasValue ? this.Id.ToString() : "";
            result += " " + this.CommandName + " " + string.Join(" ", this.Arguments);

            return result;
        }
    }
}
