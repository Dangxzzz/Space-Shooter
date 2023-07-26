using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShipMenuScreen : MonoBehaviour
{
    #region Variables

    public Color[] colors = new Color[5];

    public TextMeshProUGUI NameLabel;
    public GameObject Planet;
    public Button BackButton;
    [SerializeField] private GameObject[] ships = new GameObject[5];

    #endregion

    #region Unity lifecycle

    private void Start()
    {
        BackButton.onClick.AddListener(OnBackButtonClick);
    }

    private void Update()
    {
        Planet.transform.Rotate(0.0f, 0, 0.1f);
        ChangeTextColor();
    }

    #endregion

    #region Private methods

    private void ChangeTextColor()
    {
        int ntx = Random.Range(0, colors.Length);
        NameLabel.color = colors[ntx];
    }

    private void OnBackButtonClick()
    {
        SceneManager.LoadScene(Scenes.StartMenu);
    }

    #endregion
}