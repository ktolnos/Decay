using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver: MonoBehaviour
{
    public AudioClip looseSound;
    public GameObject effect;
    public float time;
    private Animator transition;
    private AudioSource audioSource;
    private bool StartedReloading = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        transition = Camera.main.GetComponentInChildren<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(StartedReloading){
            return;
        }
        StartedReloading = true;
        transition.SetTrigger("End");
        if(other.gameObject.layer == LayerMask.NameToLayer("Damaging")){
            Destroy(Instantiate(effect, transform),1);
            audioSource.PlayOneShot(looseSound);
            StartCoroutine(ReloadLevel(time));
        }
    }
    IEnumerator ReloadLevel(float time){
        
        yield return new WaitForSeconds(time);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        transition.SetTrigger("Start");
    }
}