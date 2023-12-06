using UnityEngine;

namespace Assets.Scripts
{
    public class ObjActiveSetting : MonoBehaviour
    {
        public GameObject obj;

        public void SetObjActive()
        {
            if (obj.activeSelf) obj.SetActive(false);
            else obj.SetActive(true);
        }
    }
}
