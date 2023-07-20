using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class StartMenuUI : MonoBehaviour
{
    #region Variables

    public TextMeshProUGUI NameLabel;
    public TextMeshProUGUI SliderLabel;
    public Color[] colors=new Color[5];
    public GameObject Planet;
    public float radius = 5f;
    public GameObject Ship;
    public float speed = 0.001f;
    public float tiltAngle = 45f;
    private float _angle;
    public Button StartGameButton;
    public Button SettingButton;
    public Button ExitButton;


    private Vector3 _center;

    #endregion

    #region Unity lifecycle

  

    private void Start()
    {
        _center = Ship.transform.position;
        StartGameButton.onClick.AddListener(OnStartGameButtonClick);
        SettingButton.onClick.AddListener(OnSettingButtonClick);
        ExitButton.onClick.AddListener(OnExitButtonClick);
        
    }
    private void Update()
    {
        Planet.transform.Rotate(0.0f, 0, 0.1f);
        MotionShip();
        ChangeTextColor();
    }

    private void OnStartGameButtonClick()
    {
        SceneManager.LoadScene(Scenes.Game);
    }

    private void OnSettingButtonClick()
    {
        SceneManager.LoadScene(Scenes.ShipMenu);
    }

    private void OnExitButtonClick()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    private void MotionShip()
    {
        _angle += speed * Time.deltaTime;

        float x = _center.x + Mathf.Cos(_angle + 30) * radius;
        float y = _center.y + Mathf.Cos(_angle * 1);
        float z = _center.z + Mathf.Sin(_angle) * radius;

        Ship.transform.position = new Vector3(x, y, z);

        Vector3 direction = new(-Mathf.Sin(_angle), 0f, Mathf.Cos(_angle));

        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        Ship.transform.rotation = toRotation;

        float tiltAmount = Mathf.Sin(_angle * 2f) * tiltAngle;
        Quaternion tiltRotation = Quaternion.Euler(0f, 0f, tiltAmount);
        Ship.transform.rotation *= tiltRotation;
    }

    private void ChangeTextColor()
    {
        int ntx = Random.Range(0, colors.Length);
        NameLabel.color = colors[ntx];
        SliderLabel.color = colors[ntx];
    }

    #endregion
}