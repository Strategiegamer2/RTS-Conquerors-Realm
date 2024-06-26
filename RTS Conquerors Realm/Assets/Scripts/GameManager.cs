using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GodViewCamera cameraMovement;

    public Slider panSpeedSlider;
    public Slider rotationSpeedSlider;
    public Slider scrollSpeedSlider;

    private bool isPaused = false;

    void Start()
    {
        // Initialize sliders with current camera values
        panSpeedSlider.value = cameraMovement.panSpeed;
        rotationSpeedSlider.value = cameraMovement.rotationSpeed;
        scrollSpeedSlider.value = cameraMovement.scrollSpeed;

        // Add listeners to update camera values
        panSpeedSlider.onValueChanged.AddListener(delegate { UpdatePanSpeed(); });
        rotationSpeedSlider.onValueChanged.AddListener(delegate { UpdateRotationSpeed(); });
        scrollSpeedSlider.onValueChanged.AddListener(delegate { UpdateScrollSpeed(); });

        // Hide menus initially
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void CloseOptions()
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void UpdatePanSpeed()
    {
        cameraMovement.panSpeed = panSpeedSlider.value;
    }

    void UpdateRotationSpeed()
    {
        cameraMovement.rotationSpeed = rotationSpeedSlider.value;
    }

    void UpdateScrollSpeed()
    {
        cameraMovement.scrollSpeed = scrollSpeedSlider.value;
    }
}
