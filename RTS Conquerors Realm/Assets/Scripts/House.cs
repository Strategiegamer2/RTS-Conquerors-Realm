using UnityEngine;
using System.Collections;

public class House : MonoBehaviour
{
    public int populationIncrease = 5;
    public int talersIncome = 100;
    public float incomeInterval = 120f; // 2 minutes

    private Base playerBase;

    void Start()
    {
        playerBase = FindObjectOfType<Base>();
        if (playerBase == null)
        {
            Debug.LogError("Base not found in the scene.");
            return;
        }

        playerBase.AddPopulation(populationIncrease);
        StartCoroutine(GenerateTalers());
    }

    private IEnumerator GenerateTalers()
    {
        while (true)
        {
            yield return new WaitForSeconds(incomeInterval);
            playerBase.AddResources(talersIncome, "Talers");
            Debug.Log("Generated Talers: " + talersIncome);
        }
    }
}
