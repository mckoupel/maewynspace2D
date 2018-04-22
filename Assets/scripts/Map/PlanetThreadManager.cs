using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;
using System.Linq;

using Amib.Threading;

/// <summary>
/// Осуществляет поиск актуальных планет, с помощью потоков
/// </summary>
public class PlanetThreadManager : MonoBehaviour, IPlanetManager
{
    public SpaceGeneral spaceGeneral;
    public ScreenManager screenManager;

    /// <summary>
    /// Текущее количество планет
    /// </summary>
    public int planetCount;
    /// <summary>
    /// Время, через которое список планет отдаетс в ScreenMaanger
    /// </summary>
    public float returnTime = 1;

    /// <summary>
    /// Продвинутый менеджер потоков
    /// </summary>
    private SmartThreadPool smartThreadPool;
    /// <summary>
    /// Найденные планеты
    /// </summary>
    private ConcurrentDictionary<Vector3Int, Planet> planetList;

    /// <summary>
    /// Состояние мира (показывает состояние потоков)
    /// </summary>
    private WorldStatus status = WorldStatus.no;

    void Start()
    {
        planetCount = spaceGeneral.spaceDB.minPlanetCount;

        planetList = new ConcurrentDictionary<Vector3Int, Planet>();

        // Запускается поток, ищущий планеты
        smartThreadPool = new SmartThreadPool();
        smartThreadPool.QueueWorkItem(PlanetListener);

        // Запускается корутина, отдающая планеты в ScreenManager
        StartCoroutine(PlanetReturning(returnTime));

    }

    void Update()
    {
        // при определенном зуме необходимо корректировать максимальное количество планет
        if (spaceGeneral.cellCount.x  < 10)
        {
            planetCount = spaceGeneral.spaceDB.maxPlanetCount;
        }
        else
            planetCount = spaceGeneral.spaceDB.minPlanetCount;
    }

    /// <summary>
    /// Вызывается в отдельном потоке. Выстраивает очередь на добавление чанков.
    /// </summary>
    public void PlanetListener()
    {
        while (true)
        {
            for (int h = -spaceGeneral.cellCount.y / 2 + spaceGeneral.cellCurrent.y + 1; h < spaceGeneral.cellCount.y / 2 + spaceGeneral.cellCurrent.y; h++)
            {
                for (int w = -spaceGeneral.cellCount.x / 2 + spaceGeneral.cellCurrent.x + 1; w < spaceGeneral.cellCount.x / 2 + spaceGeneral.cellCurrent.x; w++)
                {
                    if (status == WorldStatus.stop)
                        return;

                    //бывается момент порехода от полного оторбражения к минмальному, здесь нужно очищать список
                    if (planetList.Count > planetCount)
                        planetList = new ConcurrentDictionary<Vector3Int, Planet>();

                    // находим список планет, которые не попадают в поле зрения
                    var outOfViewList = planetList.Where(e => (e.Key.x > spaceGeneral.cellCount.x / 2 + spaceGeneral.cellCurrent.x + 1 || e.Key.x < -spaceGeneral.cellCount.x / 2 + spaceGeneral.cellCurrent.x)
                    || (e.Key.y > spaceGeneral.cellCount.y / 2 + spaceGeneral.cellCurrent.y + 1 || e.Key.y < -spaceGeneral.cellCount.y / 2 + spaceGeneral.cellCurrent.y)).ToList();

                    //очищаем
                    for (int i = 0; i < outOfViewList.Count; i++)
                    {
                        Planet deletedplanet;
                        if (planetList.TryRemove(outOfViewList[i].Key, out deletedplanet))
                        {
                            i--;
                        }
                    }
                    
                    // поиск значений из шума
                    Vector3Int planetIndex = new Vector3Int(w, h, 0);
                    Planet planet = spaceGeneral.map.GetPlanet(planetIndex); 

                    // нет планеты
                    if (planet == null)
                        continue;
                    // есть планета
                    else
                    {
                        // если планеты с таким индексом еще нет в списке
                        if (!planetList.ContainsKey(planet.index))
                        {
                            // добавляем, при условии, что список планет не перепелонен
                            if (planetList.Count < planetCount)
                                planetList.TryAdd(planet.index, planet);
                            else
                            {
                                // отсортировали планеты списка в порядке убывния расстояния рейтингов
                                var ordList = planetList.OrderByDescending(e => Mathf.Abs(spaceGeneral.map.ShipRait - e.Value.rait));

                                // первый элемент будет самым дальим по рейту
                                var longest = ordList.First();

                                // если расстояние между самым дальним элементом списка больше текущей планеты
                                if (Mathf.Abs(spaceGeneral.map.ShipRait - longest.Value.rait) > Mathf.Abs(spaceGeneral.map.ShipRait - planet.rait))
                                {
                                    Planet longestPlanet;
                                    planetList.TryRemove(longest.Key, out longestPlanet);

                                    planetList.TryAdd(planet.index, planet);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Отдает планеты на рендер раз в какое-то время
    /// </summary>
    public IEnumerator PlanetReturning(float time)
    {
        status = WorldStatus.running;
        while (true)
        {
            yield return new WaitForSeconds(time);
            screenManager.GetPlanets(planetList.Values.ToList());
        }
    }

    public void OnApplicationQuit()
    { 
        status = WorldStatus.stop;
    }

    public enum WorldStatus
    {
        no,
        running,
        pause,
        stop
    }

}
