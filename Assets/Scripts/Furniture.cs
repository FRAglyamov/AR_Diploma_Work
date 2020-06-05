using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Furniture", menuName = "Furniture", order = 1)]
public class Furniture : ScriptableObject
{
    public int Id;
    public string Name; // Наименование товара
    public string Description; // Описание
    public float Price; // Цена
    public string ProductType; // Тип продукта (стол, стул, кресло и т.п.)
    public List<Sprite> Images; // Изображения
    public GameObject Model; // 3D Модель - Prefab
}
