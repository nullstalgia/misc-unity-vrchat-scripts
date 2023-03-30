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
using UnityEngine;
using UnityEditor;

public class MaterialCopier : EditorWindow
{
    private GameObject sourceParent;
    private GameObject targetParent;

    [MenuItem("Tools/Null's Tools/Material Copier")]
    public static void ShowWindow()
    {
        GetWindow<MaterialCopier>("Material Copier");
    }
    
    #region GUI
    private void OnGUI()
    {
        GUILayout.Label("Material Copier (by name)", EditorStyles.boldLabel);
        sourceParent = (GameObject)EditorGUILayout.ObjectField("Source Parent", sourceParent, typeof(GameObject), true);
        targetParent = (GameObject)EditorGUILayout.ObjectField("Target Parent", targetParent, typeof(GameObject), true);

        if (GUILayout.Button("Copy Materials"))
        {
            CopyMaterials(sourceParent, targetParent);
        }
    }
    #endregion

    #region Utility
    private void CopyMaterials(GameObject source, GameObject target)
    {
        if (source == null || target == null)
        {
            Debug.LogError("Source and/or target parent GameObjects are not assigned.");
            return;
        }

        SkinnedMeshRenderer[] sourceMeshes = source.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        SkinnedMeshRenderer[] targetMeshes = target.GetComponentsInChildren<SkinnedMeshRenderer>(true);

        foreach (SkinnedMeshRenderer sourceMesh in sourceMeshes)
        {
            foreach (SkinnedMeshRenderer targetMesh in targetMeshes)
            {
                if (sourceMesh.gameObject.name == targetMesh.gameObject.name)
                {
                    targetMesh.sharedMaterials = sourceMesh.sharedMaterials;
                    Debug.Log("Materials copied from " + sourceMesh.gameObject.name + " to " + targetMesh.gameObject.name);
                }
            }
        }
    }
    #endregion
}