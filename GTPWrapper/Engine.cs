using GTPWrapper.DataTypes;
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
        /// Gets a list of past moves.
        /// </summary>
        public List<Board> MoveHistory;
        /// <summary>
        /// Gets the number of captured stones by given color.
        /// </summary>
        public Dictionary<Color, int> Captures;

        /// <summary>
        /// Gets or sets the name of the engine.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the version of the engine.
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// Gets or sets whether the engine is accepting commands.
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Gets or sets the board size.
        /// </summary>
        public int BoardSize { get; set; }
        /// <summary>
        /// Gets or sets the komi.
        /// </summary>
        public float Komi { get; set; }

        /// <summary>
        /// Initializes a new instance of the Engine class.
        /// </summary>
        public Engine(string name, string version = "") {
            this.CommandQueue = new Queue<Command>();
            this.ResponseList = new Dictionary<Command, Response>();
            this.MoveHistory = new List<Board>();

            this.Captures = new Dictionary<Color, int>() {
                { Color.Black, 0 },
                { Color.White, 0 }
            };

            this.SupportedCommands = new List<string>(new string[] {
                "protocol_version", "name", "version", "known_command", "list_commands", "quit",
                "boardsize", "clear_board", "komi", "play", "genmove"
            });

            this.Name = name;
            this.Version = version;
            this.Enabled = true;
        }

        /// <summary>
        /// Adds a command to the queue.
        /// </summary>
        /// <param name="command">The command to add to queue.</param>
        public void AddCommand(Command command) {
            if (!Enabled) return;

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
        /// Executes all commands in the command queue.
        /// </summary>
        public void ExecuteCommands() {
            while (CommandQueue.Count > 0) {
                Response response = ExecuteCommand(CommandQueue.Peek());
                PushResponse(response);
            }
        }

        /// <summary>
        /// Executes a command and returns a corresponding response.
        /// </summary>
        /// <param name="command">The command.</param>
        protected virtual Response ExecuteCommand(Command command) {
            switch (command.Name) {
                case "protocol_version":
                    return new Response(command, "2");
                case "name":
                    return new Response(command, this.Name.Replace('\n', ' '));
                case "version":
                    return new Response(command, this.Version.Replace('\n', ' '));
                case "list_commands":
                    return new Response(command, string.Join("\n", this.SupportedCommands));
                case "known_command":
                    return new Response(
                        command,
                        command.Arguments.Count > 0 && SupportedCommands.Contains(command.Arguments[0]) ? "true" : "false"
                    );
                case "quit":
                    Quit();
                    return new Response(command);
                case "boardsize":
                    try {
                        this.BoardSize = int.Parse(command.Arguments[0]);
                        return new Response(command);
                    } catch {}

                    return new Response(command, "unacceptable size", true);
                case "clear_board":
                    this.MoveHistory.Clear();
                    this.MoveHistory.Add(new Board(this.BoardSize));
                    this.Captures[Color.Black] = this.Captures[Color.White] = 0;

                    return new Response(command);
                case "komi":
                    try {
                        this.Komi = float.Parse(command.Arguments[0]);
                        return new Response(command);
                    } catch {}

                    return new Response(command, "syntax error", true);
            }

            return new Response(command, "unknown command", true);
        }

        /// <summary>
        /// Ends the connection. Corresponds to 'quit'.
        /// </summary>
        public void Quit() {
            this.Enabled = false;
            if (ConnectionClosed != null) ConnectionClosed(this, new EventArgs());
        }
    }
}
