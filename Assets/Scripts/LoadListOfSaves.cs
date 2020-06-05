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

    [SerializeField]
    private FurniturePlacer furniturePlacer;

    private void Start()
    {
        path = Application.persistentDataPath;
        if (furniturePlacer == null)
        {
            furniturePlacer = Camera.main.GetComponent<FurniturePlacer>();
        }
    }

    private void OnEnable()
    {
        DeleteAllChildrens();
        string[] saveFiles = Directory.GetFiles(Application.persistentDataPath, "*.json");
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

    /// <summary>
    /// Считывание и загрузка сохранения из JSON файла
    /// </summary>
    /// <param name="saveName"></param>
    public void Load(string saveName = "save")
    {
        string json = File.ReadAllText(path + "/" + saveName + ".json");
        Save save = new Save();
        JsonUtility.FromJsonOverwrite(json, save);

        furniturePlacer.DeleteAll();
        foreach (var f in save.furnitureSave)
        {
            Debug.Log("Instantiate " + f.name);
            furniturePlacer.PlaceFurniture(Resources.Load<Furniture>("Furniture/" + f.name).Model, f.pos, f.rot);
        }
    }
    /// <summary>
    /// Очистка списка в UI, путём удаления потомков
    /// </summary>
    private void DeleteAllChildrens()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
