using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.GraphComponents
{
    /// <summary>
    /// Класс для реализации точки (вершины) графа.
    /// </summary>
    [Serializable]
    public class Point
    {
        /// <summary>
        /// Игровой объект точки.
        /// </summary>
        [NonSerialized]
        public readonly GameObject PointObj;
        /// <summary>
        /// Текст на игровом объекте, указывающий на название точки.
        /// </summary>
        [NonSerialized]
        public readonly TextMeshProUGUI NameTMP;
        /// <summary>
        /// Позиция точки.
        /// </summary>
        public Vector3 Position;
        /// <summary>
        /// Название точки.
        /// </summary>
        public char Name;
        /// <summary>
        /// Связанные с точкой линии.
        /// </summary>
        [NonSerialized]
        public List<Line> LinkedLines;

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

        public void SetLinkedLinesPos()
        {
            foreach (Line line in LinkedLines)
            {
                if (IsStartPoint(line))
                    line.SetStartPoint(this);
                else
                    line.SetEndPoint(this);
            }
        }

        public void ClearLinkedLines()
        {
            for (int i = 0; i < LinkedLines.Count; i++)
                RemoveLinkedLine(LinkedLines[i]);
        }

        public void RemoveLinkedLine(Line line) => LinkedLines.Remove(line);

        public bool IsStartPoint(Line line)
        {
            if (!LinkedLines.Contains(line)) return false;
            return line.StartPoint == this;
        }
    }
}
