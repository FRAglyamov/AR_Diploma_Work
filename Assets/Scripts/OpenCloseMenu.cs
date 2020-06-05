using UnityEngine;
using UnityEngine.UI;

public class OpenCloseMenu : MonoBehaviour
{
    bool isOpened = false;

    /// <summary>
    /// Открытие и закрытие выпадающего меню
    /// </summary>
    /// <param name="isFirstElseLast"></param>
    public void ToggleMenu(bool isFirstElseLast)
    {
        isOpened = !isOpened;
        if (isFirstElseLast)
        {
            for (int i = 1; i < transform.childCount; ++i)
            {
                transform.GetChild(i).gameObject.SetActive(isOpened);
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount - 1; ++i)
            {
                transform.GetChild(i).gameObject.SetActive(isOpened);
            }
        }
        GetComponent<Image>().enabled = isOpened;
    }
}
