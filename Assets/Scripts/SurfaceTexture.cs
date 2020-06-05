using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SurfaceTexture : MonoBehaviour
{
    [SerializeField]
    ARPlaneManager aRPlaneManager;
    GameObject arPlane;
    private void Awake()
    {
        arPlane = aRPlaneManager.planePrefab;
    }

    /// <summary>
    /// Замена материала Prefab'а поверхности, а также всех текущих
    /// </summary>
    /// <param name="mat"></param>
    public void TextureSelect(Material mat)
    {
        arPlane.GetComponent<MeshRenderer>().material = mat;
        foreach (var item in aRPlaneManager.trackables)
        {
            item.GetComponent<MeshRenderer>().material = mat;
        }
    }
}
