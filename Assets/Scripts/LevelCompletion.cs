using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletion: MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponentInParent<Player>() != null)
        {
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1)
                                   % SceneManager.sceneCountInBuildSettings);
        }
    }
}