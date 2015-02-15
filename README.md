GTPWrapper
==========

The Go Text Protocol or GTP is used by Go playing engines to communicate between computers or humans. GTPWrapper is a C# wrapper for the GTP protocol. This library implements [GTP Version 2 Specification Draft](http://www.lysator.liu.se/~gunnar/gtp/).

License
-------

This work is licensed under the terms of the MIT license. See LICENSE.txt.

Getting started
---------------

First, you need to create an GTP engine by implementing the abstract `Engine` class:

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
using GTPWrapper
using GTPWrapper.DataTypes

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
