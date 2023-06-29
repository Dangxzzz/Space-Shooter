using UnityEngine;

[System.Serializable]
public class Part
{
    #region Variables

    [HideInInspector]
    public GameObject go;
    public float health;

    [HideInInspector]
    public Material mat;
    public string name;
    public string[] protectedBy;

    #endregion
}

public class Enemy_4 : Enemy
{
    #region Variables

    [Header("Set in Inspector: Enemy_4")]
    public Part[] parts;
    private readonly float _duration = 4;

    private Vector3 _pos0, _pos1;
    private float _timeStart;

    #endregion

    #region Unity lifecycle

    private void Start()
    {
        _pos0 = _pos1 = Pos;
        InitMovement();

        Transform t;
        foreach (Part prt in parts)
        {
            t = transform.Find(prt.name);
            if (t != null)
            {
                prt.go = t.gameObject;
                prt.mat = prt.go.GetComponent<Renderer>().material;
            }
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        GameObject other = coll.gameObject;
        switch (other.tag)
        {
            case "ProjectileHero":
                ProjectileHero p = other.GetComponent<ProjectileHero>();
                if (!BndCheck.isOnScreen)
                {
                    Destroy(other);
                    break;
                }

                GameObject goHit = coll.contacts[0].thisCollider.gameObject;
                Part prtHit = FindPart(goHit);
                if (prtHit == null)
                {
                    goHit = coll.contacts[0].otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }

                if (prtHit.protectedBy != null)
                {
                    foreach (string s in prtHit.protectedBy)
                    {
                        if (!Destroyed(s))
                        {
                            Destroy(other);
                            return;
                        }
                    }
                }

                prtHit.health -= Main.GetWeaponDefinition(p.Type).damageOnHit;
                ShowLocalizedDamage(prtHit.mat);
                if (prtHit.health <= 0)
                {
                    prtHit.go.SetActive(false);
                }

                bool allDestroyed = true;
                foreach (Part prt in parts)
                {
                    if (!Destroyed(prt))
                    {
                        allDestroyed = false;
                        break;
                    }
                }

                if (allDestroyed)
                {
                    Main.S.ShipDestroyed(this);
                    Destroy(gameObject);
                }

                Destroy(other);
                break;
        }
    }

    #endregion

    #region Public methods

    public override void Move()
    {
        float u = (Time.time - _timeStart) / _duration;
        if (u >= 1)
        {
            InitMovement();
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2);
        Pos = (1 - u) * _pos0 + u * _pos1;
    }

    #endregion

    #region Private methods
    
    private bool Destroyed(string n)
    {
        return Destroyed(FindPart(n));
    }

    private bool Destroyed(Part prt)
    {
        if (prt == null)
        {
            return true;
        }

        return prt.health <= 0;
    }

    private Part FindPart(string n)
    {
        foreach (Part prt in parts)
        {
            if (prt.name == n)
            {
                return prt;
            }
        }

        return null;
    }

    private Part FindPart(GameObject go)
    {
        foreach (Part prt in parts)
        {
            if (prt.go == go)
            {
                return prt;
            }
        }

        return null;
    }

    private void InitMovement()
    {
        _pos0 = _pos1;
        float widMinRad = BndCheck.camWidth - BndCheck.radius;
        float hgtMinRad = BndCheck.camHeight - BndCheck.radius;
        _pos1.x = Random.Range(-widMinRad, widMinRad);
        _pos1.y = Random.Range(-hgtMinRad, hgtMinRad);
        _timeStart = Time.time;
    }

    private void ShowLocalizedDamage(Material m)
    {
        m.color = Color.red;
        damageDoneTime = Time.time + showDamageDuration;
        showingDamage = true;
    }

    #endregion
}