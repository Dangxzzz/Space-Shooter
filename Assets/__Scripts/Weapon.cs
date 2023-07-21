using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public enum WeaponType
{
    None,
    Blaster,
    Spread,
    Phaser,
    Missile,
    Laser,
    Shield,
}

[System.Serializable]
public class WeaponDefinition
{
    #region Variables

    public Color color = Color.white;
    public float continuousDamage;
    public float damageOnHit;

    public float delayBetweenShots;
    public string letter;
    public Color projectileColor = Color.white;
    public GameObject projectilePrefab;
    public WeaponType type = WeaponType.None;
    public float velocity = 20;

    #endregion
}

public class Weapon : MonoBehaviour
{
    #region Variables

    public GameObject collar;
    public WeaponDefinition def;
    public float lastShotTime;

    public LineRenderer lineRenderer;
    public static Transform ProjectileAnchor;

    [FormerlySerializedAs("_type")]
    [Header("Set Dynamically")]
    [SerializeField]
    private WeaponType type = WeaponType.None;

    private readonly float _amplitude = 25f;
    private Renderer _collarRend;
    private float _elapsedTime;
    private readonly float _frequency = 2f;
    private ProjectileHero _pm;

    private readonly float _rocketSpeed = 40f;
    private readonly float _rotationSpeed = 25f;
    private GameSoundEffectService _soundService;
    private float _startTime;
    private Transform _target;

    #endregion

    #region Properties

    public WeaponType Type
    {
        get => type;
        set => SetType(value);
    }

    #endregion

    #region Unity lifecycle

    private void Start()
    {
        _soundService = FindObjectOfType<GameSoundEffectService>();
        collar = transform.Find("Gun_prefab").gameObject;
        _collarRend = collar.GetComponent<Renderer>();
        SetType(type);
        if (ProjectileAnchor == null)
        {
            GameObject go = new("_ProjectileAnchor");
            ProjectileAnchor = go.transform;
        }

        GameObject rootGo = transform.root.gameObject;
        if (rootGo.GetComponent<Hero>() != null)
        {
            rootGo.GetComponent<Hero>().FireDelegate += Fire;
        }

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.3f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.enabled = false;
        CreateColorGradient();
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (Type == WeaponType.Missile && _pm != null && _target != null)
        {
            Vector3 direction = _target.position - _pm.transform.position;
            _pm.transform.rotation = Quaternion.Slerp(_pm.transform.rotation, Quaternion.LookRotation(direction),
                Time.deltaTime * _rotationSpeed);
            _pm.GetComponent<Rigidbody>().velocity = _pm.transform.forward * _rocketSpeed;
        }

        if (Type == WeaponType.Laser)
        {
            UpdateLaser();
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    #endregion

    #region Public methods

    public void Fire()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        if (Time.time - lastShotTime < def.delayBetweenShots)
        {
            return;
        }

        ProjectileHero p;
        Vector3 vel = Vector3.up * def.velocity;
        if (transform.up.y < 0)
        {
            vel.y = -vel.y;
        }

        switch (Type)
        {
            case WeaponType.Blaster:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;
            case WeaponType.Spread:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                break;
            case WeaponType.Phaser:
                p = MakeProjectile();
                p.StartCoroutine(MoveProjectileSin(p));
                p = MakeProjectile();
                p.StartCoroutine(MoveProjectileCos(p));
                break;
            case WeaponType.Missile:
                _pm = MakeProjectile();
                _target = FindNearestEnemy();
                break;
            case WeaponType.Laser:
                _soundService.PlayLaserSound();
                StartCoroutine(FireLaser());
                break;
        }
    }

    public ProjectileHero MakeProjectile()
    {
        GameObject go = Instantiate(def.projectilePrefab);
        _soundService.PlayShootSound();
        if (transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }

        go.transform.position = collar.transform.position;
        go.transform.SetParent(ProjectileAnchor, true);
        ProjectileHero p = go.GetComponent<ProjectileHero>();
        p.Type = Type;
        lastShotTime = Time.time;
        return p;
    }

    public void SetType(WeaponType wt)
    {
        type = wt;
        if (Type == WeaponType.None)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        def = Main.GetWeaponDefinition(type);
        _collarRend.material.color = def.color;
        lastShotTime = 0;
    }

    #endregion

    #region Private methods

    private void CreateColorGradient()
    {
        Gradient gradient = new();
        gradient.mode = GradientMode.Blend;

        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0] = new GradientColorKey(Color.white, 0f);
        colorKeys[1] = new GradientColorKey(Color.magenta, 1f);

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(1f, 0f);
        alphaKeys[1] = new GradientAlphaKey(1f, 1f);

        gradient.SetKeys(colorKeys, alphaKeys);

        lineRenderer.colorGradient = gradient;
        
        Material material = new(Shader.Find("Sprites/Default"));
        material.color = Color.white;
        material.SetColor("_GradientColor", Color.white);
        material.SetColor("_GradientColor2", Color.magenta);

        lineRenderer.material = material;
    }

    private Transform FindNearestEnemy()
    {
        Enemy[] enemyShips = FindObjectsOfType<Enemy>();

        if (enemyShips.Length == 0)
        {
            return null;
        }

        Enemy closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Enemy enemyShip in enemyShips)
        {
            float distance = Vector3.Distance(transform.position, enemyShip.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemyShip;
            }
        }

        if (closestEnemy != null)
        {
            return closestEnemy.transform;
        }

        return null;
    }

    private IEnumerator FireLaser()
    {
        lineRenderer.enabled = true;
        while (Input.GetKey(KeyCode.Space))
        {
            lineRenderer.SetPosition(0, collar.transform.position);
            yield return null;
        }

        lineRenderer.enabled = false;
    }

    private IEnumerator MoveProjectileCos(ProjectileHero projectile)
    {
        _startTime = Time.time;

        while (projectile != null)
        {
            float elapsedTime = Time.time - _startTime;
            float theta = Mathf.PI * 2 * _frequency * elapsedTime;
            Vector3 cos = Vector3.right * Mathf.Cos(theta) * _amplitude;

            projectile.rigid.velocity = Vector3.up * def.velocity + cos;

            yield return null;
        }
    }

    private IEnumerator MoveProjectileSin(ProjectileHero projectile)
    {
        _startTime = Time.time;

        while (projectile != null)
        {
            float elapsedTime = Time.time - _startTime;
            float theta = Mathf.PI * 2 * _frequency * elapsedTime;
            Vector3 sin = Vector3.right * Mathf.Sin(theta + Mathf.PI) * _amplitude;

            projectile.rigid.velocity = Vector3.up * def.velocity + sin;

            yield return null;
        }
    }

    private void UpdateLaser()
    {
        if (lineRenderer != null && lineRenderer.enabled)
        {
            lineRenderer.SetPosition(0, collar.transform.position);
            RaycastHit[] hits = Physics.RaycastAll(collar.transform.position, Vector3.up);
            bool enemyHit = false;
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    lineRenderer.SetPosition(1,
                        new Vector3(hit.point.x, hit.point.y, collar.transform.position.z + 1f));
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(def.continuousDamage);
                        enemyHit = true;
                    }
                }
            }

            if (!enemyHit)
            {
                Vector3 laserEndPoint = collar.transform.position + Vector3.up * 100f;
                lineRenderer.SetPosition(1,
                    new Vector3(laserEndPoint.x, laserEndPoint.y, collar.transform.position.z + 1f));
            }
        }
    }

    #endregion
}