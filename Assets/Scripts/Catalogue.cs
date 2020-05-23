using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Catalogue : MonoBehaviour
{
    [SerializeField]
    private GameObject furnitureButton;
    public GameObject furnitureInfo; // Для ссылки из FurnitureGrid
    private void Awake()
    {
        var furnitures = Resources.LoadAll<ScriptableObject>("Furniture");
        if (furnitures == null)
        {
            Debug.Log("Lenght(furnitures) == null");
        }
        else
        {
            Debug.Log("Lenght(furnitures) " + furnitures.Length);
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
