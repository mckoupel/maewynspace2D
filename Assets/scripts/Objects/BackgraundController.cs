using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Управление скейлом и смещением фона через UV
/// </summary>
public class BackgraundController : MonoBehaviour
{
    private Material mat;
    private RectTransform rectTransform;

    /// <summary>
    /// Оффсет, с которого мы начинаем
    /// </summary>
    public Vector2 currentOffset;

    /// <summary>
    /// Скейл, с которого мы начинаем
    /// </summary>
    public float currentScale;

	void Start ()
    {
        mat = this.GetComponent<Image>().material;
        rectTransform = this.GetComponent<RectTransform>();

        currentOffset = new Vector2(mat.GetFloat("_UVoffsetX"), mat.GetFloat("_UVoffsetY"));
        currentScale = mat.GetFloat("_UVscale");
    }
     
    /// <summary>
    /// Растягиваем бэкграунд под размер камеры
    /// </summary>
	void LateUpdate ()
    {
        Bounds camBounds = SpaceMath.GetOrthographicBounds(Camera.main);
        rectTransform.sizeDelta = new Vector2((camBounds.max.x * 2) * 50f, (camBounds.max.y * 2) * 50);
    }

    /// <summary>
    /// Двигаем UV
    /// </summary>
    public void MoveUV(Vector2 offset)
    {
        currentOffset += offset;
        //на всякий случай, чтобы разряды когда-нибудь не переполнились мы отсекаем целую часть
        currentOffset = new Vector2(currentOffset.x - (int)currentOffset.x, currentOffset.y - (int)currentOffset.y);

        mat.SetFloat("_UVoffsetX", currentOffset.x);
        mat.SetFloat("_UVoffsetY", currentOffset.y);
    }

    /// <summary>
    /// Скейлим UV
    /// </summary>
    public void ScaleUV(float scale)
    {
        currentScale = scale;
        mat.SetFloat("_UVscale", currentScale);
    }
}
