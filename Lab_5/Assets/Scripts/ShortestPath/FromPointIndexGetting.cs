using UnityEngine;
using static Assets.Scripts.VarsHolder;

namespace Assets.Scripts.ShortestPath
{
    public class FromPointIndexGetting : MonoBehaviour
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
                        FromPointIndex = MainGraph.GetPointIndex(go);
                        Transform toText = ShortestPathBegginer.transform.Find("ToText");
                        toText.gameObject.SetActive(true);
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
