# Developer Console

I made this system to handle executing commands for C# projects/games.

If you find a bug or have a suggestion feel free to open an issue or a create a pull request

## Example Usage

### Init

You need to call ``DevConsole.Initialize();`` before using the console system.   
You can then set your output interface like so ``DevConsole.SetOutput(new SystemConsoleOutput());``

See ``SystemConsoleOutput.cs`` as an example in order to make your own output.
### Console Command

Commands must be static methods. The reason i designed it to not support non static methods is because i felt as though it would lead to spaghetti code if you have random commands thrown into gameplay code. With static methods you can have your commands self contained in a single static class that accesses managers for specific gameplay objects.

```cs
// Commands can have multiple command aliases
[ConCommand("testcmd")]
[ConCommandDesc("Explain how to use this command and what it does here")]
private static void Clear()
{
    ConsoleSystem.Clear();
}
```

Commands use explicit types for command parameters
You can use any built in C# data type

* bool
* byte
* sbyte
* decimal
* double
* float
* int
* uint
* long
* ulong
* short
* ushort
* char
* string

Example command with parameters

```cs
[ConCommand("add")]
[ConCommandDesc("add two numbers together and print output")]
private static void Add(int a, int b)
{
   DevConsole.LogMessage((a + b).ToString());
}
```

### ConVar

Console variables are a way to manipulate a static field. You can use these for stuff like storing settings for game variables and user settings.

Convars support types listed above.

```CS
[ConVar] private static int ExampleConVar;

```

Setting Convar

```CS
DevConsole.ExecuteCommand($"exampleconvar {value}")
```

You can output convar value

```cs
// Print ExampleConVar value to console
DevConsole.ExecuteCommand("exampleconvar")
```