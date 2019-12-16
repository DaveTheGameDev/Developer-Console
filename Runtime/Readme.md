# Developer Console

I made this system to handle executing commands for C# games.
This project has been setup as a unity package but can be used in any C# project.	

If you use this in your project hit me up on [twitter](https://twitter.com/ReignOfDave) or [linked in](https://www.linkedin.com/in/david-conway-gamedev/).

If you find a bug or have a suggestion feel free to open an issue or a create a pull request
## Example Usage

Commands must be static methods. The reason i designed it to not support non static methods is because i felt as though it would lead to spaghetti code.

With static methods you can have your commands self contained in a single static class that accesses managers.

```cs
// Commands can have multiple command aliases
[ConsoleCommand("clear", description: "clears the console window")]
[ConsoleCommand("cls", "clears the console window")]
[ConsoleUsage("Explain how to use this command and what it does here")]
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
[ConsoleCommand("add", "clears the console window")]
[ConsoleUsage("add 1 2")]
private static void Add(int a, int b)
{
    ConsoleSystem.Write((a + b).ToString());
}
```

# known Issues / Limitations
* Docs for adding new types as parameters do not exist yet. To add new commands you can take a look at ConsoleHelper.cs
