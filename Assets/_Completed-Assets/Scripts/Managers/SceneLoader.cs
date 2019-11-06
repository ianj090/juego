using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            Debug.Log("recognized key");
            SceneManager.LoadScene(1);
            Debug.Log("correctly changed scene");
        }
    }
}
