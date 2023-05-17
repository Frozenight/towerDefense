using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private string sessionDataFileName = "session.json";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public SessionData LoadSessionData()
    {
        return Load<SessionData>(sessionDataFileName);
    }

    public void SaveSessionData(SessionData data)
    {
        Save(data, sessionDataFileName);
    }

    public GameData LoadGameData()
    {
        return Load<GameData>(dataFileName);
    }

    public void SaveGameData(GameData data)
    {
        Save(data, dataFileName);
    }

    private T Load<T>(string fileName)
    {
        string fullPath = Path.Combine(dataDirPath, fileName);
        T loadedData = default(T);

        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<T>(dataToLoad);

            }catch (Exception e)
            {
                Debug.LogError("Error  occured when trying to load data from file:" + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save<T>(T data, string fileName)
    {
        string fullPath = Path.Combine(dataDirPath, fileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }catch (Exception e)
        {
            Debug.LogError("Error occured when trying  to save data to file:" + fullPath + "\n" + e);
        }
    }


}
