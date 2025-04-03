using UnityEngine;

public class GlowingTile : MonoBehaviour
{
    public Color glowColor = Color.cyan;    // color to glow
    public float glowDuration = 1.0f;       // how long the glow lasts

    private Material tileMaterial;
    private Color originalColor;
    private bool isGlowing = false;

    void Start()
    {
        // get and clone the material so each tile has its own instance
        tileMaterial = GetComponent<Renderer>().material;
        originalColor = tileMaterial.GetColor("_EmissionColor");
        tileMaterial.EnableKeyword("_EMISSION");
    }

    public void TriggerGlow()
    {
        if (!isGlowing)
            StartCoroutine(GlowEffect());
    }

    private System.Collections.IEnumerator GlowEffect()
    {
        isGlowing = true;
        tileMaterial.SetColor("_EmissionColor", glowColor);

        yield return new WaitForSeconds(glowDuration);

        tileMaterial.SetColor("_EmissionColor", originalColor);
        isGlowing = false;
    }
}
