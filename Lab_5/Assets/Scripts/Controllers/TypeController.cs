using Assets.Scripts.GraphComponents;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Assets.Scripts.VarsHolder;

namespace Assets.Scripts.Controllers
{
    public class TypeController : MonoBehaviour
    {
        private static TMP_Dropdown GraphType;

        private void Start()
        {
            GraphType = GetComponent<TMP_Dropdown>();
            GraphType.onValueChanged.AddListener(new UnityAction<int>(ChangeType));
        }

        public static void ChangeType(int value)
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

        private static void SetSimpleType()
        {
            MainGraph.Type = "Обычный граф";
            foreach (Line line in MainGraph.Lines)
            {
                line.WeightIF.gameObject.SetActive(false);
                line.ArrowRenderer.gameObject.SetActive(false);
                line.WeightIF.onEndEdit.RemoveAllListeners();
            }
            InteractSimpleGraph();
        }

        public static void InteractSimpleGraph()
        {
            foreach (KeyValuePair<string, Button> pair in ButNameAlgorithmLauncherMap)
                if (pair.Value.interactable)
                    pair.Value.interactable = false;
        }

        private static void SetWeightedType()
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
                if (int.TryParse(secondNum, out int num) && num >= 0 && num <= 99)
                {
                    line.Weight = num.ToString();
                    weightIF.text = num.ToString();
                }

                weightIF.onEndEdit.RemoveAllListeners();
                weightIF.onEndEdit.AddListener(delegate { OnWeightedLineWeightValueChanged(line); });
                line.UpdateInputFieldPosition();
            }
            MainGraph.Type = "Взвешенный граф";
            InteractWeightedGraph();
        }

        public static void InteractWeightedGraph()
        {
            foreach (KeyValuePair<string, Button> pair in ButNameAlgorithmLauncherMap)
            {
                if (pair.Key.Equals("TransportNetwork") && pair.Value.interactable)
                    pair.Value.interactable = false;
                else if (!pair.Key.Equals("TransportNetwork"))
                    pair.Value.interactable = true;
            }
        }

        public static void OnWeightedLineWeightValueChanged(Line line)
        {
            TMP_InputField weightIF = line.WeightIF;
            string value = weightIF.text;
            if (int.TryParse(value, out int weight) && weight >= 0 && weight <= 99)
                line.Weight = value;
            if (line.Weight != value)
                weightIF.text = line.Weight;
        }

        private static void SetTransportNetworkType()
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
                weightIF.onEndEdit.AddListener(delegate { OnTransportWeightValueChanged(line); });

                if (MainGraph.Type.Equals("Обычный граф"))
                {
                    weightIF.gameObject.SetActive(true);
                    line.ArrowRenderer.gameObject.SetActive(true);
                }
                line.UpdateInputFieldPosition();
            }
            MainGraph.Type = "Транспортная сеть";
            InteractTransportNetwork();
        }

        public static void InteractTransportNetwork()
        {
            foreach (KeyValuePair<string, Button> pair in ButNameAlgorithmLauncherMap)
            {
                if ((pair.Key.Equals("AbjMatrix") || pair.Key.Equals("ShortestPath")) && pair.Value.interactable)
                    pair.Value.interactable = false;
                else if (!pair.Key.Equals("AbjMatrix") && !pair.Key.Equals("ShortestPath"))
                    pair.Value.interactable = true;
            }
        }

        public static void OnTransportWeightValueChanged(Line line)
        {
            TMP_InputField weightIF = line.WeightIF;
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
                    if (usedNumOfUnits >= 0 && maxNumOfUnits <= 99 && usedNumOfUnits <= maxNumOfUnits)
                        line.Weight = input;
        }

        private static bool IsTransportFormat(string input)
        {
            string[] parts = input.Split('/');
            return parts.Length == 2 && int.TryParse(parts[0], out _) && int.TryParse(parts[1], out _);
        }
    }
}
