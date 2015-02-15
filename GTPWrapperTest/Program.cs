using GTPWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTPWrapperTest {
    public class Program {
        static void Main(string[] args) {
            TestEngine engine = new TestEngine();
            engine.NewCommand += engine_NewCommand;
            engine.ResponsePushed += engine_ResponsePushed;
            engine.ConnectionClosed += engine_ConnectionClosed;

            while (true) {
                string input = Console.ReadLine();
                engine.ParseString(input);
            }
        }

        static void engine_ConnectionClosed(object sender, EventArgs e) {
            Environment.Exit(0);
        }

        static void engine_NewCommand(object sender, CommandEventArgs e) {
            TestEngine engine = (TestEngine)sender;
            engine.ExecuteCommands();
        }

        static void engine_ResponsePushed(object sender, ResponseEventArgs e) {
            Console.WriteLine(e.Response.ToString());
            Console.WriteLine();
        }
    }
}
