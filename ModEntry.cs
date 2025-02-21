using System;
using System.IO;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace ChatLoggerMod
{
    public class ModEntry : Mod
    {
        private string logFilePath;

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.ReturnedToTitle += OnReturnedToTitle;
            helper.Events.Chat.ChatMessageReceived += OnChatMessageReceived;
        }

        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            string saveName = Game1.GetSaveGameName(); // Nome do save
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"); // Data e hora
            string fileName = $"{saveName}.{timestamp}.txt"; // Nome do arquivo
            logFilePath = Path.Combine(Helper.DirectoryPath, fileName);

            LogMessage("=== Início do registro do chat ===");
        }

        private void OnChatMessageReceived(object sender, ChatMessageEventArgs e)
        {
            string message = $"[{DateTime.Now:HH:mm:ss}] {e.Source}: {e.Message}";
            LogMessage(message);
        }

        private void OnReturnedToTitle(object sender, ReturnedToTitleEventArgs e)
        {
            LogMessage("=== Fim do registro do chat ===");
            logFilePath = null; // Reset no arquivo ao voltar para o menu
        }

        private void LogMessage(string message)
        {
            if (!string.IsNullOrEmpty(logFilePath))
            {
                File.AppendAllText(logFilePath, message + Environment.NewLine);
            }
        }
    }
}
