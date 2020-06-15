namespace DeveloperConsole
{
    public class Commands
    {
        [ConCommand("help")]
        [ConCommandDesc("Display Help Commands")]
        private static void Help()
        {
            DevConsole.LogMessage("help.cmd");
            DevConsole.LogMessage("help.cvar");
        }
        
        [ConCommand("help.cmd")]
        [ConCommandDesc("Display All Commands")]
        private static void HelpCmd()
        {
            foreach (var command in DevConsole.GetCommands())
            {
                DevConsole.LogMessage(command.ToString());
            }
        }
        
        [ConCommand("help.cvar")]
        [ConCommandDesc("Display All ConVars")]
        private static void HelpCVar()
        {
            foreach (var command in DevConsole.GetConVars())
            {
                DevConsole.LogMessage(command.ToString());
            }
        }
    }
}