using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataInfo;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour
{
    [SerializeField] private string filePath;
    public void Initialize()
    {
        filePath = Application.persistentDataPath + "/data.dat";
    }

    public void Save(GameDataObject gameData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);
        GameData data = new GameData();
        data.hp = gameData.hp;
        data.speed = gameData.speed;
        data.damage = gameData.damage;
        data.killCount = gameData.killCount;
        data.equipItem = gameData.equipItem;
        bf.Serialize(file, data);
        file.Close();
    }

    public GameData Load()
    {
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file);
            file.Close();
            return data;
        }
        else
        {
            return new GameData();
        }
    }
}
