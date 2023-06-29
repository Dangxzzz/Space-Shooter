using UnityEngine;

public class Sheild : MonoBehaviour
{
    #region Variables

    [Header("Set Dynamically")]
    public int levelShown;
    [Header("Set in Inspector")]
    public float rotationsPerSecond = 0.1f;
    private Material _mat;

    #endregion

    #region Unity lifecycle

    private void Start()
    {
        _mat = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        int currentLevel = Mathf.FloorToInt(Hero.S.ShieldLevel);
        if (levelShown != currentLevel)
        {
            levelShown = currentLevel;
            _mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
        }

        float rZ = -(rotationsPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
    }

    #endregion
}