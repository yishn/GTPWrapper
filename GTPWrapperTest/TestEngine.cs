using GTPWrapper;
using GTPWrapper.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTPWrapperTest {
    public class TestEngine : Engine {
        Random random = new Random();

        public TestEngine() : base("GTP Test Engine", "1.0") {
            this.SupportedCommands.AddRange(new string[] { "error" });
        }

        protected override Response ExecuteCommand(Command command) {
            switch (command.Name) {
                case "error":
                    return new Response(command, "an expected error occurred", true);
            }

            return base.ExecuteCommand(command);
        }

        protected override Vertex? GenerateMove(Color color) {
            List<Vertex> list = Board.GetVertices(Color.Empty).ToList();
            Vertex v = Vertex.Pass;

            while (v == Vertex.Pass || !this.IsLegal(new Move(color, v))) {
                if (list.Count == 0) return Vertex.Pass;
                int i = random.Next(0, list.Count);

                v = list[i];
                list.RemoveAt(i);
            }

            return v;
        }
    }
}
