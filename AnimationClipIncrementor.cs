/*
 * Copyright (C) 2023 nullstalgia
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationClipKeyframesEditor : EditorWindow
{
    private AnimationClip animationClip;
    private float incrementValue = 1.0f;
    private int numberOfKeyframes = 1;

    private Dictionary<string, bool> propertyToggles = new Dictionary<string, bool>();

    [MenuItem("Tools/Null's Tools/Animation Clip Keyframes Incrementor")]
    public static void ShowWindow()
    {
        GetWindow<AnimationClipKeyframesEditor>("Animation Clip Keyframes Incrementor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select clip and properties to increment", EditorStyles.boldLabel);

        AnimationClip newClip = EditorGUILayout.ObjectField("Animation Clip", animationClip, typeof(AnimationClip), false) as AnimationClip;

        if (newClip != animationClip)
        {
            animationClip = newClip;
            UpdateProperties();
        }

        incrementValue = EditorGUILayout.FloatField("Increment Value", incrementValue);
        numberOfKeyframes = EditorGUILayout.IntField("Number of new Keyframes", numberOfKeyframes);

        GUILayout.Space(10);

        GUILayout.Label("Properties to Increment", EditorStyles.boldLabel);

        var propertyNames = new List<string>(propertyToggles.Keys);

        foreach (var propertyName in propertyNames)
        {
            EditorGUI.BeginChangeCheck();
            bool newValue = EditorGUILayout.Toggle(propertyName, propertyToggles[propertyName]);

            if (EditorGUI.EndChangeCheck())
            {
                propertyToggles[propertyName] = newValue;
            }
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Add Keyframes"))
        {
            AddKeyframes();
        }
    }

    private void UpdateProperties()
    {
        propertyToggles.Clear();

        if (animationClip != null)
        {
            var bindings = AnimationUtility.GetCurveBindings(animationClip);

            foreach (var binding in bindings)
            {
                if (!propertyToggles.ContainsKey(binding.propertyName))
                {
                    propertyToggles.Add(binding.propertyName, false);
                }
            }
        }
    }

    private void AddKeyframes()
    {
        if (animationClip == null)
        {
            Debug.LogError("No Animation Clip selected.");
            return;
        }

        Undo.RecordObject(animationClip, "Increment Keyframes in Animation Clip");

        var bindings = AnimationUtility.GetCurveBindings(animationClip);
        var propertiesToUpdate = new List<EditorCurveBinding>();

        foreach (var binding in bindings)
        {
            if (propertyToggles[binding.propertyName])
            {
                propertiesToUpdate.Add(binding);
            }
        }

        if (propertiesToUpdate.Count == 0)
        {
            Debug.LogWarning("No properties selected. Please select at least one property to update.");
            return;
        }

        float frameRate = animationClip.frameRate;

        foreach (var binding in propertiesToUpdate)
        {
            var curve = AnimationUtility.GetEditorCurve(animationClip, binding);
            float lastTime = curve.keys[curve.length - 1].time;
            float lastValue = curve.keys[curve.length - 1].value;

            for (int i = 1; i <= numberOfKeyframes; i++)
            {
                float newTime = lastTime + (incrementValue / frameRate) * i;
                float newValue = lastValue + incrementValue * i;
                curve.AddKey(newTime, newValue);
            }

            AnimationUtility.SetEditorCurve(animationClip, binding, curve);
        }

        Debug.Log("Keyframes added successfully.");
    }
}
