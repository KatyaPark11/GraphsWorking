using Assets.Scripts.GraphComponents;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Assets.Scripts.VarsHolder;

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
            foreach (Line line in MainGraph.Lines)
            {
                TMP_InputField weight = line.WeightIF;
                weight.onValueChanged.AddListener(delegate { OnTransportWeightValueChanged(weight); });
            }
        }

        private void OnTransportWeightValueChanged(TMP_InputField inputField)
        {
            string value = inputField.text;
            string filteredValue = FilterTransportWeightInput(value);
            if (filteredValue != value)
            {
                inputField.text = filteredValue;
            }
        }

        private string FilterTransportWeightInput(string input)
        {
            string filtered = "0/0";

            string[] parts = input.Split('/');

            if (parts.Length == 2)
            {
                if (int.TryParse(parts[0], out int usedNumOfUnits) && int.TryParse(parts[1], out int maxNumOfUnits))
                {
                    if (usedNumOfUnits <= maxNumOfUnits)
                    {
                        filtered = input;
                    }
                }
            }
            return filtered;
        }
    }
}
