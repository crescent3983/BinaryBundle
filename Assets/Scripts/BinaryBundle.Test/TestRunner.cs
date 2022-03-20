using BinaryBundle;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.Assertions;

namespace BinaryBundleTest {
    public static class TestRunner {

        [MenuItem("Window/BinaryBundle/Test/Generate Code", false)]
        public static void GenerateCode() {
            // Get output folder
            var script = AssetDatabase.FindAssets("t:Script TestRunner");
            var scriptPath = AssetDatabase.GUIDToAssetPath(script[0]);
            var outputPath = Path.GetDirectoryName(scriptPath) + "/Generated/Generated.cs";

            // Generate all fit types
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(TestRunner));
            var original = File.Exists(outputPath) ? File.ReadAllText(outputPath) : string.Empty;
            BinaryBundleSerializer.GenerateCode(outputPath, assembly.GetTypes());

            // Revert back when compliation failed
            void OnCompilationFinish(string s, CompilerMessage[] compilerMessages) {
                CompilationPipeline.assemblyCompilationFinished -= OnCompilationFinish;
                if (compilerMessages.Count(m => m.type == CompilerMessageType.Error) > 0) {
                    if (EditorUtility.DisplayDialog("Compile Failed", "Revert?", "OK", "Cancel")) {
                        File.WriteAllText(outputPath, original);
                        AssetDatabase.ImportAsset(outputPath, ImportAssetOptions.ForceSynchronousImport);
                    }
                }
            }
            CompilationPipeline.assemblyCompilationFinished += OnCompilationFinish;
            AssetDatabase.ImportAsset(outputPath, ImportAssetOptions.ForceSynchronousImport);
        }

        [MenuItem("Window/BinaryBundle/Test/Run Test", false)]
        public static void RunTest() {
            Assert.raiseExceptions = true;
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(TestRunner));
            foreach(var type in assembly.GetTypes()) {
                if (!type.IsInterface && typeof(IBinaryBundleTest).IsAssignableFrom(type)) {
                    try {
                        var inst = (IBinaryBundleTest)Activator.CreateInstance(type);
                        if (inst.RunTest()) {
                            Debug.Log($"<color=lime>[Passed]</color> {type.Name}");
                        }
                        else {
                            Debug.LogError($"<color=red>[Failed]</color> {type.Name}");
                        }
                    }
                    catch(Exception e) {
                        Debug.LogError($"<color=red>[Failed]</color> {type.Name} " + e.Message);
                    }
                }
            }
        }
    }
}
