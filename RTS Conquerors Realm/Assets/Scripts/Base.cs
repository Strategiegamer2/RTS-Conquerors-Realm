using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    private GameObject baseUI; // Reference to the UI panel
    private GameObject gameUI; // Reference to the UI panel
    public float workerCooldown = 5f; // Cooldown time for worker
    public GameObject workerPrefab; // Assign the worker prefab in the Inspector
    public Transform spawnPoint; // The point where workers will spawn

    private Image workerFillImage;
    private Button workerButton;
    private float workerTimer = 0f;

    private int wood = 0;
    private int stone = 0;
    private int niter = 0;
    private int talers = 0;
    private int population = 0;
    private int maxPopulation = 10;

    private TextMeshProUGUI woodText;
    private TextMeshProUGUI stoneText;
    private TextMeshProUGUI niterText;
    private TextMeshProUGUI talersText;
    private TextMeshProUGUI populationText;

    void Start()
    {
        GameObject uiParent = GameObject.Find("UI");
        if (uiParent == null)
        {
            Debug.LogError("UI parent not found in the scene.");
            return;
        }

        // Find the BaseUI and GameUI under the UI parent
        baseUI = uiParent.transform.Find("BaseUI").gameObject;
        if (baseUI == null)
        {
            Debug.LogError("BaseUI not found in the scene.");
            return;
        }

        gameUI = uiParent.transform.Find("GameUI").gameObject;
        if (gameUI == null)
        {
            Debug.LogError("GameUI not found in the scene.");
            return;
        }

        // Find the text elements under the Panel in GameUI
        Transform panelTransform = gameUI.transform.Find("Panel");
        if (panelTransform == null)
        {
            Debug.LogError("Panel not found in GameUI.");
            return;
        }

        woodText = panelTransform.Find("Wood Text").GetComponent<TextMeshProUGUI>();
        stoneText = panelTransform.Find("Stone Text").GetComponent<TextMeshProUGUI>();
        niterText = panelTransform.Find("Niter Text").GetComponent<TextMeshProUGUI>();
        talersText = panelTransform.Find("Talers Text").GetComponent<TextMeshProUGUI>();
        populationText = panelTransform.Find("Population Text").GetComponent<TextMeshProUGUI>();

        UpdateResourceUI();

        workerButton = baseUI.transform.Find("Worker Button").GetComponent<Button>();
        workerFillImage = workerButton.transform.Find("Worker Image").GetComponent<Image>();

        workerButton.onClick.AddListener(() => StartCoroutine(BuildWorker()));

        workerFillImage.fillAmount = 0;

        baseUI.SetActive(false);
    }

    void OnMouseDown()
    {
        baseUI.SetActive(true);
    }

    void Update()
    {
        if (workerTimer > 0)
        {
            workerTimer -= Time.deltaTime;
            workerFillImage.fillAmount = (workerCooldown - workerTimer) / workerCooldown;
            if (workerTimer <= 0)
            {
                workerButton.interactable = true;
                workerFillImage.fillAmount = 1;
            }
        }

        // Close Base UI when the E key is pressed
        if (Input.GetKeyDown(KeyCode.E) && baseUI.activeSelf)
        {
            baseUI.SetActive(false);
        }
    }

    private IEnumerator BuildWorker()
    {
        if (talers >= 50 && population < maxPopulation)
        {
            talers -= 50;
            UpdateResourceUI();

            workerButton.interactable = false;
            workerTimer = workerCooldown;
            workerFillImage.fillAmount = 0;
            yield return new WaitForSeconds(workerCooldown);
            GameObject worker = Instantiate(workerPrefab, spawnPoint.position, spawnPoint.rotation);
            worker.transform.SetParent(GameObject.Find("Units").transform);
            Debug.Log("Worker built");
        }
        else
        {
            Debug.Log("Not enough Talers or population limit reached to build a worker.");
        }
    }

    public void AddResources(int amount, string resourceType)
    {
        if (resourceType == "Wood")
        {
            wood += amount;
            Debug.Log("Wood added: " + amount + ". Total wood: " + wood);
        }
        else if (resourceType == "Stone")
        {
            stone += amount;
            Debug.Log("Stone added: " + amount + ". Total stone: " + stone);
        }
        else if (resourceType == "Niter")
        {
            niter += amount;
            Debug.Log("Niter added: " + amount + ". Total niter: " + niter);
        }
        else if (resourceType == "Talers")
        {
            talers += amount;
            Debug.Log("Talers added: " + amount + ". Total talers: " + talers);
        }
        UpdateResourceUI();
    }

    public int GetResourceAmount(string resourceType)
    {
        if (resourceType == "Wood")
        {
            return wood;
        }
        else if (resourceType == "Stone")
        {
            return stone;
        }
        else if (resourceType == "Niter")
        {
            return niter;
        }
        else if (resourceType == "Talers")
        {
            return talers;
        }
        return 0;
    }

    public bool CanAffordBuilding(int woodCost, int stoneCost, int niterCost)
    {
        return wood >= woodCost && stone >= stoneCost && niter >= niterCost;
    }

    public void DeductResources(int woodCost, int stoneCost, int niterCost)
    {
        wood -= woodCost;
        stone -= stoneCost;
        niter -= niterCost;
        UpdateResourceUI();
    }

    public void AddPopulation(int amount)
    {
        maxPopulation += amount;
        UpdateResourceUI();
    }

    private void UpdateResourceUI()
    {
        // Update the UI with the new resource values
        woodText.text = "Wood: " + wood;
        stoneText.text = "Stone: " + stone;
        niterText.text = "Niter: " + niter;
        talersText.text = "Talers: " + talers;
        populationText.text = "Population: " + population + "/" + maxPopulation;
    }
}


