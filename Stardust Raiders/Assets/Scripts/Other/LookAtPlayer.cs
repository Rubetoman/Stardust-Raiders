using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to keep any GameObject pointing at Player GameObject. Is executed in Edit mode also.
/// </summary>
[ExecuteInEditMode]
public class LookAtPlayer : MonoBehaviour {

    private GameObject player;  // Player GameObject.

    void Start () {
        player = GameManager.Instance.player;   // Get Player.
    }
	
	void Update () {
        if (player == null)     // Avoid executing code if player variable is null.
        {
            Debug.LogWarning("Player couldn't be found. Searching again...");
            player = GameManager.Instance.player;    // Set player variable.
            if (player != null)
                Debug.LogWarning("Player found!");
            else
                return;
        }
        // Look at the player: Take player position and then make the gameObject rotate towards that position.
        var playerPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(playerPos - transform.position),360f);
    }
}
