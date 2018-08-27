using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_Move : MonoBehaviour
{

    public Transform target;
    
    //so the fishes dont swim ontop of eachother
    Vector3 targetOffset;

    public float speed = 1.0f;

    //amount object use to turn
    [Range(0.0f, 3.0f)]
    public float FaceSpeed = 2.5f;

    //where to look
    Vector3 dir;


    [Range(0, 5)]
    public float RotateSpeed = 1f;
    [Range(0, 5)]
    public float Radius = 1f;

    private float _angle;

    [HideInInspector]
    public bool movingToNewTarget;

    private void Start()
    {
        float random = Random.Range(-0.2f, 0.6f);
        speed = speed + random;
    }

    void Update()
    {
        if (target != null)
        {
            // rotate fish towards target
            Quaternion rot = Quaternion.LookRotation(dir);
            // slerp to the desired rotation over time
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, FaceSpeed * Time.deltaTime);
        }

        if (movingToNewTarget)
        {
            SwimToTarget();

            Vector3 diff = targetOffset - transform.position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < 1.0f)
                movingToNewTarget = false;
        }
        else
            SwimAroundTarget();
    }

    void SwimToTarget()
    {
        dir = targetOffset - transform.position;

        float step = speed * Time.deltaTime;
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, step);
        Vector3 MovingTo = Vector3.MoveTowards(transform.position, targetOffset, Time.deltaTime * speed);
        transform.position = MovingTo;
    }

    void SwimAroundTarget()
    {
        Vector3 MovingTo = Vector3.MoveTowards(transform.position, calculateCircle(0.0f), Time.deltaTime * speed);
        transform.position = MovingTo;

        //direction to face
        dir = calculateCircle(1f) - transform.position;
    }

    Vector3 offset;
    Vector3 calculateCircle(float faceOffset)
    {
        _angle += 0.4f * Time.deltaTime;
        offset = new Vector3(Mathf.Sin(_angle + faceOffset),  0.0f, Mathf.Cos(_angle + faceOffset)) * Radius;

        Vector3 MovingTo = offset + targetOffset;


        return MovingTo;
    }

    public void NewTargetOffset()
    {
        targetOffset = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f));
        targetOffset += target.position;
    }

    //draw interacting sphere
    private void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            //draw line between target and transform
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, targetOffset);
        }
    }
}
