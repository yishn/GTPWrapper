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
        public event EventHandler<ResponseEventArgs> ResponsePushed;

        /// <summary>
        /// Gets the queue which contains all unfinished commands
        /// </summary>
        public Queue<Command> CommandQueue { get; private set; }
        /// <summary>
        /// Gets the list of all available responses.
        /// </summary>
        public Dictionary<Command, Response> ResponseList { get; private set; }

        /// <summary>
        /// Initializes a new instance of the GTPEngine class.
        /// </summary>
        public Engine() {
            this.CommandQueue = new Queue<Command>();
            this.ResponseList = new Dictionary<Command, Response>();
        }

        /// <summary>
        /// Adds a command to the queue.
        /// </summary>
        /// <param name="command">The command to add to queue.</param>
        public void AddCommand(Command command) {
            this.CommandQueue.Enqueue(command);
            if (NewCommand != null) NewCommand(this, new CommandEventArgs(command));
        }

        /// <summary>
        /// Pushes a response to the list and removes the associated command from the queue.
        /// </summary>
        /// <param name="response">The response to add to list.</param>
        public void PushResponse(Response response) {
            if (this.CommandQueue.Count == 0) return;

            this.ResponseList.Add(response.Command, response);
            Command c = this.CommandQueue.Peek();

            while (this.ResponseList.ContainsKey(c)) {
                Response r = this.ResponseList[c];
                this.ResponseList.Remove(c);
                this.CommandQueue.Dequeue();
                if (ResponsePushed != null) ResponsePushed(this, new ResponseEventArgs(r));

                c = this.CommandQueue.Count > 0 ? this.CommandQueue.Peek() : null;
            }
        }
    }
}
