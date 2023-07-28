using UnityEngine;

[CreateAssetMenu(fileName = "New Ship", menuName = "Scriptable Objects/Ship")]
public class Ship : ScriptableObject
{
    #region Variables

    public string nameShip;
    public GameObject shipModel;

    #endregion
}