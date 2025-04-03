using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> tiles;         // list of piano tile cubes
    public float timeBetweenGlows = 2f;    // time between each glow

    private GameObject currentTile;        // currently glowing tile

    void Start()
    {
        if (tiles.Count == 0)
        {
            Debug.LogError("No tiles assigned in the GameManager!");
            return;
        }

        StartCoroutine(GlowTilesSequence());
    }

    IEnumerator GlowTilesSequence()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenGlows);

            // pick a random tile
            int index = Random.Range(0, tiles.Count);
            currentTile = tiles[index];

            // trigger the glow effect
            currentTile.GetComponent<GlowingTile>().TriggerGlow();

            // optional: add sound here if you want to auto-play it with glow
            // AudioSource audio = currentTile.GetComponent<AudioSource>();
            // if (audio) audio.Play();
        }
    }

    // call this when player taps a tile
    public void OnTileClicked(GameObject clickedTile)
    {
        if (clickedTile == currentTile)
        {
            Debug.Log("Correct tile!");
            // play note or increase score here
        }
        else
        {
            Debug.Log("Wrong tile.");
            // maybe end game or deduct points
        }
    }
}
