using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.GraphComponents
{
    /// <summary>
    /// Класс для реализации графа.
    /// </summary>
    [Serializable]
    public class Graph
    {
        /// <summary>
        /// Список линий, из которых состоит граф.
        /// </summary>
        public List<Line> Lines;
        /// <summary>
        /// Список точек, из которых состоит граф.
        /// </summary>
        public List<Point> Points;
        /// <summary>
        /// Тип графа.
        /// </summary>
        public string Type = "Обычный граф";
        /// <summary>
        /// Максимально допустимое число точек.
        /// </summary>
        [NonSerialized]
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

        /// <summary>
        /// Конструктор класса для загрузки графа из указанного json файла.
        /// </summary>
        /// <param name="filePath">Полный путь к файлу.</param>
        /// <exception cref="FileNotFoundException">Ошибка, возникающая при отсутствии файла загрузки.</exception>
        public Graph(string filePath)
        {
            if (!File.Exists(filePath)) 
                throw new FileNotFoundException("Файл для загрузки не найден.", filePath);
            JsonUtility.FromJsonOverwrite(File.ReadAllText(filePath), this);
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
            newLineGO.name = $"Line{Lines.Count}";
            line = new Line(newLineGO, point, point, Type);
            line.SetLineRendererPos();
            Lines.Add(line);
        }

        public void AddLine(GameObject newLineGO, Point startPoint, Point endPoint, string type, string weight)
        {
            Line line = new(newLineGO, startPoint, endPoint, type, weight);
            line.SetLineRendererPos();
            line.SetArrowRendererPos();
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

        public void RemoveLine(Line line, bool isDeleteLinks = false)
        {
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
            int pointIndex = GetPointIndex(go);
            return Points[pointIndex];
        }

        public Point GetPoint(char pointName)
        {
            int pointIndex = GetPointIndex(pointName);
            return Points[pointIndex];
        }

        public int GetPointIndex(GameObject go)
        {
            return char.Parse(go.name) - 'A';
        }

        public int GetPointIndex(char pointName)
        {
            return pointName - 'A';
        }

        public Line GetLine(GameObject go)
        {
            int lineIndex = GetLineIndex(go);
            return Lines[lineIndex];
        }

        public bool TryGetLine(Point startPoint, Point endPoint, out Line foundLine)
        {
            foreach (Line line in Lines)
            {
                if (line.StartPoint == startPoint && line.EndPoint == endPoint)
                {
                    foundLine = line;
                    return true;
                }
            }
            foundLine = null;
            return false;
        }

        public int GetLineIndex(GameObject go)
        {
            return int.Parse(go.name[4..]);
        }

        public void UpdatePointsNames(int startIndex)
        {
            for (int i = startIndex; i < Points.Count; i++)
            {
                Point point = Points[i];
                char newName = (char)(i + 'A');
                point.Name = newName;
                point.NameTMP.text = newName.ToString();
                point.PointObj.name = newName.ToString();
            }
        }

        public void UpdatePointsNames(GameObject startPoint)
        {
            int startIndex = GetPointIndex(startPoint);
            UpdatePointsNames(startIndex);
        }

        public void UpdateLinesNames(int startIndex)
        {
            for (int i = startIndex; i < Lines.Count; i++)
            {
                Line line = Lines[i];
                string newName = $"Line{i}";
                line.Name = newName;
                line.LineObj.name = newName;
            }
        }

        public void UpdateLinesNames(GameObject startLine)
        {
            int startIndex = GetLineIndex(startLine);
            UpdateLinesNames(startIndex);
        }

        public void UpdateLinesNames()
        {
            UpdateLinesNames(0);
        }

        public int[,] GetAdjacencyMatrix()
        {
            int matrixSize = Points.Count;
            int[,] adjacencyMatrix = new int[matrixSize, matrixSize];

            Dictionary<char, int> pointIndexMap = new();
            int index = 0;
            foreach (Point point in Points)
            {
                pointIndexMap.Add(point.Name, index);
                index++;
            }

            foreach (Line line in Lines)
            {
                int startIndex = pointIndexMap[line.StartPoint.Name];
                int endIndex = pointIndexMap[line.EndPoint.Name];
                int weight;
                if (Type.Equals("Транспортная сеть"))
                    weight = int.Parse(line.Weight[(line.Weight.IndexOf('/') + 1)..]);
                else
                    weight = int.Parse(line.Weight);
                adjacencyMatrix[startIndex, endIndex] = weight;
            }
            return adjacencyMatrix;
        }

        public void SetLinkedLines()
        {
            foreach (Line line in Lines)
            {
                line.StartPoint.AddLinkedLine(line);
                line.EndPoint.AddLinkedLine(line);
            }
        }

        /// <summary>
        /// Метод, сохраняющий экземпляр графа в json файле в указанной папке с указанным именем.
        /// </summary>
        /// <param name="saveDir">Путь к папке сохранения.</param>
        /// <param name="ghaphName">Название файла для сохранения.</param>
        public void Save(string saveDir, string graphName = "test")
        {
            string json = JsonUtility.ToJson(this, true);
            if (!Directory.Exists(saveDir)) 
                Directory.CreateDirectory(saveDir);
            File.WriteAllText($"{saveDir}\\{graphName}.json", json);
        }
    }
}
