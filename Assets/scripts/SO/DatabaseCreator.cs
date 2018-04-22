#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// Создание базы данных
/// </summary>
public class DatabaseCreator : MonoBehaviour
{
    public string dbPath;
    public string dbName;
    public string type;

    public void CreateDataBase(string path, string name, string type)
    {
        var instance = ScriptableObject.CreateInstance(type);
        AssetDatabase.CreateAsset(instance, path + "/" + name + ".asset");
        AssetDatabase.SaveAssets();
    }
}

[CustomEditor(typeof(DatabaseCreator))]
public class DatabaseCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DatabaseCreator t = (DatabaseCreator)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Create Database", GUILayout.MaxWidth(115)))
        {
            t.CreateDataBase(t.dbPath, t.dbName, t.type);
        }

    }
}
#endif