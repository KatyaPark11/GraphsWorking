using Assets.Scripts.Controllers;
using Assets.Scripts.ShortestPath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
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
                    if (go.CompareTag("Point"))
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
    }
}
