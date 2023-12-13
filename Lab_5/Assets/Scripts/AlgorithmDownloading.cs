using Assets.Scripts.GraphTraversal;
using Assets.Scripts.SpanningTree;
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
            TurnOffExtraObjs();
            ShortestPathBegginer.SetActive(true);
            Transform fromText = ShortestPathBegginer.transform.Find("FromText");
            fromText.gameObject.SetActive(true);
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
            TurnOffExtraObjs();
            TransportNetworkBegginer.SetActive(true);
            Transform sourceText = TransportNetworkBegginer.transform.Find("SourceText");
            sourceText.gameObject.SetActive(true);
        }

        private void TurnOffExtraObjs()
        {
            foreach (Button button in Buttons)
                if (button.interactable)
                    button.interactable = false;
            if (GraphType.interactable)
                GraphType.interactable = false;
        }
    }
}
