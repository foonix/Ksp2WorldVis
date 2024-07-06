using KSP.Rendering.Planets;
using UnityEngine;

namespace WorldVis
{

    public class PqsOverlay : MonoBehaviour, IPQSOverlay
    {
        public Material overlayMaterial;
        [Range(0f, 1f)] public float strength = 0.9f;

        public enum OverlayMode
        {
            BiomeMask,
            SubZoneMask,
        }
        public OverlayMode mode;

        private PQS pqsPlanet;

        private readonly int strengthId = Shader.PropertyToID("_Strength");
        private readonly int overlayTexId = Shader.PropertyToID("_OverlayTexture");
        private readonly int biomeMaskTexId = Shader.PropertyToID("_BiomeMaskTex");

        public Material OverlayMaterial
        {
            get
            {
                var texture = mode switch
                {
                    OverlayMode.BiomeMask => pqsPlanet.data.heightMapInfo.mask,
                    OverlayMode.SubZoneMask => pqsPlanet.data.heightMapInfo.subZoneMask,
                    _ => Texture2D.blackTexture,
                };
                overlayMaterial.SetTexture(overlayTexId, texture);
                overlayMaterial.SetFloat(strengthId, strength);
                return overlayMaterial;
            }
        }

        private void OnEnable()
        {
            pqsPlanet = FindObjectOfType<PQS>();
            pqsPlanet.PQSRenderer.AddOverlay(this);

            overlayMaterial.EnableKeyword("_USE_PQS_BUFFER");
        }

        private void OnDisable()
        {
            pqsPlanet.PQSRenderer.RemoveOverlay(this);
        }
    }
}
