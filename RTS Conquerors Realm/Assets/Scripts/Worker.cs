using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Worker : MonoBehaviour
{
    public int resourceAmount = 100; // Amount of resource collected each trip
    public float miningInterval = 1f; // Interval between mining actions
    public int resourcePerInterval = 20; // Amount of resource collected per interval

    private GameObject targetResource; // The resource the worker is targeting
    private GameObject baseObject; // Reference to the base
    private bool isReturning = false; // Flag to check if worker is returning to base
    private int currentResources = 0; // Current resources collected by the worker
    private string resourceType; // Type of resource being collected

    private NavMeshAgent navAgent;
    private float miningTimer = 0f;
    private bool isSelected = false;

    void OnMouseDown()
    {
        isSelected = true;
        WorkerUIManager.instance.ToggleWorkerUI(true);
    }

    void OnMouseUp()
    {
        isSelected = false;
    }

    void OnDestroy()
    {
        if (isSelected)
        {
            WorkerUIManager.instance.ToggleWorkerUI(false);
        }
    }

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        baseObject = GameObject.FindWithTag("Base"); // Find the base in the scene
    }

    void Update()
    {
        if (targetResource != null && !isReturning)
        {
            if (Vector3.Distance(transform.position, targetResource.transform.position) < 2f)
            {
                MineResource();
            }
            else
            {
                MoveToTarget(targetResource.transform.position);
            }
        }
        else if (isReturning)
        {
            Vector3 closestPoint = GetClosestPointOnBounds(baseObject.GetComponent<Collider>(), transform.position);
            if (Vector3.Distance(transform.position, closestPoint) < 2f)
            {
                DropOffResources();
            }
            else
            {
                MoveToTarget(closestPoint);
            }
        }
    }

    public void AssignResource(GameObject resource)
    {
        targetResource = resource;
        isReturning = false;
        resourceType = resource.tag; // Store the type of resource
        MoveToTarget(resource.transform.position);
    }

    private void MoveToTarget(Vector3 target)
    {
        navAgent.SetDestination(target);
    }

    private void MineResource()
    {
        miningTimer += Time.deltaTime;
        if (miningTimer >= miningInterval)
        {
            miningTimer = 0f;
            currentResources += resourcePerInterval;
            Debug.Log("Current Resources: " + currentResources);
            if (currentResources >= resourceAmount)
            {
                Debug.Log("Collected enough resources, returning to drop-off point");
                isReturning = true;
                Vector3 closestPoint = GetClosestPointOnBounds(baseObject.GetComponent<Collider>(), transform.position);
                MoveToTarget(closestPoint);
            }
        }
    }

    private void DropOffResources()
    {
        // Add resources to the base
        baseObject.GetComponent<Base>().AddResources(currentResources, resourceType);
        currentResources = 0;
        isReturning = false;
        MoveToTarget(targetResource.transform.position); // Go back to the resource
    }

    private Vector3 GetClosestPointOnBounds(Collider collider, Vector3 point)
    {
        return collider.ClosestPoint(point);
    }

    public void DropOffEarly()
    {
        if (currentResources > 0)
        {
            isReturning = true;
            Vector3 closestPoint = GetClosestPointOnBounds(baseObject.GetComponent<Collider>(), transform.position);
            MoveToTarget(closestPoint);
        }
    }
}
