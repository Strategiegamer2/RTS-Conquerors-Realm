using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Barracks : MonoBehaviour
{
    public GameObject unitType1Prefab; // Assign the first unit prefab in the Inspector
    public GameObject unitType2Prefab; // Assign the second unit prefab in the Inspector
    public Transform spawnPoint; // The point where units will spawn
    public float buildTimeType1 = 5f; // Time to build unit type 1
    public float buildTimeType2 = 7f; // Time to build unit type 2

    public GameObject queueImagePrefab; // Assign the queue image prefab in the Inspector
    public Transform queuePanel; // Assign the queue panel (UI element) in the Inspector

    private Queue<BuildQueueItem> buildQueue = new Queue<BuildQueueItem>();
    private bool isBuilding = false;

    void Start()
    {
        if (queuePanel == null)
        {
            Debug.LogError("Queue Panel is not assigned in the Inspector.");
        }
    }

    public void BuildUnit(int unitType)
    {
        GameObject unitPrefab = null;
        float buildTime = 0f;

        if (unitType == 1)
        {
            unitPrefab = unitType1Prefab;
            buildTime = buildTimeType1;
        }
        else if (unitType == 2)
        {
            unitPrefab = unitType2Prefab;
            buildTime = buildTimeType2;
        }

        if (unitPrefab != null)
        {
            var buildQueueItem = new BuildQueueItem(unitPrefab, buildTime);
            buildQueue.Enqueue(buildQueueItem);
            if (!isBuilding)
            {
                StartCoroutine(HandleBuildQueue());
            }

            // Add to UI Queue
            GameObject queueImageObj = Instantiate(queueImagePrefab, queuePanel);
            QueueImage queueImage = queueImageObj.GetComponent<QueueImage>();
            queueImage.Initialize(buildTime);
            buildQueueItem.QueueImage = queueImage;
        }
    }

    private IEnumerator HandleBuildQueue()
    {
        isBuilding = true;
        while (buildQueue.Count > 0)
        {
            BuildQueueItem currentItem = buildQueue.Dequeue();
            yield return StartCoroutine(BuildUnitCoroutine(currentItem));
        }
        isBuilding = false;
    }

    private IEnumerator BuildUnitCoroutine(BuildQueueItem queueItem)
    {
        QueueImage queueImage = queueItem.QueueImage;
        queueImage.Initialize(queueItem.BuildTime);

        yield return new WaitForSeconds(queueItem.BuildTime);
        Destroy(queueImage.gameObject);
        Instantiate(queueItem.UnitPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    private class BuildQueueItem
    {
        public GameObject UnitPrefab { get; private set; }
        public float BuildTime { get; private set; }
        public QueueImage QueueImage { get; set; }

        public BuildQueueItem(GameObject unitPrefab, float buildTime)
        {
            UnitPrefab = unitPrefab;
            BuildTime = buildTime;
        }
    }
}
