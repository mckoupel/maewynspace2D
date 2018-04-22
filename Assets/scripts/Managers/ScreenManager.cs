using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Управляет инстансингом объектов в сцену
/// </summary>
public class ScreenManager : MonoBehaviour
{
    public SpaceGeneral spaceGeneral;

    /// <summary>
    /// Канва, родительская для планеты
    /// </summary>
    public RectTransform spaceCanvas;
    public Text zoomText;
    public Text raitText;

    /// <summary>
    /// Список планет на рендер
    /// </summary>
    private List<Planet> planetOnRenderList;
    /// <summary>
    /// Список планет, которые сейчас отображаются
    /// </summary>
    private List<Planet> planetOnScreenList;

    private void Start()
    {
        planetOnRenderList = new List<Planet>();
        planetOnScreenList = new List<Planet>();
    }

    void LateUpdate ()
    {
        zoomText.text = "planets: " + (spaceGeneral.cellCount.x + 1).ToString() + " x " + (spaceGeneral.cellCount.y + 1).ToString();
        raitText.text = "rait: " + spaceGeneral.map.ShipRait.ToString();

        // очищаем хранилище планет от планет
        for (int i = 0; i < planetOnScreenList.Count; i++)
        {
            var planet = planetOnScreenList[i];
            Destroy(planet.go);
        }

        planetOnScreenList = new List<Planet>();


        // проходим по всем планетам из полученного списка
        for (int i = 0; i < planetOnRenderList.Count; i++)
        {
            var planet = planetOnRenderList[i];

            // добавляем к планете GameObject
            planet.go = Instantiate<GameObject>(spaceGeneral.spaceDB.planetLits[planet.variant]);
            var rectTransform = planet.go.GetComponent<RectTransform>();
          
            rectTransform.position = spaceGeneral.grid.GetCellCenterWorld(planet.index - spaceGeneral.cellCurrent);
            rectTransform.SetParent(spaceCanvas);

            // вешаем необходимые компоненты
            planet.go.name += "planet " + planet.index;
            var scaleController = planet.go.AddComponent<ScaleController>();
            scaleController.maxScale = 150;
            scaleController.scaleFactor = 10;
            planet.go.transform.localScale = scaleController.CalculateScale();

            // добавляем текст с рейтом
            GameObject text = Instantiate<GameObject>(spaceGeneral.spaceDB.textRaitPrefab);
            var textRectTrasform = text.GetComponent<RectTransform>();
            textRectTrasform.position = rectTransform.position + new Vector3(0, 0.3f, 0);
            textRectTrasform.SetParent(rectTransform);
            textRectTrasform.localScale = Vector3.one;//rectTransform.localScale;
            text.GetComponent<Text>().text = planet.rait.ToString();

            planetOnScreenList.Add(planet);
        }
	}

    /// <summary>
    /// Получает из PlanetManager список планет для рендера
    /// </summary>
    /// <param name="planetList"></param>
    public void GetPlanets(List<Planet> planetList)
    {
        planetOnRenderList = planetList;
    }

}
