using System;
using UnityEngine;

[Serializable]
public class FurnitureSave
{
    public string name;
    public Vector3 pos;
    public Quaternion rot;
}

[Serializable]
public class Save
{
    public FurnitureSave[] furnitureSave;
}
