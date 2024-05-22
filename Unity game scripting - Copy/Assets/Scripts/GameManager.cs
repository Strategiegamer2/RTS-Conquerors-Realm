using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public CameraMovement cameraMovement;

    public Slider moveSpeedSlider;
    public Slider rotationSpeedSlider;
    public Slider verticalSpeedSlider;
    public Slider mouseSensitivitySlider;

    private bool isPaused = false;

    void Start()
    {
        // Initialize sliders with current camera values
        moveSpeedSlider.value = cameraMovement.moveSpeed;
        rotationSpeedSlider.value = cameraMovement.rotationSpeed;
        verticalSpeedSlider.value = cameraMovement.verticalSpeed;
        mouseSensitivitySlider.value = cameraMovement.mouseSensitivity;

        // Add listeners to update camera values
        moveSpeedSlider.onValueChanged.AddListener(delegate { UpdateMoveSpeed(); });
        rotationSpeedSlider.onValueChanged.AddListener(delegate { UpdateRotationSpeed(); });
        verticalSpeedSlider.onValueChanged.AddListener(delegate { UpdateVerticalSpeed(); });
        mouseSensitivitySlider.onValueChanged.AddListener(delegate { UpdateMouseSensitivity(); });

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
    }

    public void CloseOptions()
    {
        optionsMenu.SetActive(false);
    }

    void UpdateMoveSpeed()
    {
        cameraMovement.moveSpeed = moveSpeedSlider.value;
    }

    void UpdateRotationSpeed()
    {
        cameraMovement.rotationSpeed = rotationSpeedSlider.value;
    }

    void UpdateVerticalSpeed()
    {
        cameraMovement.verticalSpeed = verticalSpeedSlider.value;
    }

    void UpdateMouseSensitivity()
    {
        cameraMovement.mouseSensitivity = mouseSensitivitySlider.value;
    }
}
