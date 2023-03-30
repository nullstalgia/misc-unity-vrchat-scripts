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

public class AnimationClipFinder : EditorWindow
{
    private Animator targetAnimator;
    private GameObject targetGameObject;
    private List<AnimationClip> animationClips;
    private bool hideDuplicateEntries;

    [MenuItem("Tools/Null's Tools/Animation Clip Finder")]
    public static void ShowWindow()
    {
        GetWindow<AnimationClipFinder>("Animation Clip Finder");
    }

    private void OnGUI()
    {
        GUILayout.Label("Enter an Animator and GameObject to find relevant Clips", EditorStyles.boldLabel);

        targetAnimator = (Animator)EditorGUILayout.ObjectField("Animator", targetAnimator, typeof(Animator), true);
        targetGameObject = (GameObject)EditorGUILayout.ObjectField("GameObject", targetGameObject, typeof(GameObject), true);

        hideDuplicateEntries = EditorGUILayout.Toggle("Hide Duplicate Entries", hideDuplicateEntries);

        if (GUILayout.Button("Find Animation Clips"))
        {
            if (targetAnimator != null && targetGameObject != null)
            {
                animationClips = new List<AnimationClip>();
                AnimatorController controller = targetAnimator.runtimeAnimatorController as AnimatorController;
                FindAnimationClips(controller, targetGameObject, hideDuplicateEntries, animationClips);
            }
        }

        if (animationClips != null)
        {
            EditorGUILayout.LabelField("Found Animation Clips:", EditorStyles.boldLabel);
            foreach (var clip in animationClips)
            {
                EditorGUILayout.LabelField(clip.name);
            }
        }
    }

    private void FindAnimationClips(AnimatorController controller, GameObject targetObject, bool hideDuplicates, List<AnimationClip> clips)
    {
        if (controller != null)
        {
            foreach (var layer in controller.layers)
            {
                ProcessStateMachine(layer.stateMachine, targetAnimator, targetObject, hideDuplicates, clips);
            }
        }
    }

    private void ProcessStateMachine(AnimatorStateMachine stateMachine, Animator animator, GameObject targetObject, bool hideDuplicates, List<AnimationClip> clips)
    {
        foreach (var state in stateMachine.states)
        {
            Motion motion = state.state.motion;
            ProcessMotion(motion, animator, targetObject, hideDuplicates, clips);
        }

        foreach (var childStateMachine in stateMachine.stateMachines)
        {
            ProcessStateMachine(childStateMachine.stateMachine, animator, targetObject, hideDuplicates, clips);
        }
    }

    private void ProcessMotion(Motion motion, Animator animator, GameObject targetObject, bool hideDuplicates, List<AnimationClip> clips)
    {
        if (motion is BlendTree)
        {
            BlendTree blendTree = motion as BlendTree;
            foreach (var childMotion in blendTree.children)
            {
                ProcessMotion(childMotion.motion, animator, targetObject, hideDuplicates, clips);
            }
        }
        else if (motion is AnimationClip)
        {
            AnimationClip clip = motion as AnimationClip;
            AddClipIfReferencesObject(clip, animator, targetObject, hideDuplicates, clips);
        }
    }

    private void AddClipIfReferencesObject(AnimationClip clip, Animator animator, GameObject targetObject, bool hideDuplicates, List<AnimationClip> clips)
    {
        EditorCurveBinding[] objectReferenceBindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);
        EditorCurveBinding[] propertyBindings = AnimationUtility.GetCurveBindings(clip);
        List<EditorCurveBinding> allBindings = new List<EditorCurveBinding>(objectReferenceBindings);
        allBindings.AddRange(propertyBindings);

        string targetObjectPath = AnimationUtility.CalculateTransformPath(targetObject.transform, animator.transform);

        foreach (var binding in allBindings)
        {
            if (binding.path == targetObjectPath)
            {
                if (hideDuplicates)
                {
                    if (!clips.Contains(clip))
                    {
                        clips.Add(clip);
                    }
                }
                else
                {
                    clips.Add(clip);
                }
                break;
            }
        }
    }
}
