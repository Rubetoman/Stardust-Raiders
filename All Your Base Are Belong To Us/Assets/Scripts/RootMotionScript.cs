using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnAnimatorMove()
    {
        Animator anim = GetComponent<Animator>();
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // APPLY DEFAULT ROOT MOTION, ONLY WHEN IN THESE ANIMATION STATES
        if (stateInfo.fullPathHash == Animator.StringToHash("Idle"))
        {

            anim.ApplyBuiltinRootMotion();
        }
    }
}
