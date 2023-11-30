using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.GraphComponents
{
    /// <summary>
    /// Класс для реализации графа.
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// Список линий, из которых состоит граф.
        /// </summary>
        public List<Line> Lines { get; set; }
        /// <summary>
        /// Список точек, из которых состоит граф.
        /// </summary>
        public List<Point> Points { get; set; }
        /// <summary>
        /// Тип графа.
        /// </summary>
        public string Type { get; set; } = "Обычный граф";
        /// <summary>
        /// Максимально допустимое число точек.
        /// </summary>
        public const int MaxPointsCount = 26;

        /// <summary>
        /// Конструктор класса для создания нового графа.
        /// </summary>
        public Graph() 
        {
            Lines = new List<Line>();
            Points = new List<Point>();
        }

        /// <summary>
        /// Конструктор класса для создания копии графа (ссылки на объекты сохраняются).
        /// </summary>
        /// <param name="lines">Список линий, из которых состоит граф.</param>
        /// <param name="points">Список точек, из которых состоит граф.</param>
        public Graph(List<Line> lines, List<Point> points)
        {
            Lines = lines;
            Points = points;
        }

        public Graph(string filePath)
        {
            string[] file = File.ReadAllLines(filePath);
        }

        public void AddPoint(GameObject newPointGO)
        {
            newPointGO.name = ((char)(Points.Count + 'A')).ToString();
            Point newPoint = new(newPointGO);
            newPoint.NameTMP.text = newPoint.Name.ToString();
            Points.Add(newPoint);
        }

        public void AddLine(GameObject newLineGO, Point point, ref Line line)
        {
            line = new Line(newLineGO, point, point);
            line.SetLineRendererPos();
            Lines.Add(line);
        }

        public void RemovePoint(GameObject go)
        {
            Point point = GetPoint(go);
            point.ClearLinkedLines();
            Points.Remove(point);
        }

        public void RemoveLine(int lineIndex, bool isDeleteLinks = false)
        {
            Line line = Lines[lineIndex];
            if (isDeleteLinks)
            {
                line.StartPoint.LinkedLines.Remove(line);
                line.EndPoint.LinkedLines.Remove(line);
                if (line.StartPoint.LinkedLines.Count == 0) 
                    Points.Remove(line.StartPoint);
                if (line.EndPoint.LinkedLines.Count == 0)
                    Points.Remove(line.EndPoint);
            }
            Lines.Remove(line);
        }

        public Point GetPoint(GameObject go)
        {
            int pointIndex = char.Parse(go.name) - 'A';
            return Points[pointIndex];
        }

        public void SetLinkedLines()
        {
            foreach (Line line in Lines)
            {
                line.StartPoint.AddLinkedLine(line);
                line.EndPoint.AddLinkedLine(line);
            }
        }

        public void Save()
        {

        }
    }
}
