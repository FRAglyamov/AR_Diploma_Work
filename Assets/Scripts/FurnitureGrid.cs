using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureGrid : MonoBehaviour
{
    private Furniture selectedFurniture;
    private GameObject furnitureInfo;
    public void SelectFurnitureGrid()
    {
        selectedFurniture = Resources.Load<ScriptableObject>("Furniture/" + GetComponentInChildren<TextMeshProUGUI>().text) as Furniture;
        furnitureInfo = GameObject.FindGameObjectWithTag("UIHolder").GetComponent<UILinkHolder>().GetFurnitureInfoGO();

        furnitureInfo.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = 
            "Name: " + selectedFurniture.Name + "\n" + 
            "Price: " + selectedFurniture.Price + "\n" + 
            "Description: " + selectedFurniture.Description;
        furnitureInfo.transform.GetChild(1).GetComponent<Image>().sprite = selectedFurniture.Images[0];

        if (selectedFurniture.Model != null)
        {
            Button watchARButton = furnitureInfo.transform.GetChild(2).GetComponent<Button>();
            FurniturePlacer furniturePlacer = Camera.main.GetComponent<FurniturePlacer>();
            watchARButton.onClick.RemoveAllListeners();
            watchARButton.onClick.AddListener(() => furniturePlacer.PlaceFurniture(selectedFurniture.Model));
        }
            
        else
        {
            Debug.LogWarning("Missing " + selectedFurniture.name + " model!");
            // Загрузка модели (нет сервера, поэтому пока нереализовано)
        }

        furnitureInfo.SetActive(true);
        transform.parent.parent.gameObject.SetActive(false);
    }
}
