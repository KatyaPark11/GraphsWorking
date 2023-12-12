using UnityEngine;
using static Assets.Scripts.VarsHolder;

namespace Assets.Scripts.TransportNetwork
{
    public class SourcePointIndexGetting : MonoBehaviour
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
                        SourcePointIndex = MainGraph.GetPointIndex(go);
                        Transform sinkText = TransportNetworkBegginer.transform.Find("SinkText");
                        sinkText.gameObject.SetActive(true);
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
