using UnityEngine;

public class TakeScreenshot : MonoBehaviour
{
    Camera myCamera;
    private bool takeScreenshotOnNextFrame;
    void Start()
    {
        myCamera = GetComponent<Camera>();
    }

    private void OnPostRender()
    {
        if (takeScreenshotOnNextFrame)
        {
            takeScreenshotOnNextFrame = false;

            RenderTexture renderTexture = myCamera.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = renderResult.EncodeToPNG();
            string filename = "/CameraScreenshot" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            //string fileLocation = Path.Combine(Application.persistentDataPath, filename);
            //File.WriteAllBytes(fileLocation, byteArray);
            NativeGallery.SaveImageToGallery(byteArray, "AR Furniture", filename);

            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;
        }
    }

    public void Take()
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 16);
        takeScreenshotOnNextFrame = true;
    }
}
