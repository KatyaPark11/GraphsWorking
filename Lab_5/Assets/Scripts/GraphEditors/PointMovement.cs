using UnityEngine;
using Assets.Scripts.GraphComponents;
using static Assets.Scripts.VarsHolder;

namespace Assets.Scripts.GraphEditors
{
    public class PointMovement : MonoBehaviour
    {
        private bool isObjectSelected = false;
        private RaycastHit hit;
        private Point draggingPoint;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.CompareTag("Point"))
                    {
                        GameObject go = hit.collider.gameObject;
                        int pointIndex = char.Parse(go.name) - 'A';
                        draggingPoint = MainGraph.Points[pointIndex];
                    }
                    isObjectSelected = true;
                }
            }

            if (isObjectSelected)
            {
                Vector3 worldPlacementPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPlacementPos.z = 0;

                draggingPoint.PointObj.transform.position = worldPlacementPos;
                draggingPoint.Position = worldPlacementPos;
                foreach (Line line in draggingPoint.LinkedLines)
                {
                    if (draggingPoint.IsStartPoint(line))
                        line.SetStartPoint(draggingPoint);
                    else
                        line.SetEndPoint(draggingPoint);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (isObjectSelected)
                    isObjectSelected = false;
            }
        }
    }
}