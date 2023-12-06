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
        private AreaController areaController;

        private void Start() => areaController = new();

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject hitGO = hit.collider.gameObject;
                    if (hitGO.CompareTag("Point"))
                        SetDraggingPoint(hitGO);
                }
            }

            if (isObjectSelected)
            {
                areaController.SetCurAreaColor(draggingPoint);
                Vector3 worldPlacementPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPlacementPos.z = 0;

                draggingPoint.PointObj.transform.position = worldPlacementPos;
                draggingPoint.Position = worldPlacementPos;
                draggingPoint.SetLinkedLinesPos();
                MeshColliderController.UpdateMeshColliders();
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (draggingPoint != null && Line.CompareColor(draggingPoint.LinkedLines[0].LineRenderer.startColor, Color.gray))
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

        private void SetDraggingPoint(GameObject hitGO)
        {
            int pointIndex = char.Parse(hitGO.name) - 'A';
            draggingPoint = MainGraph.Points[pointIndex];
            isObjectSelected = true;
        }
    }
}