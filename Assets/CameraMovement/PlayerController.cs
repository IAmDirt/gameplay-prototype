using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    //where you can interact with objects
    public Transform InteractingArea;

    [Range(0.0f, 5.0f)]
    public float interactingRadius;

    //how long button is hold
    float timer;

    public float maximumThrow = 1.5f;

    public float minimumThrowlimit = 0.6f;

    //how manny times has e been pressed, befor you throw an object
    public int ePressedAmount;

    //closest to player
    GameObject Closest;

    //picking up
    public GameObject currentlyHolding;

    //list of all interacteble gameobjects in range
    List<GameObject> inRangeGo = new List<GameObject>();

    DialogueManager DialogueManager;

    public bool holdingObject;

    //text is showing on the screan
    public bool diualougeDisplayed;

    private void Start()
    {
        DialogueManager = FindObjectOfType<DialogueManager>();
    }
    void FixedUpdate()
    {
        // calculating the closest interaction GameObject 
        Closest = colsestGo(inRangeGo, InteractingArea);

        durationEPressed();

        //interacting with objects
        if (Input.GetKeyDown(KeyCode.E) && Closest != null)
        {
            //calculating if "Closest" is within interaction radius
            Vector3 diff = Closest.transform.position - InteractingArea.position;
            float curDistance = diff.sqrMagnitude;

            Interacteble interacteble = Closest.GetComponent<Interacteble>();

            //continue dialouge if dialouge is showing
            if (diualougeDisplayed == true)
            {
                DialogueManager.DisplayNextSentence();
            }
            //else check to interact with object in radius
            else if (curDistance < interactingRadius && interacteble != null || holdingObject == true)
            {
                interacteble.Interact();

                //if object is a text interaction
                if (interacteble.InteractionType == Interacteble.typeOfInteraction.TextInteraction)
                {
                    Dialogue dialogue = interacteble.text.dialogue;
                    if (dialogue != null)
                    {
                        diualougeDisplayed = true;
                        DialogueManager.StartDialogue(dialogue);
                    }
                    else
                        Debug.LogWarning("missing Dialouge from " + interacteble.name);
                }
                //check what type of interaction it is
                //if object can be picked up
                else if (interacteble.InteractionType == Interacteble.typeOfInteraction.pickUp)
                {
                    if (!holdingObject)
                    {
                        currentlyHolding = Closest;
                        holdingObject = true;
                        //pick up object
                    }
                    ePressedAmount++;
                }
                else if (interacteble.InteractionType == Interacteble.typeOfInteraction.Activate)
                {

                }
            }
        }

        if (Input.GetKeyUp(KeyCode.E) && Closest != null)
        {
            if (timer > minimumThrowlimit)
                TrowObject();
            else if (ePressedAmount > 1)
                TrowObject();

            timer = 0;
        }

        //this is whats picking up the objces/dropping it
        if (holdingObject)
            PickUp();

    }
    private void OnTriggerStay(Collider other)
    {
        Interacteble interacteble = other.GetComponent<Interacteble>();
        if (interacteble != null && !inRangeGo.Contains(other.gameObject))
        {
            inRangeGo.Add(other.gameObject);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        inRangeGo.Remove(other.gameObject);
    }

    //get closest object
    public GameObject colsestGo(List<GameObject> Go, Transform trans)
    {
        GameObject closest;
        float distance = 1;
        closest = null;

        distance = Mathf.Infinity;
        Vector3 position = trans.position;
        foreach (GameObject go in Go)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    //draw interacting sphere
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(InteractingArea.position, interactingRadius);
    }

    float SmoothSpeed = 0.5f;
    void PickUp()
    {
        //make object kinematic
        Rigidbody rb = currentlyHolding.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        //disable box colider
        Collider col = currentlyHolding.GetComponent<Collider>();
        col.enabled = false;

        //if you are going to throw "X" move "x" bacwards as antisipation
        float antisipationPower = 0.0f;
        Vector3 Antisipation = new Vector3(0.0f, 0.0f, 0.0f);
        if (timer > minimumThrowlimit)
        {

            antisipationPower = (1.0f * timer - minimumThrowlimit) /3.3f;
            Antisipation = transform.forward * -antisipationPower;
        }
        
        //make object follow you smoothly
        Vector3 Offset = new Vector3(0.0f, 0.95f, 0.0f) + Antisipation;
        Vector3 smoothedPosition = Vector3.Lerp(currentlyHolding.transform.position, transform.position + Offset, SmoothSpeed);
        currentlyHolding.transform.position = smoothedPosition;
    }
    void TrowObject()
    {
        //turn of kinematic
        Rigidbody rb = currentlyHolding.GetComponent<Rigidbody>();
        rb.isKinematic = false;

        //enable box colider
        Collider col = currentlyHolding.GetComponent<Collider>();
        col.enabled = true;

        //if you hold down the e under 0.5 sec. you drop it instead.
        if (timer < minimumThrowlimit)
            timer = 0.5f;

        //throw object forward
        rb.AddForce(transform.forward * 100 * timer);

        //prepare for new objcet by reseting values
        holdingObject = false;
        currentlyHolding = null;
        ePressedAmount = 0;

    }
    void durationEPressed()
    {
        //calculate how long you have hold down the e button up to "maximumThrow"
        if (Input.GetKey(KeyCode.E) && Closest != null)
            if (timer < maximumThrow)
                timer += Time.deltaTime;
    }
}