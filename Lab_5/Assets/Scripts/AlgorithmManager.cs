using Assets.Scripts.GraphComponents;
using Assets.Scripts.GraphTraversal;
using Assets.Scripts.ShortestPath;
using Assets.Scripts.SpanningTree;
using Assets.Scripts.TransportNetwork;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.GraphCopyManager;

namespace Assets.Scripts
{
    public class AlgorithmManager : MonoBehaviour
    {
        public static TextMeshProUGUI Description;
        public static Button NextStepButton;
        public static int CurrentStepNum;
        public static int CurrentStepsCount;
        public static List<TraversalStep> TraversalSteps;
        public static List<ShortestPathStep> ShortestPathSteps;
        public static List<SpanningTreeStep> SpanningTreeSteps;
        public static List<TransportStep> TransportSteps;

        public static void DoTraversalStep()
        {
            if (TraversalSteps == null) return;
            if (TraversalSteps.Count == 0)
            {
                ControlStepButton();
                return;
            }
            TraversalStep currentStep = TraversalSteps[CurrentStepNum];
            if (currentStep.LightedOffLines != null)
                foreach (Line line in currentStep.LightedOffLines)
                    line.LightOff();
            if (currentStep.LightedOnLines != null)
                foreach (Line line in currentStep.LightedOnLines)
                    line.LightOn();
            Description.text = currentStep.DescNext;
            CurrentStepNum++;
            ControlStepButton();
        }

        public static void DoShortestPathStep()
        {
            if (ShortestPathSteps == null) return;
            if (ShortestPathSteps.Count == 0)
            {
                ControlStepButton();
                return;
            }
            ShortestPathStep currentStep = ShortestPathSteps[CurrentStepNum];
            if (currentStep.LightedOffLines != null)
                foreach (Line line in currentStep.LightedOffLines)
                    line.LightOff();
            if (currentStep.LightedOnLines != null)
                foreach (Line line in currentStep.LightedOnLines)
                    line.LightOn();
            if (currentStep.PointShortestPathPairs != null)
            {
                foreach (KeyValuePair<Point, int> pair in currentStep.PointShortestPathPairs)
                {
                    Transform background = pair.Key.PointObj.transform.Find("Background");
                    background.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = pair.Value.ToString();
                }
            }
            Description.text = currentStep.DescNext;
            CurrentStepNum++;
            ControlStepButton();
        }

        public static void DoSpanningTreeStep()
        {
            List<SpanningTreeStep> pyp = SpanningTreeSteps;
            if (SpanningTreeSteps == null) return;
            if (SpanningTreeSteps.Count == 0)
            {
                ControlStepButton();
                return;
            }
            SpanningTreeStep currentStep = SpanningTreeSteps[CurrentStepNum];
            if (currentStep.LightedOffLines != null)
                foreach (Line line in currentStep.LightedOffLines)
                    line.LightOff();
            if (currentStep.LightedOnLines != null)
                foreach (Line line in currentStep.LightedOnLines)
                    line.LightOn();
            if (currentStep.RemovedLines != null)
                foreach (Line line in currentStep.RemovedLines)
                    RemoveLine(line);
            Description.text = currentStep.DescNext;
            CurrentStepNum++;
            ControlStepButton();
        }

        private static void RemoveLine(Line line)
        {
            GraphCopy.RemoveLine(line, true);
            GraphCopy.UpdateLinesNames();
            Destroy(line.LineObj);
        }

        public static void DoTransportStep()
        {
            if (TransportSteps == null) return;
            if (TransportSteps.Count == 0)
            {
                ControlStepButton();
                return;
            }
            TransportStep currentStep = TransportSteps[CurrentStepNum];
            if (currentStep.LightedOffLines != null)
                foreach (Line line in currentStep.LightedOffLines)
                    line.LightOff();
            if (currentStep.LightedOnLines != null)
                foreach (Line line in currentStep.LightedOnLines)
                    line.LightOn();
            if (currentStep.LineUsedNumOfUnitsPairs != null)
            {
                foreach (KeyValuePair<Line, int> pair in currentStep.LineUsedNumOfUnitsPairs)
                {
                    string newWeight = pair.Value.ToString() + '/' + pair.Key.Weight[(pair.Key.Weight.IndexOf('/') + 1)..];
                    pair.Key.Weight = newWeight;
                    pair.Key.WeightIF.text = newWeight;
                }
            }
            Description.text = currentStep.DescNext;
            CurrentStepNum++;
            ControlStepButton();
        }

        public static void ControlStepButton()
        {
            if (NextStepButton.interactable && CurrentStepNum == CurrentStepsCount)
                NextStepButton.interactable = false;
        }
    }
}
