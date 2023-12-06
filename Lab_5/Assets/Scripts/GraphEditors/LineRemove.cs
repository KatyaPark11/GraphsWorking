using UnityEngine;
using static Assets.Scripts.VarsHolder;

namespace Assets.Scripts.GraphComponents
{
    public class LineRemove : BaseEditor
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                for (int i = 0; i < MainGraph.Lines.Count; i++)
                {
                    if (MainGraph.Lines[i].IsSelected)
                    {
                        RemoveLine(MainGraph.Lines[i]);
                        i--;
                    }
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray);
                for (int i = 0; i < hits.Length; i++)
                {
                    if (!Physics.Raycast(ray, out hits[i])) continue;
                    if (!hits[i].collider.CompareTag("Line")) continue;
                    Line line = MainGraph.GetLine(hits[i].collider.gameObject);
                    if (!MainGraph.Type.Equals("Обычный граф"))
                    {
                        if (!MainGraph.TryGetLine(line.EndPoint, line.StartPoint, out Line reverseLine))
                            ComplexLight(line);
                        else
                            ComplexLight(line, reverseLine);
                    }
                    else
                    {
                        SimpleLight(line);
                    }
                    return;
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                gameObject.SetActive(false);
            }
        }

        private void ComplexLight(Line line)
        {
            if (line.IsSelected) line.LightOff();
            else line.LightOn();
        }

        private void ComplexLight(Line line, Line reverseLine)
        {
            if (!line.IsSelected)
            {
                line.LightOn();
            }
            else if (line.IsSelected && !reverseLine.IsSelected)
            {
                line.LightOff();
                reverseLine.LightOn();
            }
            else
            {
                line.LightOff();
                reverseLine.LightOff();
            }
        }

        private void SimpleLight(Line line)
        {
            if (line.IsSelected)
            {
                if (MainGraph.TryGetLine(line.EndPoint, line.StartPoint, out Line reverseLine))
                    reverseLine.LightOff();
                line.LightOff();
            }
            else
            {
                if (MainGraph.TryGetLine(line.EndPoint, line.StartPoint, out Line reverseLine))
                    reverseLine.LightOn();
                line.LightOn();
            }
        }

        private void RemoveLine(Line line)
        {
            RemoveEmptyPoints(line);
            MainGraph.RemoveLine(line, true);
            MainGraph.UpdateLinesNames();
            Destroy(line.LineObj);
        }

        private void RemoveEmptyPoints(Line line)
        {
            if (line.StartPoint.LinkedLines.Count == 1)
                RemoveEmptyPoint(line.StartPoint);
            if (line.EndPoint.LinkedLines.Count == 1)
                RemoveEmptyPoint(line.EndPoint);
        }

        private void RemoveEmptyPoint(Point point)
        {
            GameObject pointGO = point.PointObj;
            MainGraph.RemovePoint(pointGO);
            MainGraph.UpdatePointsNames(pointGO);
            Destroy(pointGO);
        }

        protected override void OnDisable()
        {
            foreach (Line line in MainGraph.Lines)
            {
                if (line.IsSelected)
                {
                    line.IsSelected = false;
                    line.LineRenderer.sortingLayerName = "Default";
                    line.ArrowRenderer.sortingLayerName = "Default";
                    line.SetLineColor(Color.red);
                }
            }
            base.OnDisable();
        }
    }
}