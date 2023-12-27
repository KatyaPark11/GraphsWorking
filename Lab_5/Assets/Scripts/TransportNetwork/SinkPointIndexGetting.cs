using Assets.Scripts.GraphComponents;
using UnityEngine;
using static Assets.Scripts.AlgorithmManager;
using static Assets.Scripts.GraphCopyManager;
using static Assets.Scripts.VarsHolder;

namespace Assets.Scripts.TransportNetwork
{
    public class SinkPointIndexGetting : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    GameObject go = hit.collider.gameObject;
                    if (go.CompareTag("Point") && IsSinkPoint(go))
                    {
                        SinkPointIndex = MainGraph.GetPointIndex(go);
                        TransportNetworkBegginer.SetActive(false);
                        SavableAlgActiveController.SetActive(true);
                        Buttons[SaveButIndex].interactable = true;
                        NextStepButton.onClick.RemoveAllListeners();
                        NextStepButton.onClick.AddListener(DoTransportStep);
                        TransportSteps = TransportStepsGetting.GetTransportSteps(GraphCopy, SourcePointIndex, SinkPointIndex);
                        CurrentStepsCount = TransportSteps.Count;
                        CurrentStepNum = 0;
                        gameObject.SetActive(false);
                    }
                }
            }
        }

        private static bool IsSinkPoint(GameObject pointGO)
        {
            Point point = MainGraph.GetPoint(pointGO);
            foreach (Line line in point.LinkedLines)
                if (point.IsStartPoint(line))
                    return false;
            return true;
        }
    }
}
