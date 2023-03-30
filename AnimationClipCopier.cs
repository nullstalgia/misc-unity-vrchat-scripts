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
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using System.Collections.Generic;

public class AnimationKeyframeCopier : EditorWindow
{
    private Animator targetAnimator;
    private GameObject sourceGameObject;
    private GameObject targetGameObject;
    private List<AnimationClip> animationClips;

    [MenuItem("Tools/Null's Tools/Animation Keyframe Copier")]
    public static void ShowWindow()
    {
        GetWindow<AnimationKeyframeCopier>("Animation Keyframe Copier");
    }

    private void OnGUI()
    {
        GUILayout.Label("Enter the Animator and Source/Target GameObjects to copy keyframes from/to", EditorStyles.boldLabel);

        targetAnimator = (Animator)EditorGUILayout.ObjectField("Animator", targetAnimator, typeof(Animator), true);
        sourceGameObject = (GameObject)EditorGUILayout.ObjectField("Source GameObject", sourceGameObject, typeof(GameObject), true);
        targetGameObject = (GameObject)EditorGUILayout.ObjectField("Target GameObject", targetGameObject, typeof(GameObject), true);

        if (GUILayout.Button("Copy Keyframes"))
        {
            if (targetAnimator != null && sourceGameObject != null && targetGameObject != null)
            {
                CopyKeyframes(targetAnimator, sourceGameObject, targetGameObject);
            }
        }
        if (animationClips != null && animationClips.Count > 0)
        {
            EditorGUILayout.LabelField("Modified Animation Clips:", EditorStyles.boldLabel);
            foreach (var clip in animationClips)
            {
                EditorGUILayout.LabelField(clip.name);
            }
        }
    }

    private void CopyKeyframes(Animator animator, GameObject sourceObject, GameObject targetObject)
    {
        animationClips = new List<AnimationClip>();
        AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;

        if (controller != null)
        {
            foreach (var layer in controller.layers)
            {
                ProcessStateMachine(layer.stateMachine, animator, sourceObject, targetObject);
            }
            Debug.Log("Keyframes copied successfully.");
        }
        else
        {
            Debug.Log("No AnimatorController found");
        }
    }

    private void ProcessStateMachine(AnimatorStateMachine stateMachine, Animator animator, GameObject sourceObject, GameObject targetObject)
    {
        foreach (var state in stateMachine.states)
        {
            Motion motion = state.state.motion;
            ProcessMotion(motion, animator, sourceObject, targetObject);
        }

        foreach (var childStateMachine in stateMachine.stateMachines)
        {
            ProcessStateMachine(childStateMachine.stateMachine, animator, sourceObject, targetObject);
        }
    }

    private void ProcessMotion(Motion motion, Animator animator, GameObject sourceObject, GameObject targetObject)
    {
        if (motion is BlendTree)
        {
            BlendTree blendTree = motion as BlendTree;
            foreach (var childMotion in blendTree.children)
            {
                ProcessMotion(childMotion.motion, animator, sourceObject, targetObject);
            }
        }
        else if (motion is AnimationClip)
        {
            AnimationClip clip = motion as AnimationClip;
            CopyKeyframesForClip(clip, animator, sourceObject, targetObject);
        }
    }

    private void CopyKeyframesForClip(AnimationClip clip, Animator animator, GameObject sourceObject, GameObject targetObject)
    {
        EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(clip);

        string sourceObjectPath = AnimationUtility.CalculateTransformPath(sourceObject.transform, animator.transform);
        string targetObjectPath = AnimationUtility.CalculateTransformPath(targetObject.transform, animator.transform);

        Undo.RecordObject(clip, "Copy Keyframes to Target");

        foreach (var binding in bindings)
        {
            if (binding.path == sourceObjectPath)
            {
                EditorCurveBinding targetBinding = binding;
                targetBinding.path = targetObjectPath;

                AnimationCurve sourceCurve = AnimationUtility.GetEditorCurve(clip, binding);
                AnimationCurve targetCurve = new AnimationCurve(sourceCurve.keys);

                AnimationUtility.SetEditorCurve(clip, targetBinding, targetCurve);
                animationClips.Add(clip);
            }
        }
    }
}
