using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Controllers
{
    public class TypeController : MonoBehaviour
    {
        public TMP_Dropdown GraphType;

        private void Start()
        {
            GraphType.onValueChanged.AddListener(new UnityAction<int>(ChangeType));
        }

        private void ChangeType(int value)
        {
            string selectedType = GraphType.options[value].text;

            switch (selectedType)
            {
                case "Обычный граф":
                    SetSimpleType();
                    break;
                case "Взвешенный граф":
                    SetWeightedType();
                    break;
                case "Транспортная сеть":
                    SetTransportNetworkType();
                    break;
            }
        }

        private void SetSimpleType()
        {

        }

        private void SetWeightedType()
        {

        }

        private void SetTransportNetworkType()
        {

        }
    }
}
