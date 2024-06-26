using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Barracks : MonoBehaviour
{
    private GameObject barracksUI; // Reference to the UI panel
    public float unit1Cooldown = 5f; // Cooldown time for unit 1
    public float unit2Cooldown = 7f; // Cooldown time for unit 2
    public GameObject unitType1Prefab; // Assign the unit type 1 prefab in the Inspector
    public GameObject unitType2Prefab; // Assign the unit type 2 prefab in the Inspector
    public Transform spawnPoint; // The point where units will spawn

    private Image unit1FillImage;
    private Image unit2FillImage;
    private Button unit1Button;
    private Button unit2Button;
    private Queue<int> buildQueue = new Queue<int>(); // Queue to manage unit build order
    private bool isBuilding = false; // Flag to check if building is in progress
    private Transform unitsParent; // Parent transform for units

    void Start()
    {
        // Find the UI elements dynamically
        barracksUI = GameObject.Find("BarracksUI");
        if (barracksUI == null)
        {
            Debug.LogError("BarracksUI not found in the scene.");
            return;
        }

        unit1Button = barracksUI.transform.Find("Archer Unit").GetComponent<Button>();
        unit2Button = barracksUI.transform.Find("Mutant Unit").GetComponent<Button>();
        unit1FillImage = unit1Button.transform.Find("Archer Image").GetComponent<Image>();
        unit2FillImage = unit2Button.transform.Find("Mutant Image").GetComponent<Image>();

        unit1Button.onClick.AddListener(() => EnqueueUnit(1));
        unit2Button.onClick.AddListener(() => EnqueueUnit(2));

        unit1FillImage.fillAmount = 0;
        unit2FillImage.fillAmount = 0;

        barracksUI.SetActive(false);

        // Find the "Units" GameObject
        GameObject unitsGameObject = GameObject.Find("Units");
        if (unitsGameObject == null)
        {
            Debug.LogError("Units GameObject not found in the scene.");
            return;
        }
        unitsParent = unitsGameObject.transform;
    }

    void Update()
    {
        if (isBuilding)
        {
            UpdateBuildProgress();
        }

        // Close the Barracks UI when the E key is pressed
        if (Input.GetKeyDown(KeyCode.E) && barracksUI.activeSelf)
        {
            barracksUI.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        barracksUI.SetActive(true);
    }

    private void EnqueueUnit(int unitType)
    {
        buildQueue.Enqueue(unitType);
        if (!isBuilding)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    private IEnumerator ProcessQueue()
    {
        isBuilding = true;

        while (buildQueue.Count > 0)
        {
            int unitType = buildQueue.Dequeue();
            if (unitType == 1)
            {
                unit1Button.interactable = false;
                float timer = unit1Cooldown;
                while (timer > 0)
                {
                    timer -= Time.deltaTime;
                    unit1FillImage.fillAmount = 1 - (timer / unit1Cooldown);
                    yield return null;
                }
                GameObject unit = Instantiate(unitType1Prefab, spawnPoint.position, spawnPoint.rotation);
                unit.transform.SetParent(unitsParent);
                Debug.Log("Unit 1 built");
                unit1FillImage.fillAmount = 0;
                unit1Button.interactable = true;
            }
            else if (unitType == 2)
            {
                unit2Button.interactable = false;
                float timer = unit2Cooldown;
                while (timer > 0)
                {
                    timer -= Time.deltaTime;
                    unit2FillImage.fillAmount = 1 - (timer / unit2Cooldown);
                    yield return null;
                }
                GameObject unit = Instantiate(unitType2Prefab, spawnPoint.position, spawnPoint.rotation);
                unit.transform.SetParent(unitsParent);
                Debug.Log("Unit 2 built");
                unit2FillImage.fillAmount = 0;
                unit2Button.interactable = true;
            }
        }

        isBuilding = false;
    }

    private void UpdateBuildProgress()
    {
        // This function can be used to update the UI or other elements to show build progress
    }

    public void BuildUnitExternally(int unitType)
    {
        EnqueueUnit(unitType);
    }
}