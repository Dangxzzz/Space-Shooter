using UnityEngine;

public class Enemy_5 : Enemy
{
    #region Variables

    [SerializeField] private Transform heroTransform;

    #endregion

    #region Unity lifecycle

    private void Start()
    {
        GameObject heroObject = GameObject.FindGameObjectWithTag("Hero");
        if (heroObject != null)
        {
            heroTransform = heroObject.transform;
        }
        else
        {
            Debug.LogError("Could not find Hero object in the scene!");
        }
    }

    #endregion

    #region Public methods

    public override void Move()
    {
        if (heroTransform != null && heroTransform.gameObject.activeSelf)
        {
            Vector3 direction = heroTransform.position - transform.position;
            direction.Normalize();
            Vector3 targetPosition = transform.position + direction * speed * Time.deltaTime;
            transform.position = targetPosition;
        }
        else
        {
            base.Move();
        }
    }

    #endregion
}