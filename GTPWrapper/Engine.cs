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
    public abstract class Engine {
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
        public Stack<Tuple<Move, Board>> MoveHistory;

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
        /// Gets or sets the board.
        /// </summary>
        public Board Board { get; set; }
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
            this.MoveHistory = new Stack<Tuple<Move, Board>>();

            this.SupportedCommands = new List<string>(new string[] {
                "protocol_version", "name", "version", "known_command", "list_commands", "quit",
                "boardsize", "clear_board", "komi", "fixed_handicap", "set_free_handicap",
                "play", "genmove", "undo", "showboard"
            });

            this.Name = name;
            this.Version = version;
            this.Enabled = true;
            this.Board = new Board(19);
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
                if (r.Command.Name == "quit") Quit();

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
                    return new Response(command);
                case "boardsize":
                    try {
                        int size = int.Parse(command.Arguments[0]);
                        if (size > 25) throw new Exception();
                        this.Board = new Board(size);
                        return new Response(command);
                    } catch {}

                    return new Response(command, "unacceptable size", true);
                case "clear_board":
                    this.MoveHistory.Clear();
                    this.Board.Clear();

                    return new Response(command);
                case "komi":
                    try {
                        this.Komi = float.Parse(command.Arguments[0]);
                        return new Response(command);
                    } catch {}

                    return new Response(command, "syntax error", true);
                case "fixed_handicap":
                    if (!this.Board.IsEmpty()) return new Response(command, "board not empty", true);

                    try {
                        int count = int.Parse(command.Arguments[0]);
                        if (count < 2 || count > 9 || this.Board.Size < 7)
                            return new Response(command, "invalid number of stones", true);

                        foreach (Vertex v in Board.GetHandicapPlacement(count)) {
                            Board.SetSign(v, 1);
                        }
                        return new Response(command);
                    } catch {}
                    
                    return new Response(command, "syntax error", true);
                case "set_free_handicap":
                    if (!this.Board.IsEmpty()) return new Response(command, "board not empty", true);

                    try {
                        List<Vertex> vs = new List<Vertex>();
                        foreach (string input in command.Arguments) {
                            Vertex v = new Vertex(input);
                            if (v == Vertex.Pass || vs.Contains(v)) return new Response(command, "bad vertex list", true);
                            vs.Add(v);
                        }

                        foreach (Vertex v in vs) this.Board.SetSign(v, 1);
                        return new Response(command);
                    } catch {}

                    return new Response(command, "syntax error", true);
                case "play":
                    try {
                        Move move = Move.Parse(string.Join(" ", command.Arguments));
                        MoveHistory.Push(Tuple.Create(move, this.Board));

                        this.Board = this.Board.MakeMove(move);
                        return new Response(command);
                    } catch(FormatException) {
                        return new Response(command, "syntax error", true);
                    } catch (InvalidOperationException) {
                        return new Response(command, "illegal move", true);
                    }
                case "genmove":
                    try {
                        Color color = (Color)Enum.Parse(typeof(Color), command.Arguments[0], true);
                        Vertex? vertex = GenerateMove(color);

                        if (vertex.HasValue) {
                            // Make move and add to move history
                            Move move = new Move(color, vertex.Value);
                            MoveHistory.Push(Tuple.Create(move, this.Board));

                            this.Board = this.Board.MakeMove(move);
                        }

                        return new Response(command, vertex.HasValue ? vertex.ToString() : "resign");
                    } catch {}

                    return new Response(command, "syntax error", true);
                case "undo":
                    if (MoveHistory.Count == 0) return new Response(command, "cannot undo", true);

                    Tuple<Move, Board> tuple = MoveHistory.Pop();
                    this.Board = tuple.Item2;

                    return new Response(command);
                case "showboard":
                    string result = this.Board.ToString() + "\n-\n";
                    result += "(X) captured " + this.Board.Captures[Color.Black] + "\n";
                    result += "(O) captured " + this.Board.Captures[Color.White];
                    return new Response(command, result);
            }

            return new Response(command, "unknown command", true);
        }

        /// <summary>
        /// Generates a move with the given color on the current board.
        /// </summary>
        /// <param name="color">The color of the move.</param>
        protected abstract Vertex? GenerateMove(Color color);

        /// <summary>
        /// Ends the connection. Corresponds to 'quit'.
        /// </summary>
        public void Quit() {
            this.Enabled = false;
            if (ConnectionClosed != null) ConnectionClosed(this, new EventArgs());
        }
    }
}
