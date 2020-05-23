using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Включение/выключение показа поверхностей
/// </summary>
[RequireComponent(typeof(ARPlaneManager))]
public class PlaneDetectionToggle : MonoBehaviour
{
    ARPlaneManager m_ARPlaneManager;

    void Awake()
    {
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
    }

    public void TogglePlaneDetection()
    {
        m_ARPlaneManager.enabled = !m_ARPlaneManager.enabled;
        if (m_ARPlaneManager.enabled)
        {
            SetAllPlanesActive(true);
        }
        else
        {
            SetAllPlanesActive(false);
        }
    }

    /// <summary>
    /// Проходимся по всем поверхностям и активируем или деактивируем их GameObject'ы
    /// </summary>
    /// <param name="value">Для GameObject'а каждой поверхности применяем SetActive с этим value</param>
    void SetAllPlanesActive(bool value)
    {
        foreach (var plane in m_ARPlaneManager.trackables)
            plane.gameObject.SetActive(value);
    }
}
