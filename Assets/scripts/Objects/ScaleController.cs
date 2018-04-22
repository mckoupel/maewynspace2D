using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleController : MonoBehaviour
{
    /// <summary>
    /// Минимальный размер объекта
    /// </summary>
    public float minScale = 1;

    /// <summary>
    /// Максимальный размер объекта
    /// </summary>
    public float maxScale = 100;
    
    /// <summary>
    /// Модификатор изменения размера
    /// </summary>
    public float scaleFactor = 15;

    /// <summary>
    /// ну это и есть rectTransform xD
    /// </summary>
    public RectTransform rectTransform;

    private void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();
    }

    void LateUpdate ()
    {
        rectTransform.localScale = CalculateScale();
    }

    /// <summary>
    /// Высчитывание размера объекта относительно камеры
    /// </summary>
    /// <returns></returns>
    public Vector3 CalculateScale()
    {
        var s_scale = Mathf.Clamp(Camera.main.orthographicSize / scaleFactor, minScale, maxScale);
        return new Vector3(s_scale, s_scale, s_scale);
    }
}
