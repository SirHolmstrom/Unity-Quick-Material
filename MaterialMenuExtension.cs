using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class MaterialMenuExtension : Editor
{
    // Add menu item to material context menu
    [MenuItem("CONTEXT/Material/Create New Material From This")]
    private static void CreateNewMaterialFromThis(MenuCommand command)
    {
        Material sourceMaterial = command.context as Material;
        if (sourceMaterial == null) return;

        // Get all selected objects with MeshRenderers
        var selectedObjects = Selection.gameObjects;
        var processedMaterials = new Dictionary<Material, Material>();

        foreach (var obj in selectedObjects)
        {
            var meshRenderer = obj.GetComponent<MeshRenderer>();
            if (meshRenderer == null) continue;

            var materials = meshRenderer.sharedMaterials;
            bool materialFound = false;

            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i] == sourceMaterial)
                {
                    materialFound = true;

                    // Check if we already created a replacement for this material
                    Material newMaterial;
                    if (!processedMaterials.TryGetValue(sourceMaterial, out newMaterial))
                    {
                        // Create new material with standard shader
                        newMaterial = new Material(Shader.Find("Standard"));

                        // Copy relevant properties from source material
                        if (sourceMaterial.HasProperty("_MainTex"))
                        {
                            var mainTex = sourceMaterial.GetTexture("_MainTex");
                            if (mainTex != null)
                                newMaterial.SetTexture("_MainTex", mainTex);
                        }

                        if (sourceMaterial.HasProperty("_Color"))
                        {
                            newMaterial.SetColor("_Color", sourceMaterial.GetColor("_Color"));
                        }

                        // Generate unique path
                        string materialPath = GetUniqueMaterialPath(obj, sourceMaterial);

                        // Ensure Materials directory exists
                        string directory = Path.GetDirectoryName(materialPath);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        // Save the new material
                        AssetDatabase.CreateAsset(newMaterial, materialPath);
                        processedMaterials[sourceMaterial] = newMaterial;
                    }

                    // Replace the material
                    materials[i] = newMaterial;
                }
            }

            if (materialFound)
            {
                meshRenderer.sharedMaterials = materials;
            }
        }

        if (processedMaterials.Count > 0)
        {
            AssetDatabase.SaveAssets();
        }
    }

    // Validate the menu item
    [MenuItem("CONTEXT/Material/Create New Material From This", true)]
    private static bool ValidateCreateNewMaterial(MenuCommand command)
    {
        Material material = command.context as Material;
        if (material == null) return false;

        // Check if any selected object has this material
        var selectedObjects = Selection.gameObjects;
        foreach (var obj in selectedObjects)
        {
            var meshRenderer = obj.GetComponent<MeshRenderer>();
            if (meshRenderer == null) continue;

            var materials = meshRenderer.sharedMaterials;
            if (System.Array.Exists(materials, m => m == material))
            {
                return true;
            }
        }

        return false;
    }

    private static string GetUniqueMaterialPath(GameObject targetObject, Material sourceMaterial)
    {
        string baseFolder = "Assets/Materials";
        string baseName = sourceMaterial != null ? sourceMaterial.name : targetObject.name;
        string materialName = $"{baseName}_Material.mat";
        string fullPath = Path.Combine(baseFolder, materialName);

        int counter = 1;
        while (File.Exists(fullPath))
        {
            materialName = $"{baseName}_Material_{counter}.mat";
            fullPath = Path.Combine(baseFolder, materialName);
            counter++;
        }

        return fullPath;
    }
}