using UnityEngine;

public class Enemy_2 : Enemy
{
    #region Variables

    public float birthTime;
    public float lifeTime = 10;

    [Header("Set Dynamically: Enemy_2")]
    public Vector3 p0;
    public Vector3 p1;
    [Header("Set in Inspector: Enemy_2")]
    public float sinEccentricity = 0.6f;

    #endregion

    #region Unity lifecycle

    private void Start()
    {
        p0 = Vector3.zero;
        p0.x = -BndCheck.camWidth - BndCheck.radius;
        p0.y = Random.Range(-BndCheck.camHeight, BndCheck.camHeight);

        p1 = Vector3.zero;
        p1.x = BndCheck.camWidth + BndCheck.radius;
        p1.y = Random.Range(-BndCheck.camHeight, BndCheck.camHeight);

        if (Random.value > 0.5f)
        {
            p0.x *= -1;
            p1.x *= -1;
        }

        birthTime = Time.time;
    }

    #endregion

    #region Public methods

    public override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;
        if (u > 1)
        {
            Destroy(gameObject);
            return;
        }

        u = u + sinEccentricity * Mathf.Sin(u * Mathf.PI * 2);
        Pos = (1 - u) * p0 + u * p1;
    }

    #endregion
}