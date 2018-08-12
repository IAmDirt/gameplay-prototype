using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour {

    public Transform target;
    public float SmoothSpeed = 0.125f;
    public Vector3 offset;

    // Update is called once per frame
    void FixedUpdate () 
    {
        Vector3 desiredPosition = target.position + offset;
        //lerp stands for "linear interpolation".
        //her it blends A(transform.positon) with B(desiredposition) by smoothSpeed
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed);
        transform.position = smoothedPosition;


        transform.LookAt(target);
	
	}
}
