using UnityEngine;

namespace Assets.Scripts
{
    public class ObjActiveSetting : MonoBehaviour
    {
        public GameObject obj;

        public void SetObjActive() => obj.SetActive(true);
    }
}
