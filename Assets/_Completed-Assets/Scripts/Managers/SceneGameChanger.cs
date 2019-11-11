using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGameChanger : MonoBehaviour
{
    GameObject[] players;
    public AudioSource m_Audio;
    public AudioClip m_Music;

    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Tank");
        m_Audio.clip = m_Music;
        m_Audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (players.Length < 2)
        {
            foreach (GameObject pw in players)
            {
                pw.SetActive(false);
            }
        }
    }
}
