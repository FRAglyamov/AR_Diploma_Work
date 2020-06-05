using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Catalogue : MonoBehaviour
{
    [SerializeField]
    private GameObject furnitureButton;

    private void Awake()
    {
        var furnitures = Resources.LoadAll<ScriptableObject>("Furniture");
        if (furnitures == null)
        {
            Debug.LogWarning("Lenght(furnitures) == null");
        }
        
        foreach (var item in furnitures)
        {
            Furniture f = (Furniture)item;
            furnitureButton = Instantiate(furnitureButton, transform);
            furnitureButton.GetComponent<Image>().sprite = f.Images[0];
            furnitureButton.GetComponentInChildren<TextMeshProUGUI>().text = f.Name;
        }

    }
}
