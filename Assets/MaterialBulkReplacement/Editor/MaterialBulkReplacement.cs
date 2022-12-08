using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MaterialBulkReplacement {
    public class MaterialBulkReplacement : EditorWindow
    {
        private Material targetMaterial;
        private Material replaceMaterial;

        [MenuItem("Tools/MaterialBulkReplacement/Material Bulk Replacement Window")]
        public static void ReplaceSelectAll() {
            MaterialBulkReplacement editorWindow = (MaterialBulkReplacement)EditorWindow.GetWindow(typeof(MaterialBulkReplacement));
            editorWindow.titleContent = new GUIContent("Material Bulk Replacement");
            editorWindow.Show();
        }

        void OnGUI()
        {
            GUILayout.Label("Settings", EditorStyles.boldLabel);

            this.targetMaterial = (Material)EditorGUILayout.ObjectField("Target Material", this.targetMaterial, typeof(Material), false);
            this.replaceMaterial = (Material)EditorGUILayout.ObjectField("Replace Material", this.replaceMaterial, typeof(Material), false);

            if (GUILayout.Button("Replace"))
            {
                this.materialBulkReplace();
            }
        }

        private void materialBulkReplace()
        {
            Selection.gameObjects.ForEach(selectedGameObject => 
            {
                var meshRenderer = selectedGameObject.GetComponent<MeshRenderer>();
                
                var originalMeshRendererMaterials = meshRenderer.sharedMaterials;
                var replacedMeshRendererMaterials = meshRenderer.sharedMaterials.Select(meshRendererMaterial =>
                {
                    if (meshRendererMaterial == this.targetMaterial)
                    {
                        meshRendererMaterial = this.replaceMaterial;
                    }
                    return meshRendererMaterial;
                }).ToArray();

                meshRenderer.sharedMaterials = replacedMeshRendererMaterials;
            });
        }
    }

    public static class Extension
    {
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (T item in sequence)
                action(item);
        }
    }
}