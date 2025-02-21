using System; using System.IO; using StardewModdingAPI; using StardewModdingAPI.Events; using StardewValley;

namespace ChatLoggerMod { public class ModEntry : Mod { private string logFilePath;

public override void Entry(IModHelper helper)
    {
        helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        helper.Events.GameLoop.ReturnedToTitle += OnReturnedToTitle;
        helper.Events.Multiplayer.ChatMessageReceived += OnChatMessageReceived;
    }

    private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
    {
        string saveName = Game1.GetSaveGameName();
        string saveFolder = Path.Combine(Constants.SavesPath, saveName);
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        logFilePath = Path.Combine(saveFolder, $"ChatLog_{timestamp}.txt");
        File.AppendAllText(logFilePath, $"Session started at {timestamp}\n\n");
    }

    private void OnChatMessageReceived(object sender, ChatMessageEventArgs e)
    {
        string message = $"[{DateTime.Now:HH:mm:ss}] {e.User}: {e.Message}\n";
        File.AppendAllText(logFilePath, message);
    }

    private void OnReturnedToTitle(object sender, ReturnedToTitleEventArgs e)
    {
        if (!string.IsNullOrEmpty(logFilePath))
        {
            File.AppendAllText(logFilePath, "\nSession ended.\n");
            logFilePath = null;
        }
    }
}

}

