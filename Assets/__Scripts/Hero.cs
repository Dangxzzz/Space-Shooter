using UnityEngine;

public class Hero : MonoBehaviour
{
    #region Variables

    public WeaponFireDelegate FireDelegate;
    public float gameRestartDelay = 2f;
    public float pitchMult = 30;
    public float rollMult = -45;
    public static Hero S;

    [Header("Set in Inspector")]
    public float speed = 30;
    public Weapon[] weapons;

    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;

    private GameObject _lastTriggerGo;
    private GameSoundEffectService _soundService;

    #endregion

    #region Properties

    public float ShieldLevel
    {
        get => _shieldLevel;
        set
        {
            _shieldLevel = Mathf.Min(value, 4);
            if (value < 0)
            {
                _soundService.PlayLoseSound();
                Destroy(gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }

    #endregion

    #region Unity lifecycle

    private void Start()
    {
        _soundService = FindObjectOfType<GameSoundEffectService>();
        _soundService.SoundEffect.volume=StaticSoundVolumeSave.VolumeSound;
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("Hero.Awake()-Attempted to assign second Hero.S!");
        }

        ClearWeapons();
        weapons[0].SetType(WeaponType.Blaster);
    }

    private void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
        if (Input.GetAxis("Jump") == 1 && FireDelegate != null)
        {
            FireDelegate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        if (go == _lastTriggerGo)
        {
            return;
        }

        _lastTriggerGo = go;
        if (go.tag == "Enemy" || go.tag == "ProjectileEnemy")
        {
            _soundService.PlayDamageSound();
            ShieldLevel--;
            Destroy(go);
        }
        else if (go.tag == "PowerUp")
        {
            _soundService.PlayBonusSound();
            AbsorbPowerUp(go);
        }
        else
        {
            print("Triggered by non-Enemy:" + go.name);
        }
    }

    #endregion

    #region Public methods

    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case WeaponType.Shield:
                ShieldLevel++;
                break;
            default:
                if (pu.type == weapons[0].Type)
                {
                    Weapon w = GetEmptyWeaponSlot();
                    if (w != null)
                    {
                        w.SetType(pu.type);
                    }
                }
                else
                {
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }

                break;
        }

        pu.AbsorbedBy(gameObject);
    }

    #endregion

    #region Private methods

    private void ClearWeapons()
    {
        foreach (Weapon w in weapons)
        {
            w.SetType(WeaponType.None);
        }
    }

    private Weapon GetEmptyWeaponSlot()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].Type == WeaponType.None)
            {
                return weapons[i];
            }
        }

        return null;
    }

    #endregion

    #region Local data

    public delegate void WeaponFireDelegate();

    #endregion
}