using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Позволяет использовать разные PlanetManager'ы (не обязательно потоки)
/// </summary>
public interface IPlanetManager
{
    void PlanetListener();
    IEnumerator PlanetReturning(float time);
}
