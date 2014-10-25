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
            Engine engine = new Engine();
            engine.NewCommand += engine_NewCommand;
            engine.ResponsePushed += engine_ResponsePushed;

            while (true) {
                string input = Console.ReadLine();
                engine.ParseString(input);
            }
        }

        static void engine_NewCommand(object sender, CommandEventArgs e) {
            Engine engine = (Engine)sender;
            string response = e.Command.Name;

            if (e.Command.Name == "showboard") {
                response = new Board(19).ToString();
            }

            engine.PushResponse(new Response(e.Command, e.Command.Name == "error", response));
        }

        static void engine_ResponsePushed(object sender, ResponseEventArgs e) {
            Console.Write(e.Response.ToString() + "\n\n");
        }
    }
}
