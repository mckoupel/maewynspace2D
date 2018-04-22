using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Карта, сохраняема из шума и загружаемая
/// </summary>
[System.Serializable]
public class SpaceMap : ISpaceMap
{
    [SerializeField]
    public List<Planet> planetList;

    [SerializeField]
    public int shiprait;
    public int ShipRait { get { return shiprait; } }

    public SpaceMap(Vector3Int size, int shiprait)
    {
        this.shiprait = shiprait;
        planetList = new List<Planet>();
    }

    public Planet GetPlanet(Vector3Int planetIndex)
    {
        return planetList.FirstOrDefault(e => e.index == planetIndex);
    }
}
