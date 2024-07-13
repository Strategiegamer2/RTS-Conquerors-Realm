using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class WorkerUIManager : MonoBehaviour
{
    public static WorkerUIManager instance;

    public GameObject workerUIPanel;
    public Button barracksButton;
    public Button houseButton;

    public Base playerBase;
    public BuildingPlacer buildingPlacer;
    public Tooltip tooltip;

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

        AddTooltipListener(barracksButton, "Barracks: Costs 500 Wood, 500 Niter, and 200 Stone.");
        AddTooltipListener(houseButton, "House: Costs 100 Wood, and 200 Stone.");
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

    private void AddTooltipListener(Button button, string message)
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

        // OnPointerEnter
        var pointerEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        pointerEnter.callback.AddListener((e) => tooltip.ShowTooltip(message));
        trigger.triggers.Add(pointerEnter);

        // OnPointerExit
        var pointerExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        pointerExit.callback.AddListener((e) => tooltip.HideTooltip());
        trigger.triggers.Add(pointerExit);
    }
}
