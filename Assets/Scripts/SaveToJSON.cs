using TMPro;
using UnityEngine;

public class SaveToJSON : MonoBehaviour
{
    private GameObject[] furnitureArray;
    private string path;

    private void Start()
    {
        path = Application.persistentDataPath;
    }

    public void GetInputTextAndSave(TMP_InputField inputText)
    {
        Save(inputText.text);
    }
    /// <summary>
    /// Сохранение в JSON файл текущей мебели на сцене, её позиции и поворота
    /// </summary>
    /// <param name="saveName"></param>
    public void Save(string saveName = "save")
    {
        furnitureArray = GameObject.FindGameObjectsWithTag("Furniture");
        if (furnitureArray.Length == 0)
        {
            Debug.LogWarning("Furniture array lenght == 0");
            return;
        }

        string json = "";
        Save save = new Save();
        save.furnitureSave = new FurnitureSave[furnitureArray.Length];
        int k = 0;

        foreach (var f in furnitureArray)
        {
            // Debug.Log("Furniture array contains: " + f.name + " - " + f.name.Split('(')[0]);
            save.furnitureSave[k] = new FurnitureSave { name = f.name.Split('(')[0], pos = f.transform.position, rot = f.transform.rotation };
            k++;
        }

        json = JsonUtility.ToJson(save);
        // Debug.Log("Final JSON: " + json);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/" + saveName + ".json", json);
    }
}
