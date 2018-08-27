using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_School : MonoBehaviour
{

    [Range(10.0f, 60.0f)]
    public float MovingRadius = 20.0f;

    public GameObject Fish;

    public List<GameObject> currentFishes = new List<GameObject>();

    public bool getNewTarget;

    //where the fish swims to
    public Transform WhereToSwim;

    private void Update()
    {
        if (getNewTarget)
            GetNewTarget();
    }
    private void Awake()
    {
        SpawnFish();
    }

    private void GetNewTarget()
    {
        WhereToSwim.position =  Random.insideUnitSphere * MovingRadius + transform.position ;

        foreach (GameObject go in currentFishes)
        {
            go.GetComponent<Fish_Move>().movingToNewTarget = true;
            go.GetComponent<Fish_Move>().NewTargetOffset();
        }
        getNewTarget = false;
    }
    void SpawnFish()
    {
        int AmountToSpawn = Random.Range(8, 10);
        for (int x = 0; x < AmountToSpawn; x++)
        {
            var SpawnedFish = Instantiate(Fish, transform);

            SpawnedFish.GetComponent<Fish_Move>().target = WhereToSwim;
            SpawnedFish.GetComponent<Fish_Move>().NewTargetOffset();
            currentFishes.Add(SpawnedFish);
        }
    }
    //draw interacting sphere
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, MovingRadius);
    }
}
