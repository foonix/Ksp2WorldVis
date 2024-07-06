using KSP.Rendering.Planets;
using UnityEngine;

public class BiomeMaskMapOverlay : MonoBehaviour, IPQSOverlay
{
    public Material biomeMaskOverlay;
    [Range(0f, 1f)] public float strength = 0.9f;

    private PQS pqsPlanet;

    private readonly int strengthId = Shader.PropertyToID("_Strength");
    private readonly int overlayTexId = Shader.PropertyToID("_OverlayTexture");
    private readonly int biomeMaskTexId = Shader.PropertyToID("_BiomeMaskTex");

    public Material OverlayMaterial
    {
        get
        {
            biomeMaskOverlay.SetFloat(strengthId, strength);
            return biomeMaskOverlay;
        }
    }

    private void OnEnable()
    {
        pqsPlanet = FindObjectOfType<PQS>();
        pqsPlanet.PQSRenderer.AddOverlay(this);

        biomeMaskOverlay.EnableKeyword("_USE_PQS_BUFFER");
        var biomeMaskTex = pqsPlanet.data.materialSettings.surfaceMaterial.GetTexture(biomeMaskTexId);
        biomeMaskOverlay.SetTexture(overlayTexId, biomeMaskTex);
    }

    private void OnDisable()
    {
        pqsPlanet.PQSRenderer.RemoveOverlay(this);
    }
}
