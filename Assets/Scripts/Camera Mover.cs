using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMover : MonoBehaviour
{
    public GameObject player;
    public float grapplingHookWeight = 0.5f;
    private Vector3 newCameraPosition;
    private Vector3 coursorPosition;
    private void Update() {
        coursorPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        newCameraPosition = player.transform.position + coursorPosition*grapplingHookWeight;
        newCameraPosition.y = Mathf.Max(0f, newCameraPosition.y);
        transform.position = new Vector3(newCameraPosition.x, newCameraPosition.y, transform.position.z);
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    IEnumerator Shaking(float duration, AnimationCurve curve){
        float startTime = Time.time;
        while(Time.time - startTime <= duration){
            float power = curve.Evaluate((Time.time - startTime)/duration);
            transform.position += Random.insideUnitSphere*power;
            yield return null;
        }
    }
    public void ScreenShake(float duration, AnimationCurve curve){
        StartCoroutine(Shaking(duration, curve));
    }
}
