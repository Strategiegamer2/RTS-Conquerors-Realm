using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject optionsMenuPanel;

    //public Slider soundVolumeSlider;
    //public InputField panSpeedInput;
    //public InputField scrollSpeedInput;
    //public InputField rotationSpeedInput;

    private void Start()
    {
        ShowMainMenu();
        LoadSettings();
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        optionsMenuPanel.SetActive(false);
    }

    public void ShowOptionsMenu()
    {
        mainMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void SaveSettings()
    {
        //PlayerPrefs.SetFloat("SoundVolume", soundVolumeSlider.value);
        //PlayerPrefs.SetFloat("PanSpeed", float.Parse(panSpeedInput.text));
        //PlayerPrefs.SetFloat("ScrollSpeed", float.Parse(scrollSpeedInput.text));
        //PlayerPrefs.SetFloat("RotationSpeed", float.Parse(rotationSpeedInput.text));
        //PlayerPrefs.Save();

        ShowMainMenu();
    }

    public void LoadSettings()
    {
        //soundVolumeSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1f);
        //panSpeedInput.text = PlayerPrefs.GetFloat("PanSpeed", 20f).ToString();
        //scrollSpeedInput.text = PlayerPrefs.GetFloat("ScrollSpeed", 20f).ToString();
        //rotationSpeedInput.text = PlayerPrefs.GetFloat("RotationSpeed", 100f).ToString();
    }
}
