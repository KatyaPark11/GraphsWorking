using Assets.Scripts.Controllers;
using Assets.Scripts.GraphComponents;
using UnityEngine;
using static Assets.Scripts.VarsHolder;

namespace Assets.Scripts.GraphEditors
{
    public class PointMovement : BaseEditor
    {
        private bool isObjectSelected = false;
        private RaycastHit hit;
        private Point draggingPoint;

        private void Update()
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
                        isObjectSelected = true;
                    }                
                }
            }

            if (isObjectSelected)
            {
                if (AreaController.IsUnreachableArea())
                    foreach (Line line in draggingPoint.LinkedLines)
                        line.SetLineColor(Color.gray);
                else
                    foreach (Line line in draggingPoint.LinkedLines)
                        line.SetLineColor(Color.red);

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
                MeshColliderController.UpdateMeshColliders();
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (draggingPoint != null && AreaController.CompareColor(draggingPoint.LinkedLines[0].LineRenderer.startColor, Color.gray))
                {
                    return;
                }
                else if (isObjectSelected)
                {
                    draggingPoint = null;
                    isObjectSelected = false;
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                gameObject.SetActive(false);
            }
        }
    }
}