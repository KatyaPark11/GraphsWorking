using Assets.Scripts.GraphComponents;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Windows;
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
            MainGraph.Type = "Обычный граф";
            foreach (Line line in MainGraph.Lines)
            {
                line.WeightIF.gameObject.SetActive(false);
                line.ArrowRenderer.gameObject.SetActive(false);
                line.WeightIF.onEndEdit.RemoveAllListeners();
            }
        }

        private void SetWeightedType()
        {
            foreach (Line line in MainGraph.Lines)
            {
                TMP_InputField weightIF = line.WeightIF;
                if (MainGraph.Type.Equals("Обычный граф"))
                {
                    weightIF.gameObject.SetActive(true);
                    line.ArrowRenderer.gameObject.SetActive(true);
                }

                string weight = line.Weight;
                string secondNum = weight[(weight.IndexOf('/') + 1)..];
                if (int.TryParse(secondNum, out int num))
                {
                    line.Weight = num.ToString();
                    weightIF.text = num.ToString();
                }

                weightIF.onEndEdit.RemoveAllListeners();
                weightIF.onEndEdit.AddListener(delegate { OnWeightedLineWeightValueChanged(line.WeightIF, line); });
            }
            MainGraph.Type = "Взвешенный граф";
        }

        public static void OnWeightedLineWeightValueChanged(TMP_InputField weightIF, Line line)
        {
            string value = weightIF.text;
            if (int.TryParse(value, out _))
                line.Weight = value;
            if (line.Weight != value)
                weightIF.text = line.Weight;
        }

        private void SetTransportNetworkType()
        {
            foreach (Line line in MainGraph.Lines)
            {
                TMP_InputField weightIF = line.WeightIF;
                if (!IsTransportFormat(line.Weight))
                {
                    string newWeightFormat = $"0/{line.Weight}";
                    line.Weight = newWeightFormat;
                    weightIF.text = newWeightFormat;
                }

                weightIF.onEndEdit.RemoveAllListeners();
                weightIF.onEndEdit.AddListener(delegate { OnTransportWeightValueChanged(weightIF, line); });

                if (MainGraph.Type.Equals("Обычный граф"))
                {
                    weightIF.gameObject.SetActive(true);
                    line.ArrowRenderer.gameObject.SetActive(true);
                }
            }
            MainGraph.Type = "Транспортная сеть";
        }

        public static void OnTransportWeightValueChanged(TMP_InputField weightIF, Line line)
        {
            string value = weightIF.text;
            FilterTransportWeightInput(value, line);
            if (line.Weight != value)
                weightIF.text = line.Weight;
        }

        private static void FilterTransportWeightInput(string input, Line line)
        {
            string[] parts = input.Split('/');

            if (parts.Length == 2)
                if (int.TryParse(parts[0], out int usedNumOfUnits) && int.TryParse(parts[1], out int maxNumOfUnits))
                    if (usedNumOfUnits <= maxNumOfUnits)
                        line.Weight = input;
        }

        private bool IsTransportFormat(string input)
        {
            string[] parts = input.Split('/');
            return parts.Length == 2 && int.TryParse(parts[0], out _) && int.TryParse(parts[1], out _);
        }
    }
}
