using UnityEngine;
using System.Collections;

public class Worker : MonoBehaviour
{
    public float miningTime = 5f; // Time taken to mine resources
    public int resourceAmount = 100; // Amount of resource collected each trip
    private GameObject targetResource; // The resource the worker is targeting
    private GameObject baseObject; // Reference to the base
    private bool isReturning = false; // Flag to check if worker is returning to base
    private int currentResources = 0; // Current resources collected by the worker

    void Start()
    {
        baseObject = GameObject.FindWithTag("Base"); // Find the base in the scene
    }

    void Update()
    {
        if (targetResource != null && !isReturning)
        {
            MoveToTarget(targetResource.transform.position);
        }
        else if (isReturning)
        {
            MoveToTarget(baseObject.transform.position);
        }
    }

    public void AssignResource(GameObject resource)
    {
        targetResource = resource;
    }

    private void MoveToTarget(Vector3 target)
    {
        // Move the worker towards the target
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 5f);
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            if (isReturning)
            {
                DropOffResources();
            }
            else
            {
                StartCoroutine(MineResource());
            }
        }
    }

    private IEnumerator MineResource()
    {
        // Simulate mining time
        yield return new WaitForSeconds(miningTime);
        currentResources = resourceAmount;
        isReturning = true;
    }

    private void DropOffResources()
    {
        // Add resources to the base
        baseObject.GetComponent<Base>().AddResources(currentResources);
        currentResources = 0;
        isReturning = false;
        targetResource = null;
    }

    public void DropOffEarly()
    {
        if (currentResources > 0)
        {
            isReturning = true;
        }
    }
}