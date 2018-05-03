using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {
    private GameObject player;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        // Look at the player: Take player position minus the offset and then make the cannon rotate to that position.
        var playerPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(playerPos - transform.position),360f);
    }


}
