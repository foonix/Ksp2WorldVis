using KSP.Assets;
using KSP.Game;
using KSP.Rendering;
using KSP.Rendering.impl;
using KSP.Rendering.Planets;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace WorldVis
{


    [RequireComponent(typeof(AssetProvider))]
    public class PqsVisSetup : MonoBehaviour
    {
        [Header("In-scene dependencies")]
        public AssetProvider assetProvider;
        public PrevisCameraManager previsCameraManager;
        public PrevisGameInstance previsGameInstance;
        public Transform spawnInactive;

        [Header("Things spawned when entering playmode")]
        public PQS pqsPlanet;
        public GameObject pqsScaledPlanet;
        public DebugBiomeUIManager debugBiomeUIManager;

        [Header("Addressable load paths")]
        public string pqsPlanetToSpawn = "Celestial.Kerbin.Simulation.prefab";
        public string pqsScaledPlanetToSpawn = "Celestial.Kerbin.Scaled.prefab";
        public string flightCameraAssembly_Physics = "Assets/Scripts/Simulation Scripts/View/Cameras/prefabs/FlightCameraAssembly_Physics.prefab";
        public string flightCameraAssembly_Scaled = "Assets/Scripts/Simulation Scripts/View/Cameras/prefabs/FlightCameraAssembly_Scaled.prefab";
        public string graphicsManagerPath = "Graphics Manager.prefab";

        [Header("Debug Info scraping")]
        public bool enableDebugScraping;
        public Texture observerCubemap;
        public PQSRenderer pqsRenderer;
        public uint[] pqsRendererDrawArgsBuffer = new uint[4];

        [Header("PQS debugging options")]
        public bool debugShowBiomeColor = false;
        [Tooltip("Show only PQS quads with the given RGBA values in the biome texture.")]
        public bool debugBiomeEnabled = false;
        // needs bitmask editor layout
        public DebugBiome debugBiomeSetting = DebugBiome.All;
        public bool debugTriplanarEnabled = false;
        public DebugTriplanar debugTriplanarSetting = DebugTriplanar.All;

        private GraphicsManager graphicsManager;
        private CubemapReflectionSystem cubemapReflectionSystem;

        private IEnumerator Start()
        {
            SceneManager.CreateScene("auxPhysicsScene");

            Util.LoadGameBurstCode();
            yield return null;

            previsGameInstance.SetProperty("Assets", assetProvider);

            var gmPrefab = LoadPrefabImmediate<GameObject>(graphicsManagerPath);
            graphicsManager = Instantiate(gmPrefab, previsGameInstance.transform).GetComponent<GraphicsManager>();

            // This stops it from trying to register for evens from GameManager.Instance, which will be null.
            graphicsManager.celestialBodyPrevisScene = true;
            graphicsManager.PrevisGame = previsGameInstance;
            previsGameInstance.GraphicsManager = graphicsManager;

            // This has prefabs and materials that drive PQS.
            previsCameraManager.GlobalSettings = graphicsManager.PQSGlobalSettings;

            // can't link these directly to the prefab in addressables.
            var pqsPlanetPrefab = LoadPrefabImmediate<GameObject>(pqsPlanetToSpawn);
            pqsPlanet = Instantiate(pqsPlanetPrefab, previsGameInstance.transform).GetComponent<PQS>();
            pqsScaledPlanet = LoadPrefabImmediate<GameObject>(pqsScaledPlanetToSpawn);
            Instantiate(pqsScaledPlanet, previsGameInstance.transform);
            var kerbolPrefab = LoadPrefabImmediate<GameObject>("Celestial.Kerbol.Scaled.prefab");

            previsCameraManager.sunPrefab = kerbolPrefab;

            // This seems to be related to PQSRenderer.DebugShowBiomeColor, which
            // enables the keyword DEBUG_OUTPUT_BIOME_COLOR for CelestialBody_Local
            // But.. it doesn't seem to work.  The variant may be stripped or some other
            // property needs to be set.  It may be possible to implement this with an IPQSOverlay.
            var biomDebugUi = LoadPrefabImmediate<GameObject>("BiomeViewDebugCanvas.prefab");
            debugBiomeUIManager = Instantiate(biomDebugUi, previsGameInstance.transform).GetComponent<DebugBiomeUIManager>();
            debugBiomeUIManager.goUIRoot.SetActive(false);

            previsCameraManager.scaledPrefab = LoadPrefabImmediate<GameObject>(flightCameraAssembly_Scaled)
                .GetComponent<FlightCameraRenderStack_Scaled>();
            previsCameraManager.physicsPrefab = LoadPrefabImmediate<GameObject>(flightCameraAssembly_Physics)
                .GetComponent<FlightCameraRenderStack_Physics>();

            // SkyboxManager spews NREs, and from the code I can't see how it would be initialized
            // correctly in previs.  Nulling the ref in LightingSystem will stop it from getting IUpdate.OnUpdate().
            var lightingSystem = graphicsManager.gameObject.GetComponentInChildren<LightingSystem>();
            lightingSystem.SkyboxManager = null;

            // Can't control Awake() order by awaking specific hierarchies.
            // They use way too much FindObjectOfType(false) out of Awake(), which will miss inactives.
            // We can't control Awake() order either, because that's saved in the script metadata.
            // So the only thing I can think to do is find the objects for them, inject them, then Awake.

            yield return null;

            previsGameInstance.gameObject.SetActive(true);

            yield return null;

            pqsRenderer = FindObjectOfType<PQSRenderer>();
        }

        private T LoadPrefabImmediate<T>(string addressablePath)
        {
            var prefab = Addressables.LoadAssetAsync<T>(addressablePath);
            prefab.WaitForCompletion();
            return prefab.Result;
        }

        private void Update()
        {
            if (enableDebugScraping)
            {
                cubemapReflectionSystem = FindObjectOfType<CubemapReflectionSystem>();

                observerCubemap = cubemapReflectionSystem?.GetObserverCubemapTexture();
                var argsBuffer = pqsRenderer.GetField<PQSRenderer, ComputeBuffer>("_drawArgsBuffer");

                argsBuffer.GetData(pqsRendererDrawArgsBuffer);
            }

            if (debugBiomeUIManager != null && pqsRenderer != null
                && debugShowBiomeColor != debugBiomeUIManager.goUIRoot.activeSelf)
            {
                // This is the only thing I can find that triggers setup of DebugBiomeUIManager,
                // and it has no callers.  It indirectly toggles goUIRoot.activeSelf.
                pqsRenderer.CallPrivateVoidMethod("OnToggleBiomeDebugView");
            }

            PQSRenderer.DebugBiome = debugBiomeEnabled ? debugBiomeSetting : DebugBiome.All;
            PQSRenderer.DebugTriplanar = debugTriplanarEnabled ? debugTriplanarSetting : DebugTriplanar.All;
            PQSRenderer.DebugShowBiomeColor = debugShowBiomeColor;
        }
    }
}