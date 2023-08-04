using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject _deathMenu;
    [SerializeField] private TextMeshProUGUI _loseLabel;
    [SerializeField] private TextMeshProUGUI _scoreLabel;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _startMenuButton;

    private Main _main;

    #endregion

    #region Unity lifecycle

    private void Start()
    {
        _restartButton.onClick.AddListener(OnRestartButtonClick);
        _startMenuButton.onClick.AddListener(OnMenuButtonClick);
    }

    private void Update()
    {
        if (Hero._isDead)
        {
            CheckIsNewRecord();
            _scoreLabel.text = "Your "+Main.S.ScoreCounter.text;
            _deathMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void CheckIsNewRecord()
    {
        if (Main.S.IsNewRecord)
        {
            _loseLabel.color = Color.green;
            _loseLabel.text = "New Record!";
        }
        else
        {
            _loseLabel.color = Color.red;
            _loseLabel.text = "You Lose!";
        }
    }

    #endregion

    #region Private methods

    private void AnyButtonClick()
    {
        Hero._isDead = false;
        _deathMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnMenuButtonClick()
    {
        AnyButtonClick();
        SceneManager.LoadScene(Scenes.StartMenu);
    }

    private void OnRestartButtonClick()
    {
        AnyButtonClick();
        Main.S.Restart();
    }

    #endregion
}