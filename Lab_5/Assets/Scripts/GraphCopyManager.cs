﻿using Assets.Scripts.GraphComponents;
using Assets.Scripts.GraphTraversal;
using Assets.Scripts.ShortestPath;
using Assets.Scripts.SpanningTree;
using Assets.Scripts.TransportNetwork;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.VarsHolder;

namespace Assets.Scripts
{
    public class GraphCopyManager : MonoBehaviour
    {
        public TextMeshProUGUI Description;
        public GameObject PointPrefab;
        public GameObject LinePrefab;
        public Canvas PointsCanvas;
        public Canvas LinesCanvas;
        public Button NextStepButton;
        public static Graph GraphCopy;

        private static Graph term;

        private void OnEnable()
        {
            AlgorithmManager.Description = Description;
            AlgorithmManager.NextStepButton = NextStepButton;
            GraphCopy = new();
            for (int i = 0; i < MainGraph.Points.Count; i++)
            {
                Point point = MainGraph.Points[i];
                GameObject pointCopy = Instantiate(PointPrefab, point.PointObj.transform.position, Quaternion.identity, PointsCanvas.transform);
                GraphCopy.AddPoint(pointCopy);
                point.PointObj.SetActive(false);
            }
            for (int i = 0; i < MainGraph.Lines.Count; i++)
            {
                Line line = MainGraph.Lines[i];
                GameObject lineCopy = Instantiate(LinePrefab, line.LineObj.transform.position, Quaternion.identity, LinesCanvas.transform);
                int startPointIndex = GraphCopy.GetPointIndex(line.StartPoint.Name);
                int endPointIndex = GraphCopy.GetPointIndex(line.EndPoint.Name);
                GraphCopy.AddLine(lineCopy, GraphCopy.Points[startPointIndex], GraphCopy.Points[endPointIndex], MainGraph.Type, line.Weight);
                line.LineObj.SetActive(false);
            }
            GraphCopy.SetLinkedLines();
            foreach (Line line in GraphCopy.Lines)
                line.UpdateInputFieldPosition();
            term = MainGraph;
            MainGraph = GraphCopy;
        }

        private void OnDisable()
        {
            for (int i = 0; i < GraphCopy.Lines.Count; i++)
            {
                Line line = MainGraph.Lines[i];
                Destroy(GraphCopy.Lines[i].LineObj);
                line.LineObj.SetActive(true);
            }
            for (int i = 0; i < GraphCopy.Points.Count; i++)
            {
                Point point = MainGraph.Points[i];
                Destroy(GraphCopy.Points[i].PointObj);
                point.PointObj.SetActive(true);
            }
            GraphCopy = null;
            MainGraph = term;
            Description.text = "Туть будут пояснения к алгоритму.";
            NextStepButton.interactable = true;
        }
    }
}
