using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Интерфейс, отвечающий за карту (чтобы можно было использовать как шум, там и ранее сохраненную область)
/// </summary>
public interface ISpaceMap
{
    int ShipRait { get; }
    Planet GetPlanet(Vector3Int index);
}
