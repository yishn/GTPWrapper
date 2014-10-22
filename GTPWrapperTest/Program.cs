using GTPWrapper;
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
                engine.AddCommand(new Command(input));
            }
        }

        static void engine_NewCommand(object sender, CommandEventArgs e) {
            Engine engine = (Engine)sender;
            engine.PushResponse(new Response(e.Command, false, e.Command.CommandName));
        }

        static void engine_ResponsePushed(object sender, ResponseEventArgs e) {
            Console.WriteLine(e.Response.ToString() + "\n");
        }
    }
}
