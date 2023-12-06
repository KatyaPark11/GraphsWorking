using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class SceneChanger
    {
        public static void ChangeTheScene(int scene) => SceneManager.LoadScene(scene);
    }
}