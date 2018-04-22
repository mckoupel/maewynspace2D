using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Считает общие для других скриптов переменные 
/// </summary>
public class SpaceGeneral : MonoBehaviour
{
    /// <summary>
    /// SO с ресурсами и параметрами
    /// </summary>
    public SpaceDatabase spaceDB;

    /// <summary>
    /// Класс, в котором на основе шума выдаются числа
    /// </summary>
    public ISpaceMap map;

    /// <summary>
    /// Камера
    /// </summary>
    public CameraManager cameraManager;

    /// <summary>
    /// Сетка, к которой привязываются планеты
    /// </summary>
    public Grid grid;

    /// <summary>
    /// количество ячеек, которое помещается в камеру
    /// </summary>
    public Vector3Int cellCount;
    /// <summary>
    /// текущаяя ячейка кормабля
    /// </summary>
    public Vector3Int cellCurrent;

    public void Start()
    {
        map = new SpaceNoise(); //LoadFromJson("resources/maps/map1");
    }

    void Update ()
    {
        cellCount = SpaceMath.GetCellCount(cameraManager.camera, grid.cellSize);
    }

    /// <summary>
    /// Изменение ячейки, в которой находится корабль
    /// </summary>
    public void GoToNeibCell(Vector3Int cellOffset)
    {
        cellCurrent += cellOffset;
    }

    /// <summary>
    /// Сохранение в JSON по указанному пути
    /// </summary>
    public void SaveToJson(string name)
    {
        var savedMap = ((SpaceNoise)map).SaveToMap(new Vector3Int(50, 50, 0));
        var jsonData = JsonUtility.ToJson(savedMap);

        string path = Application.dataPath + "/" + name + ".json";
        Debug.Log(path);

        System.IO.File.WriteAllText(path, jsonData);

        #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
        #endif
    }

    /// <summary>
    /// Загрузка из JSON по указанному пути
    /// </summary>
    public SpaceMap LoadFromJson(string name)
    {
        string path = Application.dataPath + "/" + name + ".json";//"maps/" + name;
        string filetext = string.Empty;
        string line;

        System.IO.StreamReader theReader = new System.IO.StreamReader(path, System.Text.Encoding.Default);
        using (theReader)
        {
            do
            {
                line = theReader.ReadLine();

                if (line != null)
                {
                    filetext += line;
                }
            }
            while (line != null);
            theReader.Close();
        }

        SpaceMap loadedMap = JsonUtility.FromJson<SpaceMap>(filetext);
        return loadedMap;
    }
}
