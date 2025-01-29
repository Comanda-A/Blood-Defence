using UnityEngine;
using UnityEngine.SceneManagement;
using WayOfBlood.Character.Player;

namespace WayOfBlood.GameManager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] GameObject DefeatScreen;

        private PlayerController playerController;

        private void Start()
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            playerController.OnDeath += OnDeathPlayer;
        }
        
        private void OnDeathPlayer()
        {
            DefeatScreen.SetActive(true);
        }

        public void RestartGame()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }

        private void OnDestroy()
        {
            playerController.OnDeath -= OnDeathPlayer;
        }
    }
}
