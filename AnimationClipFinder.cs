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

    // Add a menu item for the Animation Clip Finder window
    [MenuItem("Tools/Animation Clip Finder")]
    public static void ShowWindow()
    {
        // Create the window with the specified title
        GetWindow<AnimationClipFinder>("Animation Clip Finder");
    }

    private void OnGUI()
    {
        // Display input fields and options
        GUILayout.Label("Enter the Animator and GameObject to find Animation Clips", EditorStyles.boldLabel);

        targetAnimator = (Animator)EditorGUILayout.ObjectField("Animator", targetAnimator, typeof(Animator), true);
        targetGameObject = (GameObject)EditorGUILayout.ObjectField("GameObject", targetGameObject, typeof(GameObject), true);

        hideDuplicateEntries = EditorGUILayout.Toggle("Hide Duplicate Entries", hideDuplicateEntries);

        // Find animation clips when the button is clicked
        if (GUILayout.Button("Find Animation Clips"))
        {
            if (targetAnimator != null && targetGameObject != null)
            {
                animationClips = FindAnimationClips(targetAnimator, targetGameObject, hideDuplicateEntries);
            }
        }

        // Display the found animation clips
        if (animationClips != null)
        {
            EditorGUILayout.LabelField("Found Animation Clips:", EditorStyles.boldLabel);
            foreach (var clip in animationClips)
            {
                EditorGUILayout.LabelField(clip.name);
            }
        }
    }

    // Find all animation clips that reference the target GameObject
    private List<AnimationClip> FindAnimationClips(Animator animator, GameObject targetObject, bool hideDuplicates)
    {
        List<AnimationClip> clips = new List<AnimationClip>();
        AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;

        // Calculate the target GameObject's path relative to the Animator
        string targetObjectPath = AnimationUtility.CalculateTransformPath(targetObject.transform, animator.transform);


        if (controller != null)
        {
            // Iterate through all layers and states in the AnimatorController
            foreach (var layer in controller.layers)
            {
                foreach (var state in layer.stateMachine.states)
                {
                    AnimationClip clip = state.state.motion as AnimationClip;

                    if (clip != null)
                    {
                        // Get all bindings (object reference and property) from the animation clip
                        EditorCurveBinding[] objectReferenceBindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);
                        EditorCurveBinding[] propertyBindings = AnimationUtility.GetCurveBindings(clip);
                        List<EditorCurveBinding> allBindings = new List<EditorCurveBinding>(objectReferenceBindings);
                        allBindings.AddRange(propertyBindings);

                        // Check if the target GameObject is referenced in the bindings
                        foreach (var binding in allBindings)
                        {
                            if (binding.path == targetObjectPath)
                            {
                                // Add the clip to the list if duplicates are allowed or the clip is not already in the list
                                if (!hideDuplicates || !clips.Contains(clip))
                                {
                                    clips.Add(clip);
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        return clips;
    }
}
