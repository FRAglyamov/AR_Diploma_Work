using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadListOfSaves : MonoBehaviour
{
    [SerializeField]
    GameObject loadButton;
    private string path;

    private void Start()
    {
        path = Application.persistentDataPath;
    }

    private void OnEnable()
    {
        DeleteAllChildrens();
        string[] saveFiles = Directory.GetFiles(Application.persistentDataPath);
        foreach (var item in saveFiles)
        {
            Debug.Log(item);
            var tmpGO = Instantiate(loadButton, this.transform);
            tmpGO.name = item.Split('/').Last().Replace(".json", "");
            Debug.Log(tmpGO.name);
            tmpGO.GetComponentInChildren<TextMeshProUGUI>().text = tmpGO.name;
            tmpGO.GetComponent<Button>().onClick.AddListener(() => this.Load(tmpGO.name));
        }
    }


    public void Load(string saveName = "save")
    {
        string json = File.ReadAllText(path + "/" + saveName + ".json");
        Save save = new Save();
        JsonUtility.FromJsonOverwrite(json, save);
        foreach (var f in save.furnitureSave)
        {
            Debug.Log("Instantiate " + f.name);
            Instantiate(Resources.Load<Furniture>("Furniture/" + f.name).Model, f.pos, f.rot);
        }
    }

    private void DeleteAllChildrens()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
