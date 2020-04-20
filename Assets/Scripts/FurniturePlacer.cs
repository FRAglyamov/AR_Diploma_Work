using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FurniturePlacer : MonoBehaviour
{
    public Transform placementIndicator;
    public GameObject selectionUI;

    private List<GameObject> furniture = new List<GameObject>();
    private GameObject curSelected;
    private Camera cam;

    void Start ()
    {
        cam = Camera.main;
        selectionUI.SetActive(false);
    }

    void Update ()
    {
        // Первый фрейм, когда касаемся экрана
        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began 
            && !EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId)) // и не касаемся UI
        {
            // Создаем луч от того места, где мы касаемся на экране
            Ray ray = cam.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;

            // Кидаем raycast
            if(Physics.Raycast(ray, out hit))
            {
                // Смотрим, попали ли мы во что-то
                if(hit.collider.gameObject != null && furniture.Contains(hit.collider.gameObject))
                {
                    // Выбираем объект, до которого дотронулись
                    if(curSelected != null && hit.collider.gameObject != curSelected)
                        Select(hit.collider.gameObject);
                    else if(curSelected == null)
                        Select(hit.collider.gameObject);
                }
            }
            else
                Deselect();
        }

        if(curSelected != null && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved)
            MoveSelected();
    }

    void MoveSelected ()
    {
        Vector3 curPos = cam.ScreenToViewportPoint(Input.touches[0].position);
        Vector3 lastPos = cam.ScreenToViewportPoint(Input.touches[0].position - Input.touches[0].deltaPosition);

        Vector3 touchDir = curPos - lastPos;

        Vector3 camRight = cam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        curSelected.transform.position += (camRight * touchDir.x + camForward * touchDir.y);
    }

    /// <summary>
    /// Вызывается при выборе объекта мебели
    /// </summary>
    /// <param name="selected"></param>
    void Select (GameObject selected)
    {
        if(curSelected != null)
            ToggleSelectionVisual(curSelected, false);

        curSelected = selected;
        ToggleSelectionVisual(curSelected, true);
        selectionUI.SetActive(true);
    }

    /// <summary>
    /// Вызывается при снятии выбора с объекта мебели
    /// </summary>
    void Deselect ()
    {
        if(curSelected != null)
            ToggleSelectionVisual(curSelected, false);

        curSelected = null;
        selectionUI.SetActive(false);
    }

    /// <summary>
    /// Вызывается при выборе/снятии выбора объекта мебели
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="toggle"></param>
    void ToggleSelectionVisual (GameObject obj, bool toggle)
    {
        obj.transform.Find("Selected").gameObject.SetActive(toggle);
    }

    /// <summary>
    /// Вызывается при нажатии кнопки мебели - создаёт новый объект мебели
    /// </summary>
    /// <param name="prefab"></param>
    public void PlaceFurniture (GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, placementIndicator.position, Quaternion.identity);
        furniture.Add(obj);
        Select(obj);
    }

    public void ScaleSelected (float rate)
    {
        curSelected.transform.localScale += Vector3.one * rate;
    }

    public void RotateSelected (float rate)
    {
        curSelected.transform.eulerAngles += Vector3.up * rate;
    }

    public void SetColor (Image buttonImage)
    {
        MeshRenderer[] meshRenderers = curSelected.transform.GetChild(0).GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer mr in meshRenderers)
        {
            if(mr.gameObject.name == "Selected")
                continue;

            mr.material.color = buttonImage.color;
        }
    }

    public void DeleteSelected ()
    {
        furniture.Remove(curSelected);
        Destroy(curSelected);
        Deselect();
    }
}