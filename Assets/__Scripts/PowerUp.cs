using TMPro;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    #region Variables

    public float birthTime;

    public GameObject cube;
    public Vector2 driftMinMax = new(.25f, 2);
    public float fadeTime = 4f;
    public TextMeshPro letter;
    public float lifeTime = 6f;
    [Header("Set in Inspector")]
    public Vector2 rotMinMax = new(15, 90);
    public Vector3 rotPerSecond;

    [Header("Set Dynamically")]
    public WeaponType type;
    private BoundsCheck _bndCheck;
    private Renderer _cubeRend;

    private Rigidbody _rigid;

    #endregion

    #region Unity lifecycle

    private void Awake()
    {
        cube = transform.Find("Cube").gameObject;
        letter = GetComponent<TextMeshPro>();
        _rigid = GetComponent<Rigidbody>();
        _bndCheck = GetComponent<BoundsCheck>();
        _cubeRend = cube.GetComponent<Renderer>();

        Vector3 vel = Random.onUnitSphere;
        vel.z = 0;
        vel.Normalize();
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        _rigid.velocity = vel;

        transform.rotation = Quaternion.identity;
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y));
        birthTime = Time.time;
    }

    private void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);
        float u = Time.time - (birthTime + lifeTime);
        if (u >= 1)
        {
            Destroy(gameObject);
            return;
        }

        if (u > 0)
        {
            Color c = _cubeRend.material.color;
            c.a = 1f - u;
            _cubeRend.material.color = c;
            c = letter.color;
            c.a = 1f - u * 0.5f;
            letter.color = c;
        }

        if (!_bndCheck.isOnScreen)
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Public methods

    public void AbsorbedBy(GameObject target)
    {
        Destroy(gameObject);
    }

    public void SetType(WeaponType wt)
    {
        WeaponDefinition def = Main.GetWeaponDefinition(wt);
        _cubeRend.material.color = def.color;
        letter.text = def.letter;
        type = wt;
    }

    #endregion
}