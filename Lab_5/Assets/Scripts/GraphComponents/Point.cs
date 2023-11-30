using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.GraphComponents
{
    /// <summary>
    /// Класс для реализации точки (вершины) графа.
    /// </summary>
    public class Point
    {
        /// <summary>
        /// Игровой объект точки.
        /// </summary>
        public GameObject PointObj { get; private set; }
        /// <summary>
        /// Текст на игровом объекте, указывающий на название точки.
        /// </summary>
        public TextMeshProUGUI NameTMP { get; private set; }
        /// <summary>
        /// Позиция точки.
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// Название точки.
        /// </summary>
        public char Name { get; private set; }
        /// <summary>
        /// Связанные с точкой линии.
        /// </summary>
        public List<Line> LinkedLines { get; private set; }

        /// <summary>
        /// Конструктор класса без использования игрового объекта.
        /// </summary>
        /// <param name="position">Позиция точки.</param>
        public Point(Vector3 position) => Position = position;

        /// <summary>
        /// Конструктор класса с использованием игрового объекта.
        /// </summary>
        /// <param name="pointObj">Игровой объект точки.</param>
        public Point(GameObject pointObj) 
        {
            PointObj = pointObj;
            NameTMP = pointObj.GetComponentInChildren<TextMeshProUGUI>();
            Position = pointObj.transform.position;
            Name = char.Parse(pointObj.name);
            LinkedLines = new List<Line>();
        }

        public void AddLinkedLine(Line line)
        {
            if (LinkedLines.Contains(line)) return;
            LinkedLines.Add(line);
        }

        public void ClearLinkedLines()
        {
            foreach (Line line in LinkedLines)
            {
                RemoveLinkedLine(line);
            }
        }

        public void RemoveLinkedLine(Line line) => LinkedLines.Remove(line);

        public bool IsStartPoint(Line line)
        {
            if (!LinkedLines.Contains(line)) return false;
            return line.StartPoint == this;
        }
    }
}
