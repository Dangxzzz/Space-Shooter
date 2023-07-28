using UnityEngine;

public class Parallex : MonoBehaviour
{
    #region Variables

    public float motionMult = 0.25f;

    public GameObject[] panels;
    [Header("Set in Inspector")]
    public GameObject poi;
    public float scrollSpeed = -30f;
    private float _depth;

    private float _panelHt;

    #endregion

    #region Unity lifecycle

    private void Start()
    {
        _panelHt = panels[0].transform.localScale.y;
        _depth = panels[0].transform.position.z;
        panels[0].transform.position = new Vector3(0, 0, _depth);
        panels[1].transform.position = new Vector3(0, _panelHt, _depth);
    }

    private void Update()
    {
        float tY, tX = 0;
        tY = Time.time * scrollSpeed % _panelHt + _panelHt * 0.5f;

        if (poi != null)
        {
            tX = -poi.transform.position.x * motionMult;
        }

        panels[0].transform.position = new Vector3(tX, tY, _depth);
        if (tY >= 0)
        {
            panels[1].transform.position = new Vector3(tX, tY - _panelHt, _depth);
        }
        else
        {
            panels[1].transform.position = new Vector3(tX, tY + _panelHt, _depth);
        }
    }

    #endregion
}