using UnityEngine;

public class Enemy_1 : Enemy
{
    #region Variables

    [Header("Set in Inspector: Enemy_1")]
    public float waveFrequency = 2;
    public float waveRotY = 45;
    public float waveWidth = 4;
    private float _birthTime;

    private float _x0;

    #endregion

    #region Unity lifecycle

    private void Start()
    {
        _x0 = Pos.x;
        _birthTime = Time.time;
    }

    #endregion

    #region Public methods

    public override void Move()
    {
        Vector3 tempPos = Pos;
        float age = Time.time - _birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = _x0 + waveWidth * sin;
        Pos = tempPos;

        Vector3 rot = new(0, sin * waveRotY, 0);
        transform.rotation = Quaternion.Euler(rot);
        base.Move();
    }

    #endregion
}