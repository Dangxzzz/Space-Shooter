using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    #region Variables
    public static bool gameIsPaused;

    public GameObject PauseMenuUI;
    public Button MainMenuButton;
    public Button ExitButton;

    #endregion

    #region Unity lifecycle

    private void Start()
    {
        gameIsPaused = false;
        Time.timeScale = 1;
        ExitButton.onClick.AddListener(OnExitButtonClick);
        MainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause_Menu();
            }
        }
    }

    #endregion

    #region Private methods

    private void Pause_Menu()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    private void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    private void OnMainMenuButtonClick()
    {
        SceneManager.LoadScene(Scenes.StartMenu);
    }

    private void OnExitButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #endregion
}