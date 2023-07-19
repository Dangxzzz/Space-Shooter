using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    #region Variables

    [Header("Tags")]
    [SerializeField] private string createTag;

    #endregion

    #region Unity lifecycle

    private void Awake()
    {
        GameObject obj = GameObject.FindWithTag(createTag);
        if (obj != null)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.tag = createTag;
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion
}