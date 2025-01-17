#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class MaterialMenuExtension : Editor
{
    // Add menu item to material context menu
    [MenuItem("CONTEXT/Material/Create New Material(s)")]
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
                    if (!processedMaterials.TryGetValue(sourceMaterial, out Material newMaterial))
                    {
                        // Create new material with the same shader as the source
                        newMaterial = new Material(sourceMaterial.shader);

                        // Dynamically copy all relevant properties
                        CopyMaterialProperties(sourceMaterial, newMaterial);

                        // Generate unique path
                        string materialPath = GetUniqueMaterialPath(obj, sourceMaterial);

                        // Ensure Materials directory exists
                        string directory = Path.GetDirectoryName(materialPath);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        // Save the new material
                        try
                        {
                            AssetDatabase.CreateAsset(newMaterial, materialPath);
                            Debug.Log($"Material created: {materialPath}");
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogError($"Failed to save material: {ex.Message}");
                            continue;
                        }

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
    [MenuItem("CONTEXT/Material/Create New Material Material(s)", true)]
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

    private static void CopyMaterialProperties(Material sourceMaterial, Material newMaterial)
    {
        for (int i = 0; i < ShaderUtil.GetPropertyCount(sourceMaterial.shader); i++)
        {
            string propertyName = ShaderUtil.GetPropertyName(sourceMaterial.shader, i);
            var type = ShaderUtil.GetPropertyType(sourceMaterial.shader, i);

            try
            {
                switch (type)
                {
                    case ShaderUtil.ShaderPropertyType.Color:
                        newMaterial.SetColor(propertyName, sourceMaterial.GetColor(propertyName));
                        break;
                    case ShaderUtil.ShaderPropertyType.Vector:
                        newMaterial.SetVector(propertyName, sourceMaterial.GetVector(propertyName));
                        break;
                    case ShaderUtil.ShaderPropertyType.Float:
                    case ShaderUtil.ShaderPropertyType.Range:
                        newMaterial.SetFloat(propertyName, sourceMaterial.GetFloat(propertyName));
                        break;
                    case ShaderUtil.ShaderPropertyType.TexEnv:
                        newMaterial.SetTexture(propertyName, sourceMaterial.GetTexture(propertyName));
                        break;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"Failed to copy property '{propertyName}': {ex.Message}");
            }
        }
    }
}

#endif
