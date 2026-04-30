
using Unity.VisualScripting;
using UnityEngine;

public class PoliceMegaphone : MonoBehaviour
{
    //ripped from PESSshooter, this works surprisingly well with almost everything
    public AudioSource PoliceMegaphonePlayer;
    public AudioClip Megaphone1, Megaphone2, Megaphone3, Megaphone4, Megaphone5, Megaphone6;

    private bool isPlaying = false;

    private float lastMegaphoneTime;
    private float megaphoneCooldown = 10f;



    public void PlayRandomMegaphone()
    {
        if (Time.time - lastMegaphoneTime > megaphoneCooldown)
        {
            isPlaying = false;
        }
        int choice = Random.Range(0, 4);

        if (choice == 0 && isPlaying == false)
        {
            PoliceMegaphonePlayer.PlayOneShot(Megaphone1);
            isPlaying = true;
            Debug.Log("Megaphone1");
            lastMegaphoneTime = Time.time;
        }
        if (choice == 1 && isPlaying == false)
        {
            PoliceMegaphonePlayer.PlayOneShot(Megaphone2);
            isPlaying = true;
            Debug.Log("Megaphone2");
            lastMegaphoneTime = Time.time;
        }
        if (choice == 2 && isPlaying == false)
        {
            PoliceMegaphonePlayer.PlayOneShot(Megaphone3);
            isPlaying = true;
            Debug.Log("Megaphone3");
            lastMegaphoneTime = Time.time;
        }
        if (choice == 3 && isPlaying == false)
        {
            PoliceMegaphonePlayer.PlayOneShot(Megaphone4);
            isPlaying = true;
            Debug.Log("Megaphone4");
            lastMegaphoneTime = Time.time;
        }
        if (choice == 4 && isPlaying == false)
        {
            PoliceMegaphonePlayer.PlayOneShot(Megaphone5);
            isPlaying = true;
            Debug.Log("Megaphone5");
            lastMegaphoneTime = Time.time;
        }
        if (choice == 5 && isPlaying == false)
        {
            PoliceMegaphonePlayer.PlayOneShot(Megaphone6);
            isPlaying = true;
            lastMegaphoneTime = Time.time;

            Debug.Log("Megaphone6");
        }

        
    }

}
