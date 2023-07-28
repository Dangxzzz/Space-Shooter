using UnityEngine;

public class GameSoundEffectService : MonoBehaviour
{
    #region Variables

    public AudioClip BonusAudioClip;

    public AudioClip GunAudioClip;
    public AudioClip KillAudioClip;
    public AudioClip LaserAudioClip;
    public AudioClip LoseAudioClip;
    public AudioSource SoundEffect;
    public AudioClip DamageAudioClip;

    #endregion

    #region Unity lifecycle

    private void Start()
    {
        SoundEffect = GetComponent<AudioSource>();
    }

    #endregion

    #region Public methods

    public void PlayBonusSound()
    {
        SoundEffect.PlayOneShot(BonusAudioClip);
    }

    public void PlayKillSound()
    {
        SoundEffect.PlayOneShot(KillAudioClip);
    }

    public void PlayLaserSound()
    {
        SoundEffect.PlayOneShot(LaserAudioClip);
    }

    public void PlayLoseSound()
    {
        SoundEffect.PlayOneShot(LoseAudioClip);
    }

    public void PlayShootSound()
    {
        SoundEffect.PlayOneShot(GunAudioClip,StaticSoundVolumeSave.VolumeSound/2);
    }

    public void PlayDamageSound()
    {
        SoundEffect.PlayOneShot(DamageAudioClip);
    }

    #endregion
}