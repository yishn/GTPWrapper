GTPWrapper
==========

```
                                A B C D E F G H J
                              9 X X X X X X O . . 9
                              8 O O O X X O . O O 8
                              7 O . O O X O O O X 7
                              6 . O O X X O O X X 6
                              5 O O X . X O X . X 5
                              4 X X X X X O X X . 4
                              3 . O O X O O O X X 3
                              2 . X X O . O . O X 2
                              1 . . X O O . O O X 1
                                A B C D E F G H J
```

The Go Text Protocol or GTP is used by Go playing engines to communicate between computers or humans. GTPWrapper is a C# library for creating GTP engines. This library implements [GTP Version 2 Specification Draft](http://www.lysator.liu.se/~gunnar/gtp/).

License
-------

This work is licensed under the terms of the MIT license. See LICENSE.txt.

Getting started
---------------

To create a GTP engine is straightforward: Simply create a new class implementing the abstract `Engine` class:

```c#
using GTPWrapper
using GTPWrapper.DataTypes

class TestEngine : Engine {
    public TestEngine() : base("Test Engine", "1.0") {}

    protected override Vertex? GenerateMove(Color color) {
        // Implement move generator
        // Return a Vertex object or null to resign.
        
        if (this.Board["K10"] == Color.Empty)
            return new Vertex("K10");
        
        return null;
    }
}
```

`Engine` can only receive commands and give responses at an abstract level, i.e. you have to execute commands and implement the actual communication with the controller yourself, e.g. like this:

```c#
class Program {
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
        Console.WriteLine(e.Response);
        Console.WriteLine();
    }
}
```

Add or overwrite commands
-------------------------

First, you'll want to add your command to the `SupportedCommands` list of your engine. Then override the `ExecuteCommand` method and you're done:

```c#
class TestEngine : Engine {
    public TestEngine() : base("Test Engine", "1.0") {
        this.SupportedCommands.Add("customcommand");
    }
    
    protected override Response ExecuteCommand(Command command) {
        if (command.Name == "customcommand")
            return new Response(command, "This is my custom command!");
        
        return base.ExecuteCommand(command);
    }
    
    protected override Vertex? GenerateMove(Color color) { return null; }
}
```
