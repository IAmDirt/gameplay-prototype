using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interacteble : MonoBehaviour {



public enum typeOfInteraction{
        TextInteraction,
        pickUp,
        Activate
    }
    public typeOfInteraction InteractionType;

    [System.Serializable]
    public class Text
    {
        //Dialogue is from its own class 
        public Dialogue dialogue;
    }
    public Text text;

    [System.Serializable]
    public class PickUp
    {
    }
    public PickUp pickUp;

    [System.Serializable]
    public class Interacting
    {
    }
    public Interacting interacting;


    public virtual void Interact()
    {
        Debug.Log("you interacted with " + transform.name);
    }

}
