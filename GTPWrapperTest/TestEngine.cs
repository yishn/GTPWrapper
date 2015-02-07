using GTPWrapper;
using GTPWrapper.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTPWrapperTest {
    public class TestEngine : Engine {
        public TestEngine() : base("GTP Test Engine", "1.0") {
            this.SupportedCommands.AddRange(new string[] { "showboard", "error" });
        }

        protected override Response ExecuteCommand(Command command) {
            switch (command.Name) {
                case "error":
                    return new Response(command, "an expected error occurred", true);
                case "showboard":
                    return new Response(command, new Board(19).ToString());
            }

            return base.ExecuteCommand(command);
        }
    }
}
