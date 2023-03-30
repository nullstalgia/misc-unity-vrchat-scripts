# misc-unity-vrchat-scripts
Just some Editor scripts I had use for while messing with Unity/VRChat SDK.


# Material Copier 

This Unity Editor script allows you to easily copy material assignments from one parent GameObject to another. It searches for SkinnedMeshRenderers within the source and target parent GameObjects, compares their names, and copies the material assignment from the source to the target if their names match.

#### Usage:
-    Import the MaterialCopier.cs script into your Unity project.
-    Make sure the script is placed inside an "Editor" folder in your Unity project's "Assets" folder.
-    In the Unity Editor, go to Tools > Null's Tools > Material Copier to open the custom editor window.
-    Assign the Source Parent and Target Parent GameObjects in the corresponding fields.
-    Press the "Copy Materials" button to start the material copying process.

Remember to have both parent GameObjects in your scene when using the script.

# Animation Clip Finder

The Animation Clip Finder is a Unity Editor tool that helps you find all animation clips in an AnimatorController that reference a specific GameObject. This tool is useful for debugging and managing animation clips in your Unity projects, especially when dealing with complex animation systems.

#### Usage:

-    Import the AnimationClipFinder.cs script into your Unity project.
-    Make sure the script is placed inside an "Editor" folder in your Unity project's "Assets" folder.
-    In the Unity Editor, go to Tools > Null's Tools > Animation Clip Finder to open the Animation Clip Finder window.
-    Drag and drop an Animator and a target GameObject into their respective fields in the window.
-    Check the "Hide Duplicate Entries" checkbox if you want to display only unique animation clips in the results.
-    Click the "Find Animation Clips" button to search for animation clips that reference the target GameObject.
-    The found animation clip titles will be displayed in the window.

# Animation Keyframe Copier for Unity

This Unity Editor Script provides an easy way to copy animation keyframes from a source GameObject to a target GameObject within an Animator. The script includes support for copying keyframes from both standard animation states and Blend Trees.

#### Usage:

-    Import the AnimationClipFinder.cs script into your Unity project.
-    Make sure the script is placed inside an "Editor" folder in your Unity project's "Assets" folder.
-    In the Unity Editor, go to Tools > Null's Tools > Animation Keyframe Copier
-    In the Animation Keyframe Copier window:
    - Assign the Animator containing the animations you want to work with.
    - Assign the source GameObject from which you want to copy the keyframes.
    - Assign the target GameObject to which you want to copy the keyframes.
    - Click the Copy Keyframes button to start the process.
-   The script will then copy all keyframes from the source GameObject to the target GameObject for all animations in the AnimatorController, including those within Blend Trees.
-   After copying the keyframes, the modified animation clips will be displayed

# Animation Clip Keyframes Incrementor

The Animation Clip Keyframes Incrementor is a Unity Editor tool that allows you to increment properties in an AnimationClip by a set amount, for a specified number of times. The tool detects the properties within the AnimationClip and provides checkboxes to let you choose which properties to modify.

#### Usage:

-    Import the AnimationClipKeyframesIncrementor.cs script into your Unity project.
-    Make sure the script is placed inside an "Editor" folder in your Unity project's "Assets" folder.
-    In the Unity Editor, go to Tools > Null's Tools > Animation Clip Keyframes Incrementor to open the Animation Clip Keyframes Editor window.
-    Drag and drop an AnimationClip asset into the "Animation Clip" field in the window.
-    Set the "Increment Value" field to determine the amount by which the properties should be incremented.
-    Set the "Number of Keyframes" field to specify the number of times you want to add new keyframes.
-    Under "Properties to Update", check the checkboxes next to the properties you want to increment.
-    Click the "Add Keyframes" button to add new keyframes with incremented values to the selected properties in the AnimationClip.

#### - Majority of scripts and descriptions written by ChatGPT-4
