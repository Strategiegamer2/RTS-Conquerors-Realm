using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public RectTransform tooltipRectTransform;
    public TextMeshProUGUI tooltipText;

    private void Awake()
    {
        HideTooltip();
    }

    private void Update()
    {
        if (tooltipRectTransform.gameObject.activeSelf)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent.GetComponent<RectTransform>(),
                Input.mousePosition,
                null,
                out position);

            // Adjust the tooltip position to be slightly above the cursor
            position.y += 20; // Adjust this value as needed
            tooltipRectTransform.localPosition = position;
        }
    }

    public void ShowTooltip(string message)
    {
        tooltipRectTransform.gameObject.SetActive(true);
        tooltipText.text = message;
    }

    public void HideTooltip()
    {
        tooltipRectTransform.gameObject.SetActive(false);
    }
}
