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
    private Player _player;

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

        _player = other.gameObject.GetComponentInParent<Player>();
        if (_player != null)
        {
            startedLoading = true;
            transition.SetTrigger("End");
            audioSource.PlayOneShot(winSound, volumeScale:2f);
            StartCoroutine(LoadNextLevel(time));
        }
    }

    private void FixedUpdate()
    {
        if (startedLoading)
        {
            _player.rb.AddForce((transform.position - _player.transform.position)*200f, ForceMode2D.Force);
        }
    }

    IEnumerator LoadNextLevel(float time){
        
        yield return new WaitForSeconds(time);
        SceneManager.LoadSceneAsync((SceneManager.GetActiveScene().buildIndex + 1)
                                   % SceneManager.sceneCountInBuildSettings);
        transition.SetTrigger("Start");
    }
}