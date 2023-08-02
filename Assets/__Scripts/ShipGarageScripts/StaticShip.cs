using UnityEngine;

public static class StaticShip
{
    #region Variables

    public static GameObject ShipInGame;

    #endregion

    #region Public methods

    public static void Initialize()
    {
        if (ShipInGame == null)
        {
            GameObject staticDataHolder = new GameObject("StaticDataHolder");
            Object.DontDestroyOnLoad(staticDataHolder);
            ShipInGame = new GameObject("ShipInGame");
            ShipInGame.transform.SetParent(staticDataHolder.transform);
        }
    }

    #endregion
}