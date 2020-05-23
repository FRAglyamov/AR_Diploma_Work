using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteFurniture : MonoBehaviour
{
    public void DeleteAll()
    {
        GameObject[] m_furnitureList = GameObject.FindGameObjectsWithTag("Furniture");
        foreach (GameObject furniture in m_furnitureList)
        {
            Destroy(furniture);
        }
    }
}
