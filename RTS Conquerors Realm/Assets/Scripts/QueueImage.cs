using UnityEngine;
using UnityEngine.UI;

public class QueueImage : MonoBehaviour
{
    public Image fillImage; // Assign the Image component in the Inspector
    private float buildTime;
    private float timer;

    public void Initialize(float buildTime)
    {
        this.buildTime = buildTime;
        timer = 0f;
        fillImage.fillAmount = 0f;
    }

    void Update()
    {
        if (timer < buildTime)
        {
            timer += Time.deltaTime;
            fillImage.fillAmount = timer / buildTime;
        }
    }
}
