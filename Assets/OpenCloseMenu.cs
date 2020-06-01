using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCloseMenu : MonoBehaviour
{
    bool isOpened = true;
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
        //foreach (Transform child in transform)
        //{
        //    child.gameObject.SetActive(true);
        //}
    }
}
