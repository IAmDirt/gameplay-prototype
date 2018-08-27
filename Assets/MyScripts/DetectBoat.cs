using UnityEngine;

public class DetectBoat : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Boat")
        {

        }
    }
}