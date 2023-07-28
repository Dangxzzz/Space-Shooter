using TMPro;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    #region Variables

    public static int Score;

    #endregion

    #region Unity lifecycle

    private void Awake()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            Score = PlayerPrefs.GetInt("HighScore");
        }

        PlayerPrefs.SetInt("HighScore", Score);
    }

    private void Update()
    {
        TextMeshProUGUI gt = GetComponent<TextMeshProUGUI>();
        gt.text = "High Score:" + Score;
        if (Score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", Score);
        }
    }

    #endregion
}