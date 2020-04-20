using Barracuda;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TestCameraImage : MonoBehaviour
{
    [SerializeField]
    ARCameraManager m_CameraManager;

    public ARCameraManager cameraManager
    {
        get { return m_CameraManager; }
        set { m_CameraManager = value; }
    }

    [SerializeField]
    RawImage m_RawImage;

    // ��� ����������� �� UI
    public RawImage rawImage
    {
        get { return m_RawImage; }
        set { m_RawImage = value; }
    }

    [SerializeField]
    Text m_ImageInfo;

    [SerializeField]
    Text m_NNInfo;

    // ��� ����������� �� UI
    public Text imageInfo
    {
        get { return m_ImageInfo; }
        set { m_ImageInfo = value; }
    }

    void OnEnable()
    {
        if (m_CameraManager != null)
        {
            m_CameraManager.frameReceived += OnCameraFrameReceived;
        }
    }

    void OnDisable()
    {
        if (m_CameraManager != null)
        {
            m_CameraManager.frameReceived -= OnCameraFrameReceived;
        }
    }

    unsafe void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        // �������� ����� ��������� ����������� � ������
        XRCameraImage image;
        if (!cameraManager.TryGetLatestImage(out image))
        {
            return;
        }

        // ����� ���������� �� �����������
        m_ImageInfo.text = string.Format(
            "Image info:\n\twidth: {0}\n\theight: {1}\n\tplaneCount: {2}\n\ttimestamp: {3}\n\tformat: {4}",
            image.width, image.height, image.planeCount, image.timestamp, image.format);


        // ������� �������� XRCameraImage, �� ����� �������� ������ � ��������� ��������� ������� �� �����������
        // XRCameraImage.GetPlane ������������ "������" ������ � ���� ������
        // ��� ����� ����� �������� ��������� ������������� ������
        // (� ���� ����������� ����������� � ������ � �������� RGBA � ������������ �� �� ������)
        // NOTE: � ����� ������ ����� �������� �� ���� ����� ��������

        // �������� RGBA ������
        // ��. XRCameraImage.FormatSupported ��� ������� ������ �������������� ��������
        var format = TextureFormat.RGBA32;

        if (m_Texture == null || m_Texture.width != image.width || m_Texture.height != image.height)
        {
            m_Texture = new Texture2D(image.width, image.height, format, false);
        }

        // ��������������� ����������� � ������, ������������� ����������� �� ��� Y
        // �� ����� ����� �������� ��������� �������������, �� �� ������ ������ �����������
        var conversionParams = new XRCameraImageConversionParams(image, format, CameraImageTransformation.MirrorY);

        // Texture2D ��������� ���������� � ������ raw texture
        // ��� ��������� ��� ��������� �������������� �� �����, �� ����� �����
        var rawTextureData = m_Texture.GetRawTextureData<byte>();
        try
        {
            image.Convert(conversionParams, new IntPtr(rawTextureData.GetUnsafePtr()), rawTextureData.Length);
        }
        finally
        {
            // ����������� �� XRCameraImage ����� ����, ��� ��������� � ���, ����� �������� ������ ��������
            image.Dispose();
        }

        // ��������� ����������� ������ �������� � ����� ��������
        m_Texture.Apply();

        // ������������� RawImage ��������, ����� ����� ���� ���������������
        m_RawImage.texture = m_Texture;

        //RunNN();
    }

    private Texture2D m_Texture;



    public NNModel modelSource;
    private Model model;
    private IWorker worker;
    private Tensor output;
    private string[] labels = new string[] { "bed", "chair", "sofa", "swivelchair" };
    private bool isWorking = false;
    private void Start()
    {
        model = ModelLoader.Load(modelSource);
        //worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);

        var inputNames = model.inputs;   // query model inputs
        var outputNames = model.outputs; // query model outputs
        m_NNInfo.text += "Input: ";
        foreach (var item in inputNames)
        {
            m_NNInfo.text += item.name + ", ";
        }
        m_NNInfo.text += "\n";
        m_NNInfo.text += "Output: " + outputNames.ToString() + "\n";
        foreach (var item in outputNames)
        {
            m_NNInfo.text += item + ", ";
        }
        m_NNInfo.text += "\n";
        foreach (var layer in model.layers)
            m_NNInfo.text += "layer.name: " + layer.name + " layer.type: " + layer.type + "\n";

    }

    //public void RunNN()
    //{
    //    StopCoroutine(NNWorker());
    //    StartCoroutine(NNWorker());
    //}

    public void RunNN()
    {
        //yield return new WaitForEndOfFrame();
        if (this.isWorking)
        {
            return;
        }
        this.isWorking = true;

        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
        //yield return new WaitForSeconds(0.1f);
        //m_NNInfo.text = "RunNN" + Time.time;

        var inputs = new Dictionary<string, Tensor>();
        //m_NNInfo.text = "Created inputs" + Time.time;
        inputs["actual_input_1"] = new Tensor(Resize(m_Texture, 32, 32));
        //m_NNInfo.text = "Inputs added" + Time.time;
        worker.Execute(inputs);

        //var coroutine = worker.ExecuteAsync(inputs);
        //yield return StartCoroutine(coroutine);

        //m_NNInfo.text = "Worker Execute" + Time.time;
        output = worker.PeekOutput("output1");
        //m_NNInfo.text = "Output Peekout" + Time.time;
        //m_NNInfo.text = "Neural Network Info: " + O.ToString() + Time.time;
        
        var map = new List<KeyValuePair<string, float>>();
        for (int i = 0; i < labels.Length; i++)
        {
            map.Add(new KeyValuePair<string, float>(labels[i], output[i]));
        }
        m_NNInfo.text = "Neural Network Info: \n";
        //m_NNInfo.text += "Inputs: " + inputs["actual_input_1"].ToString() + "\n";
        //m_NNInfo.text += "map: " + map.ToString() + "\n" + Time.time.ToString();
        string maxValueKey = map[0].Key;
        float maxValue = map[0].Value;
        foreach (var item in map)
        {
            m_NNInfo.text += "item: " + item.Key + " value: " + item.Value + "\n";
            if(item.Value > maxValue)
            {
                maxValue = item.Value;
                maxValueKey = item.Key;
            }
        }
        m_NNInfo.text += "It's a [" + maxValueKey + "]\n";
        //O?.Dispose();
        worker.Dispose();
        this.isWorking = false;


    }
    public static Texture2D Resize(Texture2D source, int newWidth, int newHeight)
    {
        source.filterMode = FilterMode.Point;
        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        rt.filterMode = FilterMode.Point;
        RenderTexture.active = rt;
        Graphics.Blit(source, rt);
        Texture2D nTex = new Texture2D(newWidth, newHeight);
        nTex.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        nTex.Apply();
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);
        return nTex;
    }
}
