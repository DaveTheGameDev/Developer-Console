# Developer Console

I made this system to handle executing commands via a console GUI from unity.


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
* char
* decimal
* double
* float
* int
* long
* sbyte
* short
* uint
* uint
* ulong
* ushort
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

## Unity types  
Right now only Color, Vector3 and GameObject are supported.
Soon all unity types will be supported.
		


# known Issues / Limitations

* Not all unity types are supported yet. 
* Docs for adding new types as parameters do not exist yet. To add new commands you can take a look at ConsoleHelper.cs

