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
        /// Fired when there is a response ready.
        /// </summary>
        public event EventHandler<ResponseEventArgs> ResponsePushed;
        /// <summary>
        /// Fired when the command 'quit' is received.
        /// </summary>
        public event EventHandler ConnectionClosed;

        /// <summary>
        /// Gets the list of supported command names.
        /// </summary>
        public List<string> SupportedCommands;
        /// <summary>
        /// Gets the queue which contains all unfinished commands
        /// </summary>
        public Queue<Command> CommandQueue;
        /// <summary>
        /// Gets the list of all available responses.
        /// </summary>
        public Dictionary<Command, Response> ResponseList;

        /// <summary>
        /// Gets or sets the name of the engine.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the version of the engine.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Initializes a new instance of the Engine class.
        /// </summary>
        public Engine(string name, string version) {
            this.SupportedCommands = new List<string>(new string[] {
                "protocol_version", "name", "version", "known_command", "list_commands", "quit", "boardsize",
                "clear_board", "komi", "play", "genmove"
            });
            this.CommandQueue = new Queue<Command>();
            this.ResponseList = new Dictionary<Command, Response>();

            this.Name = name;
            this.Version = version;

            this.NewCommand += Engine_NewCommand;
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
        /// Parses a string and adds each command to the queue.
        /// </summary>
        /// <param name="input">The string to be parsed.</param>
        public void ParseString(string input) {
            string[] lines = input.Split('\n');

            foreach (string l in lines) {
                string line = l.Trim();
                if (line == "" || line.StartsWith("#")) continue;
                if (line.IndexOf('#') >= 0) line = line.Substring(0, line.IndexOf('#'));

                this.AddCommand(new Command(line));
            }
        }

        /// <summary>
        /// Pushes a response to the list and removes the associated command from the queue.
        /// </summary>
        /// <param name="response">The response to add to list.</param>
        public void PushResponse(Response response) {
            if (this.CommandQueue.Count == 0) return;
            if (!this.CommandQueue.Contains(response.Command)) return;

            this.ResponseList.Add(response.Command, response);
            Command c = this.CommandQueue.Peek();

            while (this.ResponseList.ContainsKey(c)) {
                Response r = this.ResponseList[c];
                this.ResponseList.Remove(c);
                this.CommandQueue.Dequeue();
                if (ResponsePushed != null) ResponsePushed(this, new ResponseEventArgs(r));

                if (this.CommandQueue.Count == 0) break;
                c = this.CommandQueue.Peek();
            }
        }

        /// <summary>
        /// Ends the connection. Corresponds to 'quit'.
        /// </summary>
        public void Quit() {
            if (ConnectionClosed != null) ConnectionClosed(this, new EventArgs());
        }

        private void Engine_NewCommand(object sender, CommandEventArgs e) {
            if (!SupportedCommands.Contains(e.Command.Name)) {
                this.PushResponse(new Response(e.Command, "unknown command", true));
                return;
            }

            switch (e.Command.Name) { 
                case "protocol_version":
                    PushResponse(new Response(e.Command, "2"));
                    break;
                case "name":
                    PushResponse(new Response(e.Command, this.Name));
                    break;
                case "version":
                    PushResponse(new Response(e.Command, this.Version));
                    break;
                case "list_commands":
                    PushResponse(new Response(e.Command, string.Join("\n", this.SupportedCommands)));
                    break;
                case "known_command":
                    PushResponse(new Response(
                        e.Command,
                        e.Command.Arguments.Count > 0 && SupportedCommands.Contains(e.Command.Arguments[0]) ? "true" : "false"
                    ));
                    break;
                case "quit":
                    PushResponse(new Response(e.Command));
                    Quit();
                    break;
            }
        }
    }
}
