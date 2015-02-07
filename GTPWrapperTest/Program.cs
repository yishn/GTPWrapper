using GTPWrapper;
using GTPWrapper.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTPWrapperTest {
    public class Program {
        static void Main(string[] args) {
            Engine engine = new Engine("GTP\nWrapper\nTest", "1");
            engine.NewCommand += engine_NewCommand;
            engine.ResponsePushed += engine_ResponsePushed;
            engine.ConnectionClosed += engine_ConnectionClosed;

            engine.SupportedCommands.AddRange(new string[] { "showboard", "error" });

            while (true) {
                string input = Console.ReadLine();
                engine.ParseString(input);
            }
        }

        static void engine_ConnectionClosed(object sender, EventArgs e) {
            Environment.Exit(0);
        }

        static void engine_NewCommand(object sender, CommandEventArgs e) {
            Engine engine = (Engine)sender;
            string response = e.Command.Name;

            if (e.Command.Name == "showboard") {
                response = new Board(13).ToString();
            }

            engine.PushResponse(new Response(e.Command, response, e.Command.Name == "error"));
        }

        static void engine_ResponsePushed(object sender, ResponseEventArgs e) {
            Console.Write(e.Response.ToString() + "\n\n");
        }
    }
}
