using Assets.Scripts.Controllers;
using Assets.Scripts.GraphComponents;
using UnityEngine;
using static Assets.Scripts.Controllers.TypeController;
using static Assets.Scripts.VarsHolder;

namespace Assets.Scripts.GraphEditors
{
    public class GraphCreator : BaseEditor
    {
        public GameObject PointPrefab;
        public GameObject LinePrefab;
        public Canvas PointsCanvas;
        public Canvas LinesCanvas;

        private Line curLine;
        private AreaController areaController;

        private void Start() => areaController = new();

        private void Update()
        {
            Vector3 worldPlacementPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPlacementPos.z = 0;

            if (Input.GetMouseButtonUp(0))
            {
                areaController.SetCurAreaColor(curLine);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    GameObject go = hit.collider.gameObject;
                    if (go.CompareTag("Point"))
                        DoWithExistingPoint(worldPlacementPos, go);
                }
                else
                {
                    if (MainGraph.Points.Count > 0 && curLine == null) return;
                    else if (MainGraph.Points.Count >= Graph.MaxPointsCount) return;
                    CreateLineWithNewPoint(worldPlacementPos);
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                gameObject.SetActive(false);
            }
            else if (curLine != null)
            {
                SetEndPosOfCurLine(worldPlacementPos);
            }
        }

        private void DoWithExistingPoint(Vector3 worldPlacementPos, GameObject go)
        {
            Point point = MainGraph.GetPoint(go);
            if (curLine == null)
            {
                CreateLineWithNewStart(worldPlacementPos, point);
                return;
            }

            Line line = new(curLine.StartPoint, point);
            if (point == curLine.StartPoint) return;

            if (MainGraph.Lines.Contains(line))
                curLine.GoToTheAnotherPoint(point);
            else
                CreateLineBetweenExistingPoints(worldPlacementPos, point);
        }

        private void CreateLineWithNewStart(Vector3 worldPlacementPos, Point point)
        {
            GameObject newLine = Instantiate(LinePrefab, worldPlacementPos, Quaternion.identity, LinesCanvas.transform);
            MainGraph.AddLine(newLine, point, ref curLine);
            AddListener(MainGraph.Lines[^1]);
            curLine.WeightIF.gameObject.SetActive(false);
        }

        private void CreateLineBetweenExistingPoints(Vector3 worldPlacementPos, Point point)
        {
            curLine.SetEndPoint(point);
            GameObject newLine = Instantiate(LinePrefab, worldPlacementPos, Quaternion.identity, LinesCanvas.transform);
            MainGraph.AddLine(newLine, point, ref curLine);
            AddListener(MainGraph.Lines[^1]);
            curLine.WeightIF.gameObject.SetActive(false);
        }

        private void CreateLineWithNewPoint(Vector3 worldPlacementPos)
        {
            GameObject newPointGO = Instantiate(PointPrefab, worldPlacementPos, Quaternion.identity, PointsCanvas.transform);
            MainGraph.AddPoint(newPointGO);
            curLine?.SetEndPoint(MainGraph.Points[^1]);
            GameObject newLine = Instantiate(LinePrefab, worldPlacementPos, Quaternion.identity, LinesCanvas.transform);
            MainGraph.AddLine(newLine, MainGraph.Points[^1], ref curLine);
            AddListener(MainGraph.Lines[^1]);
            curLine.WeightIF.gameObject.SetActive(false);
        }

        private void SetEndPosOfCurLine(Vector3 worldPlacementPos)
        {
            areaController.SetCurAreaColor(curLine);
            Point curPoint = new(worldPlacementPos);
            curLine.SetEndPoint(curPoint);
        }

        private void AddListener(Line line)
        {
            if (MainGraph.Type.Equals("Взвешенный граф"))
                line.WeightIF.onEndEdit.AddListener(delegate { OnWeightedLineWeightValueChanged(line); });
            else if (MainGraph.Type.Equals("Транспортная сеть"))
                line.WeightIF.onEndEdit.AddListener(delegate { OnTransportWeightValueChanged(line); });
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