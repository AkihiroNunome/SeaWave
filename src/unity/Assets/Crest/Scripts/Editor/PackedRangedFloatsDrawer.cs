using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// Material attribute that allows 4 ranged float values with individual GUIs to be packed into a single float4
/// shader uniform. Empy slots are given the name _
/// </summary>
public class PackedRangedFloatsDrawer : MaterialPropertyDrawer
{
    string[] _name = new string[4];
    float[] _min = new float[4];
    float[] _max = new float[4];
    float _numValid = 4f;

    public PackedRangedFloatsDrawer(
        string name0, float _min0, float _max0,
        string name1, float _min1, float _max1,
        string name2, float _min2, float _max2,
        string name3, float _min3, float _max3)
    {
        _name[0] = name0; _min[0] = _min0; _max[0] = _max0;
        _name[1] = name1; _min[1] = _min1; _max[1] = _max1;
        _name[2] = name2; _min[2] = _min2; _max[2] = _max2;
        _name[3] = name3; _min[3] = _min3; _max[3] = _max3;

        for(int i = 0; i < 4; i++)
        {
            if (_name[i] == "_")
            {
                _name[i] = null;
                _numValid--;
            }
        }
    }

    public override void OnGUI(Rect position, MaterialProperty prop, String label, MaterialEditor editor)
    {
        EditorGUI.BeginChangeCheck();

        EditorGUI.showMixedValue = prop.hasMixedValue;

        Vector4 value = prop.vectorValue;

        for (int i = 0; i < 4; i++)
        {
            if (_name[i] == null) continue;

            Rect rect = position;
            float h = position.height / _numValid;
            rect.y += i * (h);
            rect.height = h;

            // Any label makes the slider disappear??
            //value[i] = EditorGUI.Slider(rect, new GUIContent("a"), value[i], _min[i], _max[i]);

            // Hack - force a label and a slider to draw :(. this does not seem to fill the full width
            // or be proportioned properly, but its close enough i guess.
            float w = 1 / 2.45f;
            Rect rectL = rect;
            rectL.width = rect.width * w;
            EditorGUI.LabelField(rectL, _name[i]);
            Rect rectR = rect;
            rectR.x = rect.width * w;
            rectR.width = rect.width * (1f - w) * 1.06f;
            value[i] = EditorGUI.Slider(rectR, value[i], _min[i], _max[i]);
        }

        EditorGUI.showMixedValue = false;

        if (EditorGUI.EndChangeCheck())
        {
            prop.vectorValue = value;
        }
    }

    public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
    {
        return base.GetPropertyHeight(prop, label, editor) * _numValid;
    }
}
