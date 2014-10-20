using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTPWrapper {
    /// <summary>
    /// Represents a GTP engine.
    /// </summary>
    public class Engine {
        /// <summary>
        /// Fired when there is a new command in the queue.
        /// </summary>
        public event EventHandler<CommandEventArgs> NewCommand;

        /// <summary>
        /// Gets the queue which contains all unfinished commands
        /// </summary>
        public Queue<Command> CommandQueue { get; private set; }

        /// <summary>
        /// Initializes a new instance of the GTPEngine class.
        /// </summary>
        public Engine() {
            this.CommandQueue = new Queue<Command>();
        }

        /// <summary>
        /// Adds a command to the queue.
        /// </summary>
        /// <param name="input">Command string</param>
        public void Command(string input) {
            Command command = new Command(input);
            this.CommandQueue.Enqueue(command);

            if (NewCommand != null) NewCommand(this, new CommandEventArgs(command));
        }
    }
}
