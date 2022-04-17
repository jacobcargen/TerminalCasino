using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace TerminalCasino
{
    [Serializable]
    public class SaveData
    {
        public string name;
        public int money;
        public bool isUser;

        /// <summary>
        /// Default
        /// </summary>
        public SaveData() { }

        /// <summary>
        /// Send in a player(idealy the user) to make a saveable file. 
        /// You can then Use Saving.SaveProgress(yourNewSaveableData)
        /// </summary>
        /// <param name="playerToSave"></param>
        public SaveData(Player playerToSave)
        {
            this.name = playerToSave.name;
            this.money = playerToSave.money;
            this.isUser = playerToSave.isUser;
        }
    }
    public static class Saving
    {
        private static bool isDebug = false; // Set to true to enable debug for the saving
        // Save file folder
        private static string savePath = AppDomain.CurrentDomain.BaseDirectory + @"\saves";
        // Name of the save file
        private static string fileName = @"\tcasino.save";


        /// <summary>
        /// SAVING: Call this to save the players progress. 
        /// DEFAULTING: Set isDefaulting to true, to reset the players progress. 
        /// You must also send a SaveData contructor call with a empty player.
        /// - new SaveData(new Player())
        /// </summary>
        /// <param name="saveData"></param>
        public static void SaveProgress(SaveData saveData, bool isDefaulting = false)
        {
            // If save debugging is enabled...
            if (isDebug)
            {
                // Tell user their progress is resetting
                if (isDefaulting) Tools.TellUser("Resetting progress...");
                // Tell the user their current progress is saving
                else Tools.TellUser("Saving progress..."); 
            }

            // Makes sure the save path exists
            Tools.CreateDirIfNone(savePath);

            // Create a filestream
            FileStream fileStream = new FileStream(savePath + fileName, FileMode.Create);
            // Save the progress to a file
            new BinaryFormatter().Serialize(fileStream, saveData);
            // Close the filestream
            fileStream.Close();
            // Let the user know their progress has saved
            if (isDebug) Tools.TellUser("Progress saved.");
        }

        /// <summary>
        /// Load an existing progress save for the user. It will deafult if one is not found.
        /// </summary>
        /// <returns>Returns the saved data</returns>
        public static SaveData LoadProgress()
        {
            // If is debugging .. Tell the user their progress is loading
            if (isDebug) Tools.TellUser("Loading progress...");

            // Makes sure the save path exists
            Tools.CreateDirIfNone(savePath);

            // If a file does exist ..
            if (File.Exists(savePath + fileName)) 
            {
                FileStream fileStream = new FileStream(savePath + fileName, FileMode.Open); // Open a new filestream
                SaveData saveData = new BinaryFormatter().Deserialize(fileStream) as SaveData; // Retrieve data from the saved file in the form of SaveData class
                fileStream.Close(); // Close the filestream
                if (isDebug) Tools.TellUser("Progress loaded.");
                return saveData; // Return the saved data
            }
            // If a file does not exist .. create one
            else
            {
                // If is debugging .. Tell user a save file was not found
                if (isDebug) Tools.TellUser("Progress save file not found in '" + savePath + "'");
                // Default the save
                SaveProgress(new SaveData(new Player()), true);
                // Calls itself now that a file exists.
                // IF by any means the file cannot be found after creation, this recursion might cause a crash!
                return LoadProgress();
            }
        }
        
        /// <summary>
        /// Will delete an existing save file WITHOUT creating a new one.
        /// </summary>
        public static void DeleteSave()
        {
            // Trying to delete save
            if (isDebug) Tools.TellUser("Deleting save...");
            // If a file is found...
            if (File.Exists(savePath + fileName))
            {
                // Delete the save
                File.Delete(savePath + fileName);
                if (isDebug) Tools.TellUser("File deleted.");
            }
        }
    }
}
