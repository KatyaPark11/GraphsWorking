using Assets.Scripts.GraphComponents;
using Assets.Scripts.GraphTraversal;
using Assets.Scripts.ShortestPath;
using Assets.Scripts.SpanningTree;
using Assets.Scripts.TransportNetwork;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.AlgorithmManager;
using static Assets.Scripts.GraphCopyManager;
using static Assets.Scripts.VarsHolder;

namespace Assets.Scripts
{
    public class AlgorithmDownloading : MonoBehaviour
    {
        public void DownloadDFS()
        {
            NextStepButton.onClick.RemoveAllListeners();
            NextStepButton.onClick.AddListener(DoTraversalStep);
            TraversalSteps = TraversalStepsGetting.GetDepthFirstSteps(GraphCopy);
            CurrentStepsCount = TraversalSteps.Count;
            CurrentStepNum = 0;
        }

        public void DownloadBFS()
        {
            NextStepButton.onClick.RemoveAllListeners();
            NextStepButton.onClick.AddListener(DoTraversalStep);
            TraversalSteps = TraversalStepsGetting.GetBreadthFirstSteps(GraphCopy);
            CurrentStepsCount = TraversalSteps.Count;
            CurrentStepNum = 0;
        }

        public void DownloadShortestPath()
        {
            NextStepButton.onClick.RemoveAllListeners();
            NextStepButton.onClick.AddListener(DoShortestPathStep);
            foreach (Point point in GraphCopy.Points)
            {
                GameObject backgroundGO = new("Background");
                Image background = backgroundGO.AddComponent<Image>();
                background.transform.SetParent(point.PointObj.transform);
                background.GetComponent<RectTransform>().sizeDelta = new Vector2(15f, 15f);
                backgroundGO.transform.SetParent(point.PointObj.transform);
                background.transform.position = point.PointObj.transform.position + new Vector3(0, 20f, 0);

                GameObject textObject = new("TextOverObject");
                TextMeshProUGUI textMesh = textObject.AddComponent<TextMeshProUGUI>();
                textMesh.text = "0";
                textMesh.alignment = TextAlignmentOptions.Center;
                textMesh.fontSize = 10;
                textMesh.color = Color.blue;

                textObject.transform.SetParent(backgroundGO.transform);
                textMesh.transform.position = backgroundGO.transform.position;
            }
            ShortestPathSteps = ShortestPathStepsGetting.GetShortestPathSteps(GraphCopy, FromPointIndex, ToPointIndex);
            CurrentStepsCount = ShortestPathSteps.Count;
            CurrentStepNum = 0;
        }

        public void DownloadSpanningTree()
        {
            NextStepButton.onClick.RemoveAllListeners();
            NextStepButton.onClick.AddListener(DoSpanningTreeStep);
            SpanningTreeSteps = SpanningTreeStepsGetting.GetSpanningTreeSteps(GraphCopy);
            CurrentStepsCount = SpanningTreeSteps.Count;
            CurrentStepNum = 0;
        }

        public void DownloadTransportNetwork()
        {
            NextStepButton.onClick.RemoveAllListeners();
            NextStepButton.onClick.AddListener(DoTransportStep);
            TransportSteps = TransportStepsGetting.GetTransportSteps(GraphCopy, SourcePointIndex, SinkPointIndex);
            CurrentStepsCount = TransportSteps.Count;
            CurrentStepNum = 0;
        }
    }
}
