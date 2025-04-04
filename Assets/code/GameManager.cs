using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> tiles;         // Assign tiles manually in inspector
    public float timeBetweenPrompts = 2f;

    private GameObject currentTile;

    void Start()
    {
        StartCoroutine(GlowSequence());
    }

    IEnumerator GlowSequence()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenPrompts);

            // pick a random tile
            int index = Random.Range(0, tiles.Count);
            currentTile = tiles[index];

            // trigger its glow
            currentTile.GetComponent<GlowingTile>().TriggerGlow();
        }
    }

    public void OnTileClicked(GameObject clickedTile)
    {
        if (clickedTile == currentTile)
        {
            Debug.Log("Correct tile clicked!");
            AudioSource audio = clickedTile.GetComponent<AudioSource>();
            if (audio != null)
                audio.Play();
        }
        else
        {
            Debug.Log("Wrong tile.");
        }
    }
}
