using UnityEngine;
using UnityEngine.UI;

public class WorkerUIManager : MonoBehaviour
{
    public static WorkerUIManager instance;

    public GameObject workerUIPanel;
    public Button barracksButton;
    public Button houseButton;

    public Base playerBase;
    public BuildingPlacer buildingPlacer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        workerUIPanel.SetActive(false);

        barracksButton.onClick.AddListener(() => BuildBuilding("Barracks"));
        houseButton.onClick.AddListener(() => BuildBuilding("House"));
    }

    public void ToggleWorkerUI(bool isVisible)
    {
        workerUIPanel.SetActive(isVisible);
    }

    private void BuildBuilding(string buildingType)
    {
        if (buildingType == "Barracks")
        {
            if (playerBase.CanAffordBuilding(500, 500, 200))
            {
                playerBase.DeductResources(500, 500, 200);
                buildingPlacer.StartPlacingBuilding("Barracks");
            }
            else
            {
                Debug.Log("Not enough resources to build Barracks");
            }
        }
        else if (buildingType == "House")
        {
            if (playerBase.CanAffordBuilding(100, 200, 0))
            {
                playerBase.DeductResources(100, 200, 0);
                buildingPlacer.StartPlacingBuilding("House");
            }
            else
            {
                Debug.Log("Not enough resources to build House");
            }
        }
    }
}
