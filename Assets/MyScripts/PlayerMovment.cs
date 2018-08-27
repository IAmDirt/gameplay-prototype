using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerStatus;


public class PlayerMovment : MonoBehaviour
{

    public Transform boat;
    public float moveSpeed;         //How fast the character moves
    Rigidbody controller;    //Rigidbody to move
    float smooth = 10f;             //Turn smoothing
    float tiltAngle;                //The angle at which to tilt the character
    float offset;

    Quaternion characterRotation;    //Stores the characters rotation

    float horizontal;
    float vertical;

    void Start()
    {
        controller = GetComponent<Rigidbody>();
        Controll.IsFirstPerson = true;
    }

    void FixedUpdate()
    {
        //movment controlls
        if (Controll.IsFirstPerson == true)
        {
            //Receives user input
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            //Controls change based on camera position (This is for debugging purposes)
            Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
            forward.y = 0;
            forward = forward.normalized;

            Vector3 right = new Vector3(forward.z, 0, -forward.x);

            //The direction the player is moving
            Vector3 inputVec = horizontal * right + vertical * forward;
            inputVec *= moveSpeed;

            //Move the controller in that direction
            controller.MovePosition(transform.position + (inputVec) * Time.deltaTime);

            //Determines the level of tilt based on user input
            if (Mathf.Abs(vertical) > 0 && Mathf.Abs(horizontal) > 0)
            {
                tiltAngle = 0.5f;
            }
            else
            {
                tiltAngle = 1f;
            }

            //Calculates the tilt based on user input
            Quaternion target = Quaternion.Euler((Mathf.Abs(vertical) + Mathf.Abs(horizontal)) * tiltAngle, 0, 0);
            if (inputVec != Vector3.zero)
            {
                //Smoothing rotates the character and assigns the new rotation to CharacterRotation
                characterRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputVec, Vector3.up), Time.deltaTime * smooth);
                transform.rotation = characterRotation * target;
            }



            //cange from boat to player controlls
            if (Input.GetKey(KeyCode.Space))
            {
                Camera camera = Camera.main;

                if (Controll.IsFirstPerson == true)
                {
                    Controll.IsFirstPerson = false;
                    camera.GetComponent<SmoothFollow>().target = boat;
}
                else
                {
                    Controll.IsFirstPerson = true;
                    camera.GetComponent<SmoothFollow>().target = transform;
                }
            }
        }
    }
}
