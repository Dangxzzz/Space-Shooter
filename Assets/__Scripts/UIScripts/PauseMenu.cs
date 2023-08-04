using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    #region Variables

    public static bool gameIsPaused;

    [SerializeField] private GameObject _pauseMenuUI;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _resumeButton;

    #endregion

    #region Unity lifecycle

    private void Start()
    {
        gameIsPaused = false;
        Time.timeScale = 1;
        _exitButton.onClick.AddListener(OnExitButtonClick);
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
        _resumeButton.onClick.AddListener(OnResumeButtonClick);
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

    private void OnExitButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnMainMenuButtonClick()
    {
        SceneManager.LoadScene(Scenes.StartMenu);
        Time.timeScale = 1f;
    }

    private void OnResumeButtonClick()
    {
        Resume();
    }

    private void Pause_Menu()
    {
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    private void Resume()
    {
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    #endregion
}