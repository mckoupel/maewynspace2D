using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс, отвечающий за планету
/// </summary>
[System.Serializable]
public class Planet
{
    /// <summary>
    /// Рейтинг планеты
    /// </summary>
    [SerializeField]
    public int rait;

    /// <summary>
    /// Индекс планеты в мире
    /// </summary>
    [SerializeField]
    public Vector3Int index;

    /// <summary>
    /// Вариант спрайта планеты
    /// </summary>
    [SerializeField]
    public int variant;

    /// <summary>
    /// Объект планеты в сцене (задается после инициализации)
    /// </summary>
    [System.NonSerialized]
    public GameObject go;

    
    public Planet(Vector3Int index,   int variant, int rait)
    {
        this.index = index;
        this.variant = variant;
        this.rait = rait;
    }

}
