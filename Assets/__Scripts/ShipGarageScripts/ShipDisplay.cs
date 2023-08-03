using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipDisplay : MonoBehaviour
{
    #region Variables

    [Header("Description")]
    [SerializeField] private TextMeshProUGUI _shipName;

    [Header("ship Model")]
    [SerializeField] private GameObject _shipModel;
    [SerializeField] private Ship _defaultShip;
    private Ship _chosenShip;
    private bool _isChoseShip;

    #endregion

    #region Public methods

    public void SaveShip()
    {
        StaticShip._isShipSaved = true;
        if (_isChoseShip)
        {
            StaticShip.ShipInGame = _chosenShip;
        }
        else
        {
            StaticShip.ShipInGame = _defaultShip;
        }

        SceneManager.LoadScene(Scenes.StartMenu);
    }

    public void UpdateShip(Ship _newShip)
    {
        _isChoseShip = true;
        _shipName.text = _newShip.nameShip;

        if (_shipModel.transform.childCount > 0)
        {
            Destroy(_shipModel.transform.GetChild(0).gameObject);
        }

        Instantiate(_newShip.shipModel, _shipModel.transform.position, _shipModel.transform.rotation,
            _shipModel.transform);
        _chosenShip = _newShip;
    }

    #endregion
}