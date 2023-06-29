using UnityEngine;
using System.Collections;

public class Enemy_6 : Enemy
{
    private Vector3 _pos0, _pos1;
    private float _timeStart;
    private float _duration = 4;
    public GameObject projectilePrefab;  
    public float minShootDelay = 5f;  
    public float maxShootDelay = 10f;
    public float projectileSpeed = 10f;
    public float shootRadius= 20f;
    void Start()
    {
        _pos0 = _pos1 = Pos;
        InitMovement();
        StartCoroutine(ShootProjectiles());
    }
    
    private IEnumerator  ShootProjectiles()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minShootDelay, maxShootDelay));
            
            for (int i = 0; i < 8; i++)
            {
                float angle = i * 45f; 
                float radians = angle * Mathf.Deg2Rad;
                Vector3 direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f);
                Vector3 spawnPosition = transform.position + direction * shootRadius;

                GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
                Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
                projectileRigidbody.velocity = direction.normalized * projectileSpeed;

                float rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                projectile.transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
            }
        }
    }

    void InitMovement()
    {
        _pos0 = _pos1;
        float widMinRad = BndCheck.camWidth - BndCheck.radius;
        float hgtMinRad = BndCheck.camHeight - BndCheck.radius;
        _pos1.x = Random.Range(-widMinRad, widMinRad);
        _pos1.y = Random.Range(-hgtMinRad, hgtMinRad);
        _timeStart = Time.time;
    }
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
}
