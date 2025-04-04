using UnityEngine;

public class GlowingTile : MonoBehaviour
{
    public Color glowColor = Color.cyan;
    public float glowDuration = 0.5f;
    public float fadeDuration = 0.5f;

    private Material tileMaterial;
    private Color originalColor;
    private bool isGlowing = false;

    void Start()
    {
        tileMaterial = GetComponent<Renderer>().material;
        originalColor = tileMaterial.color;
    }

    public void TriggerGlow()
    {
        if (!isGlowing)
            StartCoroutine(GlowEffect());
    }

    private System.Collections.IEnumerator GlowEffect()
    {
        isGlowing = true;
        tileMaterial.color = glowColor;

        yield return new WaitForSeconds(glowDuration);

        float t = 0;
        while (t < fadeDuration)
        {
            tileMaterial.color = Color.Lerp(glowColor, originalColor, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }

        tileMaterial.color = originalColor;
        isGlowing = false;
    }
}
