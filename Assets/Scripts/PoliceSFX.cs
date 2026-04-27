using UnityEngine;

public class PoliceSFX : MonoBehaviour
{
    
    public AudioSource PoliceSFXPlayer;
    private AudioClip[] sirenClips;

    [SerializeField] public AudioClip Siren1;
    [SerializeField] public AudioClip AltSiren;


    private void Start()
    {
        if (PoliceSFXPlayer == null)
        {
            PoliceSFXPlayer = GetComponent<AudioSource>();
        }

        sirenClips = new AudioClip[]
            { Siren1, AltSiren };
        PoliceSFXPlayer.loop = true;

        PlayRandomSiren();
    }

    public void PlayRandomSiren()
    {
        int randomIndex = Random.Range(0, sirenClips.Length);
        AudioClip selectedClip = sirenClips[randomIndex];

        PoliceSFXPlayer.clip = selectedClip;
        PoliceSFXPlayer.Play();
    }

    public void SwitchRandomSiren()
    {
        PoliceSFXPlayer.Stop();
        PlayRandomSiren();
    }
}
