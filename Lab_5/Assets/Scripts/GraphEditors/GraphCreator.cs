using Assets.Scripts.GraphComponents;
using UnityEngine;

namespace Assets.Scripts.GraphEditors
{
    public class ObjectPlacement : MonoBehaviour
    {
        public GameObject PointPrefab;
        public Canvas Canvas;
        public GameObject LinePrefab;
        public static Graph Graph;
        public GameObject PointMovement;

        private Line curLine;

        private void OnEnable() => PointMovement.SetActive(false);

        private void Start() => Graph = new Graph();

        private void Update()
        {
            Vector3 worldPlacementPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPlacementPos.z = 0;

            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    GameObject go = hit.collider.gameObject;
                    if (go.CompareTag("Point"))
                    {
                        int pointIndex = char.Parse(go.name) - 'A';
                        Point point = Graph.Points[pointIndex];
                        if (curLine == null)
                        {
                            GameObject newLine = Instantiate(LinePrefab, worldPlacementPos, Quaternion.identity, Canvas.transform);
                            Graph.AddLine(newLine, point, ref curLine);
                            return;
                        }

                        Line line = new(curLine.StartPoint, point);
                        if (point == curLine.StartPoint) return;
                        if (Graph.Lines.Contains(line))
                        {
                            curLine.GoToTheAnotherPoint(point);
                        }
                        else
                        {
                            curLine.SetEndPoint(point);
                            GameObject newLine = Instantiate(LinePrefab, worldPlacementPos, Quaternion.identity, Canvas.transform);
                            Graph.AddLine(newLine, point, ref curLine);
                        }
                    }
                }
                else
                {
                    if (Graph.Points.Count > 0 && curLine == null) return;
                    else if (Graph.Points.Count >= Graph.MaxPointsCount) return;

                    GameObject newPointGO = Instantiate(PointPrefab, worldPlacementPos, Quaternion.identity, Canvas.transform);
                    Graph.AddPoint(newPointGO);
                    curLine?.SetEndPoint(Graph.Points[^1]);
                    GameObject newLine = Instantiate(LinePrefab, worldPlacementPos, Quaternion.identity, Canvas.transform);
                    Graph.AddLine(newLine, Graph.Points[^1], ref curLine);
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                PointMovement.SetActive(true);
                gameObject.SetActive(false);
            }
            else if (curLine != null)
            {
                Point curPoint = new Point(worldPlacementPos);
                curLine.SetEndPoint(curPoint);
            }
        }

        private void OnDisable()
        {
            if (curLine == null) return;
            Destroy(curLine.LineObj);
            Graph.Lines.Remove(curLine);
            curLine = null;
            Graph.SetLinkedLines();
        }
    }
}