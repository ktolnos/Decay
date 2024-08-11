using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletion: MonoBehaviour
{
    public AudioClip winSound;
    public float time;
    private Animator transition;
    private AudioSource audioSource;
    private bool startedLoading = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        transition = Camera.main.GetComponentInChildren<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (startedLoading){
            return;
        }
        
        if (other.gameObject.GetComponentInParent<Player>() != null)
        {
            startedLoading = true;
            transition.SetTrigger("End");
            audioSource.PlayOneShot(winSound);
            StartCoroutine(LoadNextLevel(time));
        }
    }

    IEnumerator LoadNextLevel(float time){
        
        yield return new WaitForSeconds(time);
        SceneManager.LoadSceneAsync((SceneManager.GetActiveScene().buildIndex + 1)
                                   % SceneManager.sceneCountInBuildSettings);
        transition.SetTrigger("Start");
    }
}