using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingController : MonoBehaviour {

    public Transform center;
    public float rotationSpeed = 10.0f;
    public GameObject[] flickers;
    public Material[] materials;
    public float flickFrequency = 1.0f;

    private float timer = 0.0f;
    private Vector3 centralPoint;
	// Use this for initialization
	void Start () {
        centralPoint = new Vector3(center.position.x, center.position.y, center.position.z);
    }
	
	// Update is called once per frame
	void Update () {

        Rotate();
        ChangeMaterial();
        
    }

    private void Rotate()
    {
        transform.RotateAround(centralPoint, center.forward, rotationSpeed * Time.deltaTime);
    }

    private void ChangeMaterial()
    {
        foreach (Material m in materials)
        {
            timer += Time.deltaTime;
            if (timer > flickFrequency)
            {
                foreach (GameObject f in flickers)
                {
                    if(f != null)
                        f.GetComponent<Renderer>().material = m;
                }
                timer = 0.0f;
            }
        }
    }
}
