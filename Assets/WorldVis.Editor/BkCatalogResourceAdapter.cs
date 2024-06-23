using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Klm.Unity.Editor
{
    public class BkCatalogResourceAdapter : ResourcesAPI
    {
        private static readonly string catalogBundlePath = "Assets/DoNotDistribute/ksp2.catalog";
        
        private readonly AssetBundle catalogBundle;

        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad()
        {
            overrideAPI = new BkCatalogResourceAdapter();
        }

        protected BkCatalogResourceAdapter()
        {
            catalogBundle = AssetBundle.GetAllLoadedAssetBundles().FirstOrDefault(bnd => bnd.name.Equals("ksp2"));

            if (!catalogBundle)
            {
                if (!File.Exists(catalogBundlePath))
                {
                    Debug.LogWarning($"Could not find {catalogBundlePath}."
                        + "Please run the ThunderKit importer, and then run the import job "
                        + "at Assets/ThunderKitSettings/Pipelines/ImportKsp2ToEditor to create this file.");
                    return;
                }

                Debug.Log("Loading BundleKit Catalog");
                catalogBundle = AssetBundle.LoadFromFile(catalogBundlePath);
                foreach (var shader in catalogBundle.LoadAllAssets<Shader>())
                {
                    if (!Shader.Find(shader.name))
                    {
                        ShaderUtil.RegisterShader(shader);
                    }
                }
            }
        }

        protected override UnityEngine.Object Load(string path, Type systemTypeInstance)
        {
            // Try Unity first
            UnityEngine.Object asset = base.Load(path, systemTypeInstance);

            // Avoid errors if assets loading is attempted right before domain reload, which can unload the bundle.
            if (asset == null && catalogBundle)
            {
                asset = catalogBundle.LoadAsset(path, systemTypeInstance);
            }

            return asset;
        }
    }
}
