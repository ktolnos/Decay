using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver: MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Damaging")){
            // TODO play game over sound
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}