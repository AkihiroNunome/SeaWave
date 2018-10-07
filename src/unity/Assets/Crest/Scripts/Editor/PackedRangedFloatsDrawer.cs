using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// Material attribute that allows 4 ranged float values with individual GUIs to be packed into a single float4
/// shader uniform. Empty slots are given the name _
/// </summary>
public class PackedRangedFloatsDrawer : MaterialPropertyDrawer
{
    float[] _min = new float[4];
    float[] _max = new float[4];
    GUIContent[] _labels = new GUIContent[4];
    float _numValid = 4f;

    public PackedRangedFloatsDrawer(
        string name0, float _min0, float _max0,
        string name1, float _min1, float _max1,
        string name2, float _min2, float _max2,
        string name3, float _min3, float _max3)
    {
        _labels[0] = name0 == "_" ? null : new GUIContent(name0);
        _labels[1] = name1 == "_" ? null : new GUIContent(name1);
        _labels[2] = name2 == "_" ? null : new GUIContent(name2);
        _labels[3] = name3 == "_" ? null : new GUIContent(name3);

        _min[0] = _min0; _max[0] = _max0;
        _min[1] = _min1; _max[1] = _max1;
        _min[2] = _min2; _max[2] = _max2;
        _min[3] = _min3; _max[3] = _max3;

        for (int i = 0; i < 4; i++) if (_labels[i] == null) _numValid--;
    }

    public override void OnGUI(Rect totalRect, MaterialProperty prop, String label, MaterialEditor editor)
    {
        // The magic line of code that makes EditorGUI.Slider work :/ - without this the label does not appear
        // for me. Only took a day of trial and error and googling to find this one!
        EditorGUIUtility.labelWidth = 125f;

        EditorGUI.BeginChangeCheck();

        EditorGUI.showMixedValue = prop.hasMixedValue;

        Vector4 value = prop.vectorValue;

        for (int i = 0; i < 4; i++)
        {
            if (_labels[i] == null) continue;

            var rect = totalRect;
            float h = totalRect.height / _numValid;
            rect.y += i * (h);
            rect.height = h;

            value[i] = EditorGUI.Slider(rect, _labels[i], value[i], _min[i], _max[i]);
        }

        EditorGUI.showMixedValue = false;

        if (EditorGUI.EndChangeCheck())
        {
            prop.vectorValue = value;
        }
    }

    public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
    {
        return EditorGUIUtility.singleLineHeight * _numValid;
    }
}
