using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Count_Master_SAY.Control;
using Count_Master_SAY.Level;
using Count_Master_SAY.Trigger;


namespace Count_Master_SAY.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager singleton;
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject inGamePanel;
        [SerializeField] private GameObject losePanel;
        LevelGenerator levelGenerator;
        public GameObject levvel;
        private void Awake()
        {
            singleton = this;
            levelGenerator = LevelGenerator.singleton;
            Time.timeScale = 0;

        }
        private void Update()
        {
            WaitAndupdate();
        }
        void WaitAndupdate()
        {
            //Persons Count
            if (PlayerManager.singleton)
            {
                int numberOfPerson = PlayerManager.singleton.persons.Count;
                string personCount = numberOfPerson.ToString();
                Text text = FindObjectOfType<PlayerManager>().GetComponentInChildren<Text>();
                text.text = personCount;
            }

            //Enemies Count
            if (EnemyManager.singleton)
            {
                for (int i = 0; i < GameObject.FindGameObjectsWithTag(Triggers.EnemyZone).Length; i++)
                {
                    int numberOfEnemy = EnemyManager.singleton.enemiesGroupArray[i].enemies.Count;
                    string enemyCount = numberOfEnemy.ToString();
                    Text[] text2 = FindObjectOfType<EnemyManager>().GetComponentsInChildren<Text>();
                    text2[i].text = enemyCount;
                }
            }
        }

        public void PlayButton()
        {
            Time.timeScale = 1;
            GameObject.Find("Play Button").SetActive(false);
        }
        public void NextLevelButton()
        {
            Destroy(levelGenerator.levels[levelGenerator.currentLevel]);
            levelGenerator.currentLevel++;
            Instantiate(levelGenerator.levels[levelGenerator.currentLevel], levelGenerator.transform);

            inGamePanel.SetActive(true);
            #region old code
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            #endregion
        }
        public void RestartButton()
        {
            Destroy(levelGenerator.levels[levelGenerator.currentLevel]);
            Instantiate(levelGenerator.levels[levelGenerator.currentLevel], levelGenerator.transform);
            #region old code
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            #endregion
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
            winPanel.SetActive(true);
        }
    }
}

