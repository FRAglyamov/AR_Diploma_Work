using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveScene : MonoBehaviour
{
    private GameObject[] furnitureArray;
    private string path;


    private void Start()
    {
        path = Application.persistentDataPath;
    }

    public void Save(string saveName = "save")
    {
        furnitureArray = GameObject.FindGameObjectsWithTag("Furniture");
        if (furnitureArray.Length == 0)
        {
            return;
        }
        //string json = JsonUtility.ToJson(furnitureArray);
        //debugText.text = json;
        //System.IO.File.WriteAllText(path + "/" + saveName + ".json", json);
        foreach (var f in furnitureArray)
        {
            FurnitureSave fs = new FurnitureSave { name = f.name, pos = f.transform.position, rot = f.transform.rotation };
            string json = JsonUtility.ToJson(fs);
            System.IO.File.WriteAllText(Application.persistentDataPath + "/" + saveName + ".json", json);
        }


    }

    public void Load(string saveName = "save")
    {
        string json = System.IO.File.ReadAllText(path + "/" + saveName + ".json");
        FurnitureSave[] fs = new FurnitureSave[] { };
        JsonUtility.FromJsonOverwrite(json, fs);
        foreach (var f in fs)
        {
            //Instantiate(f.name, f.pos, f.rot);
        }
    }
}

public class FurnitureSave
{
    public string name;
    public Vector3 pos;
    public Quaternion rot;
}
