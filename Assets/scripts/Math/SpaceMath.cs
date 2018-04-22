using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpaceMath
{
    /// <summary>
    /// Отдает границы мира, который попадает в камеру
    /// </summary>
    /// <param name="camera"></param>
    /// <returns></returns>
    public static Bounds GetOrthographicBounds(Camera camera)
    {
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * (Screen.width / (float)Screen.height), cameraHeight, 0));
        return bounds;
    }

    /// <summary>
    /// Возвращает количество ячеек, которые попадают в камеру
    /// </summary>
    public static Vector3Int GetCellCount(Camera camera, Vector2 cellSize)
    {
        var bounds = GetOrthographicBounds(camera);
        Vector3Int cellCount = new Vector3Int((int)Mathf.Ceil(bounds.max.x / cellSize.x), (int)Mathf.Ceil(bounds.max.y / cellSize.y), 0);

        return cellCount * 2;
    }




}
