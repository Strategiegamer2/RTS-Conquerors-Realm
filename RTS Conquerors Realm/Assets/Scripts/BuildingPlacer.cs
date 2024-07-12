using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public GameObject barracksPrefab;
    public GameObject housePrefab;
    public Material hologramMaterial;

    private GameObject currentHologram;
    private GameObject currentBuildingPrefab;
    private bool isPlacing = false;

    void Update()
    {
        if (isPlacing)
        {
            UpdateHologramPosition();

            if (Input.GetMouseButtonDown(0) && IsValidPlacement())
            {
                PlaceBuilding();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelPlacement();
            }
        }
    }

    public void StartPlacingBuilding(string buildingType)
    {
        if (buildingType == "Barracks")
        {
            currentBuildingPrefab = barracksPrefab;
        }
        else if (buildingType == "House")
        {
            currentBuildingPrefab = housePrefab;
        }

        if (currentBuildingPrefab != null)
        {
            currentHologram = Instantiate(currentBuildingPrefab);
            SetHologramAppearance(currentHologram);
            isPlacing = true;
        }
    }

    private void UpdateHologramPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                currentHologram.transform.position = hit.point;
                currentHologram.SetActive(true);
            }
            else
            {
                currentHologram.SetActive(false);
            }
        }
    }

    private bool IsValidPlacement()
    {
        // Add logic here to check if the placement is valid (e.g., no collisions with other buildings)
        return currentHologram.activeSelf;
    }

    private void PlaceBuilding()
    {
        Instantiate(currentBuildingPrefab, currentHologram.transform.position, currentHologram.transform.rotation);
        Destroy(currentHologram);
        isPlacing = false;
    }

    private void CancelPlacement()
    {
        Destroy(currentHologram);
        isPlacing = false;
    }

    private void SetHologramAppearance(GameObject hologram)
    {
        Renderer[] renderers = hologram.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = hologramMaterial;
        }
    }
}
