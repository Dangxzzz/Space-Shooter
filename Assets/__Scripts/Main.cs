using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    #region Variables

    public float enemyDefaultPadding = 1.5f;
    public float enemySpawnPerSecond = 0.5f;

    public WeaponType[] powerUpFrequency =
    {
        WeaponType.Blaster, WeaponType.Blaster, WeaponType.Spread, WeaponType.Shield,
    };

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemis;
    public GameObject prefabPowerUp;
    public static Main S;

    public WeaponDefinition[] weaponDefinitions;

    private BoundsCheck _bndCheck;
    private static Dictionary<WeaponType, WeaponDefinition> _weapDict;

    #endregion

    #region Unity lifecycle

    private void Awake()
    {
        S = this;
        _bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);

        _weapDict = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            _weapDict[def.type] = def;
        }
    }

    #endregion

    #region Public methods

    public static WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        if (_weapDict.ContainsKey(wt))
        {
            return _weapDict[wt];
        }

        return new WeaponDefinition();
    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ShipDestroyed(Enemy e)
    {
        if (Random.value <= e.powerUpDropChance)
        {
            int ndx = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];

            GameObject go = Instantiate(prefabPowerUp);
            PowerUp pu = go.GetComponent<PowerUp>();
            pu.SetType(puType);
            pu.transform.position = e.transform.position;
        }
    }

    public void SpawnEnemy()
    {
        int ndx = Random.Range(0, prefabEnemis.Length);
        GameObject go = Instantiate(prefabEnemis[ndx]);
        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        Vector3 pos = Vector3.zero;
        float xMin = -_bndCheck.camWidth + enemyPadding;
        float xMax = _bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = _bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    #endregion
}