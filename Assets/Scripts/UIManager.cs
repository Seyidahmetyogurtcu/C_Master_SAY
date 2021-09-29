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
        public static UIManager singleton;
        public Text playersText;
        public float playersCount;
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject inGamePanel;
        [SerializeField] private GameObject losePanel;
        private void Awake()
        {
            singleton = this;
            Time.timeScale = 0;
        }
        private void Update()
        {
            WaitAndupdate();
        }
        void  WaitAndupdate()
        {
            //Persons Count
            int numberOfPerson = PlayerManager.singleton.persons.Count;
            string personCount = numberOfPerson.ToString();
            Text temp = FindObjectOfType<PlayerManager>().GetComponentInChildren<Text>();
            temp.text = personCount;

            //Enemies Count
            for (int i = 0; i < GameObject.FindGameObjectsWithTag("EnemyZone").Length; i++)
            {
                int numberOfEnemy = EnemyManager.singleton.enemiesGroupArray[i].enemies.Count;
                string enemyCount = numberOfEnemy.ToString();
                Text[] temp2 = FindObjectOfType<EnemyManager>().GetComponentsInChildren<Text>();
                temp2[i].text = enemyCount;
            }


        }

        public void PlayButton()
        {
            Time.timeScale = 1;
            GameObject.Find("Play Button").SetActive(false);
        }
        public void NextLevelButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        public void RestartButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void ExitButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        public void Lose()
        {
            //Set you lose panel
            Time.timeScale = 0;
            inGamePanel.SetActive(false);
            losePanel.SetActive(true);
        }

        public void WinLevel()
        {
            //Set you win panel
            Time.timeScale = 0;
            inGamePanel.SetActive(false);
        }
        public void WinGame()
        {
            //Set credits Panel
            Time.timeScale = 0;
            inGamePanel.SetActive(false);
            winPanel.SetActive(true);
        }
    }
}

