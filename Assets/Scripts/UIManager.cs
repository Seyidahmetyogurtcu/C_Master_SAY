using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Count_Master_SAY.Control;

namespace Count_Master_SAY.UI
{
    public class UIManager : MonoBehaviour
    {
        public Text playersText;
        public float playersCount;
        private void Awake()
        {
            Time.timeScale = 0;
        }
        private void Update()
        {
            StartCoroutine(WaitAndupdate());

        }
        IEnumerator WaitAndupdate()
        {
            int numberOfPerson = PlayerManager.singleton.persons.Count;
            string personCount = numberOfPerson.ToString();
            Text temp = FindObjectOfType<PlayerManager>().GetComponentInChildren<Text>();
            temp.text = personCount;
            for (int i = 0; i < GameObject.FindGameObjectsWithTag("EnemyZone").Length; i++)
            {
                int numberOfEnemy = EnemyManager.singleton.enemiesGroupArray[i].enemies.Count;
                string enemyCount = numberOfEnemy.ToString();
                Text[] temp2 = FindObjectOfType<EnemyManager>().GetComponentsInChildren<Text>();
                temp2[i].text = enemyCount;
            }
          



            yield return new WaitForSeconds(0.5f);


        }

        public void Play()
        {
            Time.timeScale = 1;
            GameObject.Find("Play Button").SetActive(false);
        }
        public void NextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}

