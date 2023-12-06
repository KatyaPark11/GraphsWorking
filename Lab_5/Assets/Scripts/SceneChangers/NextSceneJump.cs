using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class NextSceneJump : MonoBehaviour
    {
        public static void GoNextScene() => SceneChanger.ChangeTheScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}