using UnityEngine;

public class ProjectileHero : MonoBehaviour
{
    #region Variables

    [Header("Set Dynamically")]
    public Rigidbody rigid;
    [SerializeField]
    private WeaponType _type;
    private BoundsCheck _bndCheck;
    private Renderer _rend;

    private Transform _target;

    #endregion

    #region Properties

    public WeaponType Type
    {
        get => _type;
        set => SetType(value);
    }

    #endregion

    #region Unity lifecycle

    private void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>();
        _rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_bndCheck.offUp)
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Public methods

    public void SetType(WeaponType eType)
    {
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        _rend.material.color = def.projectileColor;
    }

    #endregion
}