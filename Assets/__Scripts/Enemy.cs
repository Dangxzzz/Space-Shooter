using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables

    public float damageDoneTime;
    public float health = 10;
    public Material[] materials;
    public bool notifiedOfDestruction;

    [Header("Set Dynamically: Enemy")]
    public Color[] originalColors;

    public float powerUpDropChance = 1f;
    public int score = 100;
    public float showDamageDuration = 0.1f;
    public bool showingDamage;
    [Header("Set in Inspector: Enemy")]
    public float speed = 10f;

    protected BoundsCheck BndCheck;

    #endregion

    #region Properties

    public Vector3 Pos
    {
        get => transform.position;
        set => transform.position = value;
    }

    #endregion

    #region Unity lifecycle

    private void Awake()
    {
        BndCheck = GetComponent<BoundsCheck>();
        materials = Utilts.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }

    private void Update()
    {
        Move();

        if (showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }

        if (BndCheck != null && BndCheck.offDown)
        {
            if (Pos.y < BndCheck.camHeight - BndCheck.radius)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        GameObject otherGo = collision.gameObject;
        switch (otherGo.tag)
        {
            case "ProjectileHero":
                ShowDamage();
                ProjectileHero p = otherGo.GetComponent<ProjectileHero>();
                if (!BndCheck.isOnScreen)
                {
                    Destroy(otherGo);
                    break;
                }

                health -= Main.GetWeaponDefinition(p.Type).damageOnHit;
                if (health <= 0)
                {
                    if (!notifiedOfDestruction)
                    {
                        Main.S.ShipDestroyed(this);
                    }

                    notifiedOfDestruction = true;
                    Destroy(gameObject);
                }

                Destroy(otherGo);
                break;

            default:
                print("Enemy hot by non-ProjectileHero:" + otherGo.name);
                break;
        }
    }

    #endregion

    #region Public methods

    public virtual void Move()
    {
        Vector3 tempPos = Pos;
        tempPos.y -= speed * Time.deltaTime;
        Pos = tempPos;
    }

    public void TakeDamage(float continiousDamage)
    {
        ShowDamage();
        health -= continiousDamage;
        if (health <= 0)
        {
            if (!notifiedOfDestruction)
            {
                Main.S.ShipDestroyed(this);
            }

            notifiedOfDestruction = true;
            Destroy(gameObject);
        }
    }

    #endregion

    #region Private methods

    private void ShowDamage()
    {
        foreach (Material m in materials)
        {
            m.color = Color.red;
        }

        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }

    private void UnShowDamage()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }

        showingDamage = false;
    }

    #endregion
}