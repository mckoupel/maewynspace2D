#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

using CoherentNoise;
using CoherentNoise.Generation;
using CoherentNoise.Generation.Combination;
using CoherentNoise.Generation.Displacement;
using CoherentNoise.Generation.Fractal;
using CoherentNoise.Generation.Modification;
using CoherentNoise.Generation.Patterns;
using CoherentNoise.Generation.Voronoi;
using CoherentNoise.Interpolation;
using CoherentNoise.Texturing;
//using System;

public class NoiseDebug : MonoBehaviour
{
    public Generator currentNoise = null;
    public Texture2D previewTex;
    public int texSize = 256;
    public int previewSize = 128;

    public string noisePath;// = "Noise/N1N";
    public string texFolder;// = "textures/Generated/Noise";
    public string texName; //= "N1N";
    public int count = 1;

    public NoiseKeeper noise = new NoiseKeeper();

    public NoiseKeeper Generate()
    {
        noise.Regenerate();

        previewTex = MakeTextureFromNoise(previewSize);
        return noise;

    }

    public Texture2D MakeTextureFromNoise(int size)
    {
        Texture2D tex = CoherentNoise.Texturing.TextureMaker.MonochromeTexture(size, size, noise.generator) as Texture2D;
        tex.name = UnityEngine.Random.Range(0, 100000).ToString();

        return tex;
    }

    public void SaveToJson()
    {
        var jsonData = JsonUtility.ToJson(noise);

        string path = Application.dataPath + "/" + noisePath + ".json";//+ "test.json";
        System.IO.File.WriteAllText(path, jsonData);
        UnityEditor.AssetDatabase.Refresh();
    }

    public void LoadFromJson()
    {
        TextAsset targetFile = Resources.Load(noisePath) as TextAsset;
        noise = JsonUtility.FromJson<NoiseKeeper>(targetFile.text);

        Generate();
    }
}

[CustomEditor(typeof(NoiseDebug))]
public class NoiseDebugEditor : Editor
{
    GUIStyle style;

    void OnEnable()
    {
        style = new GUIStyle() { padding = new RectOffset(100, 100, 100, 200) };
    }

    public override void OnInspectorGUI()
    {
        NoiseDebug t = (NoiseDebug)target;

        t.noise.type = (NoiseType)EditorGUILayout.EnumPopup("Type", t.noise.type);

        EditorGUILayout.BeginHorizontal();
        t.noise.seed = EditorGUILayout.IntField("Seed", t.noise.seed);
        if (GUILayout.Button("Rand", GUILayout.MaxWidth(80))) { t.noise.RandimizeSeed(); }
        EditorGUILayout.EndHorizontal();

        t.noise.frequency = EditorGUILayout.Slider(new GUIContent("Frequency"), t.noise.frequency, 0, 5);

        if (t.noise.type != NoiseType.voronoi)
            t.noise.octaveCount = EditorGUILayout.IntSlider("Octaves", t.noise.octaveCount, 0, 5);
        if (t.noise.type != NoiseType.voronoi && t.noise.type != NoiseType.fractalRidge)
            t.noise.persistence = EditorGUILayout.Slider(new GUIContent("Persistence"), t.noise.persistence, 0, 5);
        if (t.noise.type != NoiseType.voronoi)
            t.noise.lacunarity = EditorGUILayout.Slider(new GUIContent("Lacunarity"), t.noise.lacunarity, 0, 5);

        EditorGUILayout.PrefixLabel(string.Empty);

        t.noise.bias = EditorGUILayout.Slider(new GUIContent("Bias"), t.noise.bias, -1, 1);
        t.noise.gain = EditorGUILayout.Slider(new GUIContent("Gain"), t.noise.gain, -1, 1);
        t.noise.binarize = EditorGUILayout.Slider(new GUIContent("Binarize"), t.noise.binarize, -1.1f, 1);
        t.noise.translate = EditorGUILayout.Vector3Field(new GUIContent("Translate"), t.noise.translate);

        EditorGUILayout.PrefixLabel(string.Empty);

        EditorGUILayout.BeginHorizontal();
        t.noise.turb_seed = EditorGUILayout.IntField("Turbulence Seed", t.noise.turb_seed);
        if (GUILayout.Button("Rand", GUILayout.MaxWidth(80))) { t.noise.turb_seed = Random.Range(1, 1000000); }
        EditorGUILayout.EndHorizontal();

        t.noise.turb_freq = EditorGUILayout.Slider(new GUIContent("Turbulence Frequency"), t.noise.turb_freq, 0, 5);
        t.noise.turb_power = EditorGUILayout.Slider(new GUIContent("Turbulence Power"), t.noise.turb_power, 0, 5);

        EditorGUILayout.PrefixLabel(string.Empty);

        t.previewSize = EditorGUILayout.IntField("Preview Size", t.previewSize);
        t.noisePath = EditorGUILayout.TextField("Noise Save Path", t.noisePath);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Show Noise", GUILayout.MaxWidth(132)))
        { t.Generate(); }

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Save Noise", GUILayout.MaxWidth(80)))
        { t.SaveToJson(); }
        if (GUILayout.Button("Load Noise", GUILayout.MaxWidth(80)))
        { t.LoadFromJson(); }
        EditorGUILayout.EndHorizontal();

        // EditorGUILayout.ObjectField("Noise", targetScript.texture, typeof(Texture2D), false);
        // GUILayout.Label(targetScript.preview);

        GUILayout.Box(t.previewTex);

        t.texSize = EditorGUILayout.IntField("Texture Size", t.texSize);
        t.texFolder = EditorGUILayout.TextField("Save Folder", t.texFolder);

        EditorGUI.BeginDisabledGroup(t.count > 1 ? true : false);
        t.texName = EditorGUILayout.TextField("Texture Name", t.texName);
        EditorGUI.EndDisabledGroup();

        t.count = EditorGUILayout.IntField("Textures Count", t.count);

        GUILayout.Space(15);

        //EditorGUI.DrawPreviewTexture((new Rect(10, 550, 256, 256), targetScript.texture);


    }
}

[System.Serializable]
public enum NoiseType
{
    voronoi,
    fractalPink,
    fractalBillow,
    fractalRidge
}

[System.Serializable]
public class NoiseKeeper
{
    public NoiseType type = NoiseType.fractalPink;

    public int seed = 156567;
    public int octaveCount = 2;
    public float frequency = 0.05f;
    public float lacunarity = 1f;
    public float persistence = 0.71f;

    public float bias = 0.0f;
    public float gain = 0.0f;
    public float binarize = -1.10f;

    public Vector3 translate = Vector3.zero;

    public float turb_freq = 0.0f;
    public float turb_power = 0.0f;
    public int turb_seed = 156567;

    [System.NonSerialized]
    public Generator generator;

    public Generator Regenerate()
    {
        if (type == NoiseType.fractalBillow)
            generator = new BillowNoise(seed) { Frequency = frequency, Lacunarity = lacunarity, Persistence = persistence, OctaveCount = octaveCount };

        if (type == NoiseType.fractalPink)
            generator = new PinkNoise(seed) { Frequency = frequency, Lacunarity = lacunarity, Persistence = persistence, OctaveCount = octaveCount };

        if (type == NoiseType.fractalRidge)
            generator = new RidgeNoise(seed) { Frequency = frequency, Lacunarity = lacunarity, OctaveCount = octaveCount };

        if (type == NoiseType.voronoi)
            generator = new VoronoiValleys(seed) { Frequency = frequency };

        generator = Modification(generator);
        generator = Translate(generator);
        generator = Displacement(generator);

        return generator;
    }

    public Generator Displacement(Generator gen)
    {
        return gen.Turbulence(turb_freq, turb_power, turb_seed);
    }
    public Generator Translate(Generator gen)
    {
        return gen.Translate(translate.x, translate.y, 0);
    }
    Generator Modification(Generator gen)
    {
        Generator result = gen.Bias(bias).Gain(gain);

        if (binarize >= -1)
            result = result.Binarize(binarize);

        return result;
    }

    public void RandimizeSeed()
    {
        seed = UnityEngine.Random.Range(1, 1000000);
    }


}

#endif