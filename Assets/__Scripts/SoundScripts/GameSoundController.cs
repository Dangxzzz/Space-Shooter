using UnityEngine;

public class GameSoundController : MonoBehaviour
{
    #region Variables

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private int _levelDicrease = 5;

    #endregion

    #region Unity lifecycle

    private void Awake()
    {
        _audioSource.volume = StaticSoundVolumeSave.VolumeSound / _levelDicrease;
    }

    #endregion
}