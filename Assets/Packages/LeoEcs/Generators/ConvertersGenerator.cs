using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SeaJourney.Packages.LeoEcs.Generators
{
    public class ConverterGeneratorWindow : EditorWindow
    {
        private string componentName = "";

        [MenuItem("LeoECS Lite/Converter Generator")]
        public static void ShowWindow()
        {
            GetWindow<ConverterGeneratorWindow>("Converter Generator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Generate Converter", EditorStyles.boldLabel);
        
            componentName = EditorGUILayout.TextField("Component Name:", componentName);

            if (GUILayout.Button("Generate"))
            {
                if (string.IsNullOrEmpty(componentName))
                {
                    EditorUtility.DisplayDialog("Error", "Please enter component name", "OK");
                    return;
                }

                GenerateConverter();
            }
        }

        private void GenerateConverter()
        {
            string fullComponentName = componentName.EndsWith("Component") ? componentName : componentName + "Component";
        
            // Find the component type
            var componentType = FindType(fullComponentName);
            if (componentType == null)
            {
                EditorUtility.DisplayDialog("Error", $"Cannot find type {fullComponentName}", "OK");
                return;
            }

            // Find the ConverterToEntity type
            var converterBaseType = FindType("ConverterToEntity`1");
            if (converterBaseType == null)
            {
                EditorUtility.DisplayDialog("Error", "Cannot find ConverterToEntity type", "OK");
                return;
            }

            // Collect required namespaces
            var requiredNamespaces = new HashSet<string>();
        
            // Add component namespace
            if (!string.IsNullOrEmpty(componentType.Namespace))
                requiredNamespaces.Add(componentType.Namespace);
        
            // Add converter namespace
            if (!string.IsNullOrEmpty(converterBaseType.Namespace))
                requiredNamespaces.Add(converterBaseType.Namespace);

            // Generate using directives
            var usings = string.Join("\n", requiredNamespaces.OrderBy(x => x).Select(x => $"using {x};"));

            string code = 
                $@"{usings}

namespace Converters
{{
    public class {fullComponentName}Converter : ConverterToEntity<{fullComponentName}>
    {{
    }}
}}";

            string path = EditorUtility.SaveFilePanel(
                "Save Converter",
                Application.dataPath,
                fullComponentName + "Converter.cs",
                "cs");

            if (!string.IsNullOrEmpty(path))
            {
                System.IO.File.WriteAllText(path, code);
                AssetDatabase.Refresh();
            }
        }

        private Type FindType(string typeName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name == typeName || t.Name == typeName.Replace("`1", ""));
        }
    }
}