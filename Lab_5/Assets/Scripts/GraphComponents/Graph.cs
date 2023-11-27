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
        public bool IsDirectionVisible { get; private set; }
        public bool IsWeightVisible { get; private set; }
        /// <summary>
        /// Максимально допустимое число точек.
        /// </summary>
        public const int MaxPointsCount = 26;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public Graph() 
        { 
            Lines = new List<Line>();
            Points = new List<Point>();
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
