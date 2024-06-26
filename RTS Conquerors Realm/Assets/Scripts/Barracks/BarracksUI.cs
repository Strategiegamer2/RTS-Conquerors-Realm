using UnityEngine;

public class BarracksUI : MonoBehaviour
{
    public GameObject barracks; // Assign the barracks building in the Inspector
    public GameObject barracksUI; // Assign the UI panel in the Inspector

    private Barracks barracksScript;

    void Start()
    {
        barracksScript = barracks.GetComponent<Barracks>();
        barracksUI.SetActive(false);
    }

    void Update()
    {
        // Close the Barracks UI when the E key is pressed
        if (Input.GetKeyDown(KeyCode.E) && barracksUI.activeSelf)
        {
            barracksUI.SetActive(false);
        }
    }

    public void ShowBarracksUI()
    {
        barracksUI.SetActive(true);
    }

    public void HideBarracksUI()
    {
        barracksUI.SetActive(false);
    }

    public void BuildUnit1()
    {
        barracksScript.BuildUnitExternally(1);
    }

    public void BuildUnit2()
    {
        barracksScript.BuildUnitExternally(2);
    }
}
