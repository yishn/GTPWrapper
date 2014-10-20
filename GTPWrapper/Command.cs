using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTPWrapper {
    public class Command {
        /// <summary>
        /// An optional command id.
        /// </summary>
        public int? Id { get; private set; }
        /// <summary>
        /// The name of the command.
        /// </summary>
        public string CommandName { get; private set; }
        /// <summary>
        /// A list of arguments.
        /// </summary>
        public List<string> Arguments { get; private set; }

        /// <summary>
        /// Represents a GTP command.
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
        /// Represents a GTP command.
        /// </summary>
        /// <param name="id">An optional command id.</param>
        /// <param name="commandName">The name of the command.</param>
        /// <param name="arguments">A list of arguments, separated by spaces.</param>
        public Command(int? id, string commandName, string arguments) {
            this.Id = id;
            this.CommandName = commandName;
            this.Arguments = arguments.Split(' ').ToList<string>();
        }
    }
}
