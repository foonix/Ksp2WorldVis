using KSP.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WorldVis.Editor
{
    public class SoCreators
    {
        [MenuItem("Assets/Create/ScriptableObjects/GameStatesConfiguration")]
        private static void CreateGameStatesConfiguration()
        {
            var asset = ScriptableObject.CreateInstance<GameStatesConfiguration>();

            AssetDatabase.CreateAsset(asset, "Assets/GameStatesConfiguration.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }

        // AkWwiseInitializationSettings
        [MenuItem("Assets/Create/ScriptableObjects/AkWwiseInitializationSettings")]
        private static void CreateAkWwiseInitializationSettings()
        {
            var asset = ScriptableObject.CreateInstance<AkWwiseInitializationSettings>();

            AssetDatabase.CreateAsset(asset, "Assets/AkWwiseInitializationSettings.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }

        // ShapesAssets
        [MenuItem("Assets/Create/ScriptableObjects/ShapesAssets")]
        private static void CreateShapesAssets()
        {
            var asset = ScriptableObject.CreateInstance<Shapes.ShapesAssets>();

            AssetDatabase.CreateAsset(asset, "Assets/Resources/Shapes Assets.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}
