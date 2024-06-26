using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    private GameObject baseUI; // Reference to the UI panel
    public float workerCooldown = 5f; // Cooldown time for worker
    public GameObject workerPrefab; // Assign the worker prefab in the Inspector
    public Transform spawnPoint; // The point where workers will spawn

    private Image workerFillImage;
    private Button workerButton;
    private float workerTimer = 0f;

    private int wood = 0;
    private int crystals = 0;

    void Start()
    {
        // Find the UI elements dynamically
        baseUI = GameObject.Find("BaseUI");
        if (baseUI == null)
        {
            Debug.LogError("BaseUI not found in the scene.");
            return;
        }

        workerButton = baseUI.transform.Find("Worker Button").GetComponent<Button>();
        workerFillImage = workerButton.transform.Find("Worker Image").GetComponent<Image>();

        workerButton.onClick.AddListener(() => StartCoroutine(BuildWorker()));

        workerFillImage.fillAmount = 0;

        baseUI.SetActive(false);
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

        // Open Base UI when the base is clicked
        if (Input.GetMouseButtonDown(0) && IsMouseOverBase())
        {
            baseUI.SetActive(true);
        }

        // Close Base UI when the E key is pressed
        if (Input.GetKeyDown(KeyCode.E) && baseUI.activeSelf)
        {
            baseUI.SetActive(false);
        }
    }

    private IEnumerator BuildWorker()
    {
        workerButton.interactable = false;
        workerTimer = workerCooldown;
        workerFillImage.fillAmount = 0;
        yield return new WaitForSeconds(workerCooldown);
        GameObject worker = Instantiate(workerPrefab, spawnPoint.position, spawnPoint.rotation);
        worker.transform.SetParent(GameObject.Find("Units").transform);
        Debug.Log("Worker built");
    }

    private bool IsMouseOverBase()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject == gameObject;
        }
        return false;
    }

    public void AddResources(int amount)
    {
        // Assuming we are collecting both resources at the same rate
        wood += amount / 2;
        crystals += amount / 2;
        UpdateResourceUI();
    }

    private void UpdateResourceUI()
    {
        // Update the UI with the new resource values
        baseUI.transform.Find("Wood Text").GetComponent<Text>().text = "Wood: " + wood;
        baseUI.transform.Find("Crystals Text").GetComponent<Text>().text = "Crystals: " + crystals;
    }
}