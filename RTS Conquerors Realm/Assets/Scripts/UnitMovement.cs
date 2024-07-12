using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    private NavMeshAgent navAgent;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 position)
    {
        navAgent.SetDestination(position);
    }
}