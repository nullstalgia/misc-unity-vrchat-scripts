# misc-unity-vrchat-scripts
Just some Editor scripts I had use for while messing with Unity/VRChat SDK.


# Material Copier 

This Unity Editor script allows you to easily copy material assignments from one parent GameObject to another. It searches for SkinnedMeshRenderers within the source and target parent GameObjects, compares their names, and copies the material assignment from the source to the target if their names match.

#### Usage:
-    Import the MaterialCopier.cs script into your Unity project.
-    Make sure the script is placed inside an "Editor" folder in your Unity project's "Assets" folder.
-    Click on "Tools" in the menu bar and select "Material Copier" to open the custom editor window.
-    Assign the Source Parent and Target Parent GameObjects in the corresponding fields.
-    Press the "Copy Materials" button to start the material copying process.

Remember to have both parent GameObjects in your scene when using the script.

# Animation Clip Finder

The Animation Clip Finder is a Unity Editor tool that helps you find all animation clips in an AnimatorController that reference a specific GameObject. This tool is useful for debugging and managing animation clips in your Unity projects, especially when dealing with complex animation systems.

#### Usage:

-    Import the AnimationClipFinder.cs script into your Unity project.
-    Make sure the script is placed inside an "Editor" folder in your Unity project's "Assets" folder.
-    In the Unity Editor, go to Tools > Animation Clip Finder to open the Animation Clip Finder window.
-    Drag and drop an Animator and a target GameObject into their respective fields in the window.
-    Check the "Hide Duplicate Entries" checkbox if you want to display only unique animation clips in the results.
-    Click the "Find Animation Clips" button to search for animation clips that reference the target GameObject.
-    The found animation clip titles will be displayed in the window.


#### - Majority of scripts and descriptions written by ChatGPT-4
