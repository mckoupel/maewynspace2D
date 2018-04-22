using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Настройки и ресурсы для проекта
/// </summary>
public class SpaceDatabase : ScriptableObject
{
    /// <summary>
    /// Список префабов со спрайтами планет
    /// </summary>
    [SerializeField]
    public List<GameObject> planetLits;

    /// <summary>
    /// Список со спрайтами планет
    /// </summary>
    [SerializeField]
    public GameObject textRaitPrefab;

    /// <summary>
    /// минимальное количество отображающихся ячеек
    /// </summary>
    [SerializeField]
    public Vector3Int minCellCount;

    /// <summary>
    /// максимальное количество отображающихся ячеек
    /// </summary>
    [SerializeField]
    public Vector3Int maxCellCount;

    /// <summary>
    /// Минимальное количество ображаемых планеты
    /// </summary>
    [SerializeField]
    public int minPlanetCount;

    /// <summary>
    /// Минимальное количество отображаемых планет
    /// </summary>
    [SerializeField]
    public int maxPlanetCount;

}
