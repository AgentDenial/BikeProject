using UnityEngine;

public class OutOfBoundsDetect : MonoBehaviour
{
    public MenuManager menuManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        menuManager = FindFirstObjectByType<MenuManager>();
    }
    private void OnCollisionEnter(Collision other)
    {
        //if (other.gameObject.layer == LayerMask.NameToLayer("OutOfBounds"))
        //if (other.gameObject.CompareTag("OutOfBounds"))
        Debug.Log("test");
        

        if (1 << other.gameObject.layer == 1 << LayerMask.NameToLayer("OutOfBounds"))
        {
            Debug.Log("Fell out of bounds");
            menuManager.YouLose();
        }

       /* if ((LayerMask.NameToLayer("OutOfBounds") & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            Debug.Log("Fell out of bounds");
            menuManager.GameOver();
            
            //gameOver Logic
            //gameOverSceneTransition
        }*/
    }
}
