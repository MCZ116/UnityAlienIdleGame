using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem 
{

    public static string savePath = Application.persistentDataPath;
    public static string saveName = "/savegame.nbn";

    public static void SaveGameData(GameManager gameManager, ResearchManager researchManager, PlanetManager planetManager, BuildingManager buildingManager)
    {

        using (var writer = new StreamWriter(savePath + saveName))
        {
            var formatter = new BinaryFormatter();
            var memoryStream = new MemoryStream();
            GameData gameData = new GameData(gameManager, researchManager, planetManager, buildingManager);
            formatter.Serialize(memoryStream, gameData);
            var dataWriter = Encryption.Encrypts(Convert.ToBase64String(memoryStream.ToArray()));
            writer.WriteLine(dataWriter);

        }

    }

    public static GameData LoadData()
    {
        string savePath = Application.persistentDataPath + saveName;
        if (File.Exists(savePath))
        {

            using (var reader = new StreamReader(savePath))
            {
                var formatter = new BinaryFormatter();
                var dataToRead = reader.ReadToEnd();
                var memoryStream = new MemoryStream(Convert.FromBase64String(Encryption.Dencrypts(dataToRead)));

                GameData gameData = formatter.Deserialize(memoryStream) as GameData;

                return gameData;


            }

        }
        else
        {
            Debug.LogError("No save file in "+savePath);
            return null;
        }
    }


}
