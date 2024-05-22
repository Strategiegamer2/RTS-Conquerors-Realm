using UnityEngine;

public class BarracksSelection : MonoBehaviour
{
    public BarracksUI barracksUI; // Assign the BarracksUI script in the Inspector

    void OnMouseDown()
    {
        barracksUI.ShowBarracksUI();
    }
}
