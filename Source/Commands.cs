using AstrayEngine;

namespace DeveloperConsole
{
    public class Commands
    {
        [ConCommand("help")]
        [ConCommandDesc("Display Help Commands")]
        private static void Help()
        {
            foreach (var command in DevConsole.GetCommands()) 
                DevConsole.LogMessage(command.ToString());
            
            foreach (var command in DevConsole.GetConVars()) 
                DevConsole.LogMessage(command.ToString());
        }
        
        [ConCommand("?")]
        [ConCommandDesc("Display Help Commands")]
        private static void Help(string arg)
        {
            foreach (var command in DevConsole.GetCommands())
            {
                if (command.Alias.Equals(arg)) 
                    DevConsole.LogMessage(command.ToString());
            }
        }
    }
}