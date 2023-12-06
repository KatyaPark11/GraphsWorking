using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class BackSceneJump : MonoBehaviour
    {
        public void GoBackScene() => SceneChanger.ChangeTheScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}