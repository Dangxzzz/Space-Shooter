using UnityEngine;
using UnityEngine.Serialization;

public class ScriptableObjectChanger : MonoBehaviour
{
    #region Variables

    [SerializeField] private ScriptableObject[] scriptableObjects;

    [FormerlySerializedAs("carDisplay")]
    [Header("Display Scripts")]
    [SerializeField] private ShipDisplay _shipDisplay;

    private int currentMapIndex;

    #endregion

    #region Unity lifecycle

    private void Awake()
    {
        ChangeMap(0);
    }

    #endregion

    #region Public methods

    public void ChangeMap(int _index)
    {
        currentMapIndex += _index;
        if (currentMapIndex < 0)
        {
            currentMapIndex = scriptableObjects.Length - 1;
        }

        if (currentMapIndex > scriptableObjects.Length - 1)
        {
            currentMapIndex = 0;
        }

        if (_shipDisplay != null)
        {
            _shipDisplay.UpdateShip((Ship)scriptableObjects[currentMapIndex]);
        }
    }

    #endregion
}