using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class BackgroundMusicController : MonoBehaviour
{
    #region Variables

    [FormerlySerializedAs("createTag")]
    [Header("Tags")]
    [SerializeField] private string _createTag;

    #endregion

    #region Unity lifecycle

    private void Awake()
    {
        GameObject obj = GameObject.FindWithTag(_createTag);
        if (obj != null||SceneManager.GetActiveScene().name == Scenes.Game)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.tag = _createTag;
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion
}