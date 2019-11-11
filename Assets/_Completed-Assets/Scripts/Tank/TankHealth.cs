using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Complete
{
    public class TankHealth : MonoBehaviour
    {
        public int m_PlayerNumber = 1;
        public float m_StartingHealth = 100f;               // The amount of health each tank starts with.
        public Slider m_Slider;                             // The slider to represent how much health the tank currently has.
        public Image m_FillImage;                           // The image component of the slider.
        public Color m_FullHealthColor = Color.green;       // The color the health bar will be when on full health.
        public Color m_ZeroHealthColor = Color.red;         // The color the health bar will be when on no health.
        public GameObject m_ExplosionPrefab;                // A prefab that will be instantiated in Awake, then used whenever the tank dies.
        public Text winText;
        public Text name1;
        public Text name2;


        private AudioSource m_ExplosionAudio;               // The audio source to play when the tank explodes.
        private ParticleSystem m_ExplosionParticles;        // The particle system the will play when the tank is destroyed.
        private float m_CurrentHealth;                      // How much health the tank currently has.
        private bool m_Dead;                                // Has the tank been reduced beyond zero health yet?


        private void Awake ()
        {
            // Instantiate the explosion prefab and get a reference to the particle system on it.
            m_ExplosionParticles = Instantiate (m_ExplosionPrefab).GetComponent<ParticleSystem> ();

            // Get a reference to the audio source on the instantiated prefab.
            m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource> ();

            // Disable the prefab so it can be activated when it's required.
            m_ExplosionParticles.gameObject.SetActive (false);
        }


        private void OnEnable()
        {
            if (m_PlayerNumber == 1)
            {
                Debug.Log("Player 1 Message");
                name1.text = "Player 1";
            }
            else if (m_PlayerNumber == 2)
            {
                Debug.Log("Player 2 Message");
                name2.text = "Player 2";
            }
            // When the tank is enabled, reset the tank's health and whether or not it's dead.
            m_CurrentHealth = m_StartingHealth;
            m_Dead = false;

            // Update the health slider's value and color.
            SetHealthUI();

            StartCoroutine(waiter());

        }


        public void TakeDamage (float amount)
        {
            // Reduce current health by the amount of damage done.
            m_CurrentHealth -= amount;

            // Change the UI elements appropriately.
            SetHealthUI ();

            // If the current health is at or below zero and it has not yet been registered, call OnDeath.
            if (m_CurrentHealth <= 0f && !m_Dead)
            {
                OnDeath ();
            }
        }


        private void SetHealthUI ()
        {
            // Set the slider's value appropriately.
            m_Slider.value = m_CurrentHealth;

            // Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
            m_FillImage.color = Color.Lerp (m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
        }


        private void OnDeath ()
        {
            // Set the flag so that this function is only called once.
            m_Dead = true;

            // Move the instantiated explosion prefab to the tank's position and turn it on.
            m_ExplosionParticles.transform.position = transform.position;
            m_ExplosionParticles.gameObject.SetActive (true);

            // Play the particle system of the tank exploding.
            m_ExplosionParticles.Play ();

            // Play the tank explosion sound effect.
            m_ExplosionAudio.Play();

            // Turn the tank off.
            //gameObject.SetActive (false);       //Esta no funciona (tampoco Destroy) ya que yield Wait necesita que esté activo el objeto.

            if (m_PlayerNumber == 1)
            {
                Debug.Log("Player 2 Wins");
                winText.text = "Player 2 Wins!";
            }
            else if (m_PlayerNumber == 2)
            {
                Debug.Log("Player 1 Wins");
                winText.text = "Player 1 Wins!";
            }

            GameObject[] players = GameObject.FindGameObjectsWithTag("Tank");
            for (int i = 0; i < players.Length; i++)
            {
                players[i].transform.position = new Vector3(0, 0, 30); //New Position
            }

            StartCoroutine(wait());

            
        }

        IEnumerator wait()
        {
            Debug.Log("wait() started");
            yield return new WaitForSeconds(3);
            Debug.Log("changed scene");
            SceneManager.LoadScene(0);
        }

        IEnumerator waiter()
        {
            Debug.Log("waiter() started");
            yield return new WaitForSeconds(3);
            Debug.Log("changed texts");
            name1.text = "";
            name2.text = "";
        }
    }
}