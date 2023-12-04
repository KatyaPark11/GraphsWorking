using Assets.Scripts.Controllers;
using Assets.Scripts.GraphComponents;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.Controllers.TypeController;
using static Assets.Scripts.VarsHolder;

namespace Assets.Scripts.GraphEditors
{
    public class ObjectPlacement : BaseEditor
    {
        public GameObject PointPrefab;
        public GameObject LinePrefab;
        public Canvas PointsCanvas;
        public Canvas LinesCanvas;

        private Line curLine;

        private void Update()
        {
            Vector3 worldPlacementPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPlacementPos.z = 0;

            if (Input.GetMouseButtonUp(0))
            {
                if (AreaController.IsUnreachableArea())
                {
                    curLine?.SetLineColor(Color.gray);
                    return;
                }
                else
                {
                    curLine?.SetLineColor(Color.red);
                }

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    GameObject go = hit.collider.gameObject;
                    if (go.CompareTag("Point"))
                    {
                        Point point = MainGraph.GetPoint(go);
                        if (curLine == null)
                        {
                            GameObject newLine = Instantiate(LinePrefab, worldPlacementPos, Quaternion.identity, LinesCanvas.transform);
                            MainGraph.AddLine(newLine, point, ref curLine);
                            return;
                        }

                        Line line = new(curLine.StartPoint, point);
                        if (point == curLine.StartPoint) return;
                        if (MainGraph.Lines.Contains(line))
                        {
                            curLine.GoToTheAnotherPoint(point);
                        }
                        else
                        {
                            curLine.SetEndPoint(point);
                            GameObject newLine = Instantiate(LinePrefab, worldPlacementPos, Quaternion.identity, LinesCanvas.transform);
                            MainGraph.AddLine(newLine, point, ref curLine);
                            AddListener();
                        }
                    }
                }
                else
                {
                    if (MainGraph.Points.Count > 0 && curLine == null) return;
                    else if (MainGraph.Points.Count >= Graph.MaxPointsCount) return;

                    GameObject newPointGO = Instantiate(PointPrefab, worldPlacementPos, Quaternion.identity, PointsCanvas.transform);
                    MainGraph.AddPoint(newPointGO);
                    curLine?.SetEndPoint(MainGraph.Points[^1]);
                    GameObject newLine = Instantiate(LinePrefab, worldPlacementPos, Quaternion.identity, LinesCanvas.transform);
                    MainGraph.AddLine(newLine, MainGraph.Points[^1], ref curLine);
                    AddListener();
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                gameObject.SetActive(false);
            }
            else if (curLine != null)
            {
                if (AreaController.IsUnreachableArea())
                    curLine.SetLineColor(Color.gray);
                else
                    curLine.SetLineColor(Color.red);

                Point curPoint = new(worldPlacementPos);
                curLine.SetEndPoint(curPoint);
            }
        }

        

        private void AddListener()
        {
            if (MainGraph.Type.Equals("Взвешенный граф"))
                curLine.WeightIF.onEndEdit.AddListener(delegate { OnWeightedLineWeightValueChanged(curLine.WeightIF, curLine); });
            else if (MainGraph.Type.Equals("Транспортная сеть"))
                curLine.WeightIF.onEndEdit.AddListener(delegate { OnTransportWeightValueChanged(curLine.WeightIF, curLine); });
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (MainGraph.Points.Count == 1 && MainGraph.Points[0].LinkedLines.Count == 0)
            {
                Destroy(MainGraph.Points[0].PointObj);
                MainGraph.Points.Clear();
            }
            if (curLine == null) return;
            Destroy(curLine.LineObj);
            MainGraph.Lines.Remove(curLine);
            MeshColliderController.UpdateMeshColliders();
            curLine = null;
            MainGraph.SetLinkedLines();
        }
    }
}