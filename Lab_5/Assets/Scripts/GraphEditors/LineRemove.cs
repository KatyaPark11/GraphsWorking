using UnityEngine;
using UnityEngine.UI;
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
                        {
                            if (line.IsSelected)
                            {
                                line.LightOff();
                            }
                            else
                            {
                                line.LightOn();
                            }
                            return;
                        }
                        else
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
                            return;
                        }
                    }
                    else
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
                        return;
                    }
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                gameObject.SetActive(false);
            }
        }

        private void RemoveLine(Line line)
        {
            if (line.StartPoint.LinkedLines.Count == 1)
            {
                GameObject startPoint = line.StartPoint.PointObj;
                MainGraph.RemovePoint(startPoint);
                MainGraph.UpdatePointsNames(startPoint);
                Destroy(startPoint);
            }
            if (line.EndPoint.LinkedLines.Count == 1)
            {
                GameObject endPoint = line.EndPoint.PointObj;
                MainGraph.RemovePoint(endPoint);
                MainGraph.UpdatePointsNames(endPoint);
                Destroy(endPoint);
            }
            MainGraph.RemoveLine(line, true);
            MainGraph.UpdateLinesNames();
            Destroy(line.LineObj);
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