using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CoherentNoise;
using CoherentNoise.Generation.Fractal;
using CoherentNoise.Generation.Voronoi;

/// <summary>
/// Инициализация и метода для доступа к шуму
/// </summary>
public class SpaceNoise : ISpaceMap
{
    public int seed;
    /// <summary>
    /// рейтинг корабля
    /// </summary>
    public int shiprait;
    public int ShipRait { get { return shiprait; } }

    /// <summary>
    /// Процент, после которого ячейка заполняется планетой
    /// </summary>
    public int occupancy;

    /// <summary>
    /// Отвечает за заполняемость карты 
    /// </summary>
    public Generator noiseOccupancy;

    /// <summary>
    /// Отвечает за вариант спрайта планеты 
    /// </summary>
    public Generator noiseVariant;

    /// <summary>
    /// Отвечает за рейтинг планеты 
    /// </summary>
    public Generator noiseRait;


    public SpaceNoise(int occupancy = 20)
    {
        this.occupancy = occupancy;
        seed = Random.Range(0, 10000);
        //рейтинг корабля задал равным сиду, потому что могу))
        shiprait = seed;

        noiseOccupancy = (new PinkNoise(this.seed) { Frequency = 2f, Lacunarity = 2f, Persistence = 2f, OctaveCount = 2 }).Bias(0);
        noiseVariant = (new BillowNoise(this.seed) { Frequency = 100.2f, Lacunarity = 4.1f, Persistence = 3.8f, OctaveCount = 2 });
        noiseRait = (new BillowNoise(this.seed + 1) { Frequency = 100.2f, Lacunarity = 4.1f, Persistence = 3.8f, OctaveCount = 2 });
    }

    public Planet GetPlanet(Vector3Int planetIndex)
    {
        int planetOccupancy = NoiseToPercent(new Vector2(planetIndex.x, planetIndex.y), noiseOccupancy);
        var planetVariant = NoiseToIndex(new Vector2(planetIndex.x, planetIndex.y), noiseVariant, 4 - 1);
        int planetRaiting = NoiseToIndex(new Vector2(planetIndex.x, planetIndex.y), noiseRait, 10000);

        if (planetOccupancy < occupancy)
            return null;
        else
            return new Planet(planetIndex, planetVariant, planetRaiting);
    }

    public SpaceMap SaveToMap(Vector3Int sizes)
    {
        SpaceMap spaceMap = new SpaceMap(sizes, shiprait);

        for (int h = -sizes.y / 2 + 1; h < sizes.y / 2; h++)
        {
            for (int w = -sizes.x / 2 + 1; w < sizes.x / 2; w++)
            {
                spaceMap.planetList.Add(GetPlanet(new Vector3Int(w, h, 0)));
            }
        }

        return spaceMap;
    }

    /// <summary>
    /// Преобразование значения шума к значениям от 0 до 1
    /// </summary>
    public float NormalizeNoise(float value)
    {
        return (value * 0.5f) + 0.5f;
    }

    /// <summary>
    /// Преобразование значения шума к значениям от 0 до 100
    /// </summary>
    public int NoiseToPercent(Vector2 index, Generator gen)
    {
        return (int)Mathf.Abs((NormalizeNoise(gen.GetValue(index.x, index.y, 0)) * 100));
    }

    /// <summary>
    /// Преобразование значения шума к значениям от 0 до count
    /// </summary>
    public int NoiseToIndex(Vector2 index, Generator gen, int count)
    {
        int value = (int)Mathf.Abs(NormalizeNoise(gen.GetValue(index.x, index.y, 0) * count));

        if (value > count)
            value -= (int)(value / count) * count;

        return value;
    }

}
