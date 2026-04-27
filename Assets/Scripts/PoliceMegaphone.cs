using UnityEngine;

public class PoliceMegaphone : MonoBehaviour
{
    //ripped from PESSshooter, this works surprisingly well with almost everything
    public AudioSource PoliceMegaphonePlayer;
    public AudioClip Megaphone1, Megaphone2, Megaphone3, Megaphone4, Megaphone5, Megaphone6;

    public void PlayRandomMegaphone()
    {
        int choice = Random.Range(0, 4);

        if (choice == 0)
        {
            PoliceMegaphonePlayer.PlayOneShot(Megaphone1);
        }
        if (choice == 1)
        {
            PoliceMegaphonePlayer.PlayOneShot(Megaphone2);
        }
        if (choice == 2)
        {
            PoliceMegaphonePlayer.PlayOneShot(Megaphone3);
        }
        if (choice == 3)
        {
            PoliceMegaphonePlayer.PlayOneShot(Megaphone4);
        }
        if (choice == 4)
        {
            PoliceMegaphonePlayer.PlayOneShot(Megaphone5);
        }
        else
        {
            PoliceMegaphonePlayer.PlayOneShot(Megaphone6);
        }
    }

}
