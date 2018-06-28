using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to destroy any object on collision exit. Is used by DestroyPlane GameObject.
/// </summary>
public class DestroyOnCollisionExit : MonoBehaviour {

    private void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}
