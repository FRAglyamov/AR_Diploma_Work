using UnityEngine;

public class UILinkHolder : MonoBehaviour
{
    [SerializeField] private GameObject _appInterface;
    [SerializeField] private GameObject _catalogue;
    [SerializeField] private GameObject _furnitureInfo;

    public GameObject GetAppInterfaceGO()
    {
        return _appInterface;
    }
    public GameObject GetCatalogueGO()
    {
        return _catalogue;
    }
    public GameObject GetFurnitureInfoGO()
    {
        return _furnitureInfo;
    }
}
