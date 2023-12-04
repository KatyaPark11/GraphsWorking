using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ObjActiveSetting : MonoBehaviour
    {
        public GameObject obj;
        public Button[] OtherButtons;

        public void SetObjActive()
        {
            if (obj.activeSelf)
            {
                obj.SetActive(false);
                foreach (Button button in OtherButtons)
                {
                    if (!button.interactable)
                        button.interactable = true;
                }
            }

            else
            {
                obj.SetActive(true);
                foreach (Button button in OtherButtons)
                {
                    if (button.interactable)
                        button.interactable = false;
                }
            }
        }
    }
}
