using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ShipDisplay : MonoBehaviour
{
    #region Variables
    
    [Header("Description")]
    [SerializeField] private TextMeshProUGUI _shipName;
    
    [Header("ship Model")]
    [SerializeField] private GameObject _shipModel;

    #endregion

    #region Public methods

    public void UpdateShip(Ship _newShip)
    {
        _shipName.text = _newShip.nameShip;

        if (_shipModel.transform.childCount > 0)
        {
            Destroy(_shipModel.transform.GetChild(0).gameObject);
        }

        Instantiate(_newShip.shipModel, _shipModel.transform.position, _shipModel.transform.rotation,
            _shipModel.transform);
    }

    #endregion
}