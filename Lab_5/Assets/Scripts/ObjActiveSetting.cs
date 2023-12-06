using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.VarsHolder;

namespace Assets.Scripts
{
    public class ObjActiveSetting : MonoBehaviour
    {
        public GameObject obj;
        public Button Button;

        public void SetObjActive()
        {
            if (obj.activeSelf) obj.SetActive(false);
            else obj.SetActive(true);
        }
    }
}
