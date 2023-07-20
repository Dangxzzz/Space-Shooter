using UnityEngine;
using UnityEngine.UI;

public class SoundVolumeControllerComponent : MonoBehaviour
{
    #region Variables

    [Header("Components")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Slider _sliderSoundVolume;
    [Header("Keys")]
    [SerializeField] private string _saveVolume;
    [Header("Parameters")]
    [SerializeField] private float _volume;
    [Header("Tags")]
    [SerializeField] private string _sliderTag;

    #endregion

    #region Unity lifecycle

    private void Awake()
    {
        if (PlayerPrefs.HasKey(_saveVolume))
        {
            _volume = PlayerPrefs.GetFloat(_saveVolume);
            _audioSource.volume = _volume;
        }

        GameObject sliderObj = GameObject.FindWithTag(_sliderTag);
        if (sliderObj != null)
        {
            _sliderSoundVolume = sliderObj.GetComponent<Slider>();
            _sliderSoundVolume.value = _volume;
        }
        else
        {
            _volume = 0.5f;
            PlayerPrefs.SetFloat(_saveVolume, _volume);
            _audioSource.volume = _volume;
        }
    }

    private void LateUpdate()
    {
        GameObject sliderObj = GameObject.FindWithTag(_sliderTag);
        if (sliderObj != null)
        {
            _sliderSoundVolume = sliderObj.GetComponent<Slider>();
            _volume = _sliderSoundVolume.value;
            if (_audioSource.volume != _volume)
            {
                PlayerPrefs.SetFloat(_saveVolume, _volume);
            }
        }
        StaticSoundVolumeSave.VolumeSound = _volume;
        _audioSource.volume = _volume;
    }

    #endregion
}