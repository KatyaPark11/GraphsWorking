using Assets.Scripts.Controllers;
using Assets.Scripts.GraphComponents;
using TMPro;
using UnityEngine;
using static Assets.Scripts.VarsHolder;

namespace Assets.Scripts.GraphEditors
{
    public class Downloading : MonoBehaviour
    {
        public GameObject PointPrefab;
        public GameObject LinePrefab;
        public Canvas PointsCanvas;
        public Canvas LinesCanvas;
        public TMP_Dropdown DownloadDropdown;
        public TMP_Dropdown GraphType;

        public void Download()
        {
            if (MainGraph.Points.Count > 0)
                ClearGraphObjs();
            MainGraph = new Graph($"../{DownloadDropdown.options[DownloadDropdown.value].text}.json");
            CreateGraphObjs();
        }

        private void CreateGraphObjs()
        {
            Graph graph = new()
            {
                Type = MainGraph.Type
            };
            int graphTypeIndex = GraphType.options.FindIndex(option => option.text.Equals(graph.Type));
            GraphType.SetValueWithoutNotify(graphTypeIndex);

            CreatePoints(graph);
            CreateLines(graph);
            graph.SetLinkedLines();
            MainGraph = graph;
            foreach (Line line in MainGraph.Lines)
                line.UpdateInputFieldPosition();
            MeshColliderController.UpdateMeshColliders();
            TypeController.ChangeType(graphTypeIndex);
        }

        private void CreatePoints(Graph graph)
        {
            foreach (Point point in MainGraph.Points)
            {
                GameObject pointGO = Instantiate(PointPrefab, point.Position, Quaternion.identity, PointsCanvas.transform);
                pointGO.name = point.Name.ToString();
                graph.AddPoint(pointGO);
            }
        }

        private void CreateLines(Graph graph)
        {
            foreach (Line line in MainGraph.Lines)
            {
                GameObject lineGO = Instantiate(LinePrefab, line.StartPoint.Position, Quaternion.identity, LinesCanvas.transform);
                lineGO.name = line.Name;
                graph.AddLine(lineGO, graph.GetPoint(line.StartPoint.Name), graph.GetPoint(line.EndPoint.Name), MainGraph.Type, line.Weight);
            }
        }

        private void ClearGraphObjs()
        {
            foreach (Point point in MainGraph.Points)
                Destroy(point.PointObj);
            foreach (Line line in MainGraph.Lines)
                Destroy(line.LineObj);
        }
    }
}
