using Assets.Scripts.Controllers;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.GraphComponents
{
    /// <summary>
    /// Класс для реализации линии (ребра) графа.
    /// </summary>
    public class Line
    {
        /// <summary>
        /// Игровой объект линии.
        /// </summary>
        public GameObject LineObj { get; private set; }
        /// <summary>
        /// Линия, соединяющая две точки линии.
        /// </summary>
        public LineRenderer LineRenderer { get; private set; }
        /// <summary>
        /// Стрелка, указывающая направление линии.
        /// </summary>
        public LineRenderer ArrowRenderer { get; private set; }
        /// <summary>
        /// Поле ввода для указания веса/загруженности (в зависимости от типа графа) линии.
        /// </summary>
        public TMP_InputField WeightIF { get; private set; }
        /// <summary>
        /// Начальная точка линии.
        /// </summary>
        public Point StartPoint { get; set; }
        /// <summary>
        /// Конечная точка линии.
        /// </summary>
        public Point EndPoint { get; set; }
        /// <summary>
        /// Вес/загруженность (в зависимости от типа графа) линии.
        /// </summary>
        public int Weight { get; set; }

        private const float arrowSize = 20f;
        private const float arrowAngle = 30f;

        /// <summary>
        /// Конструктор класса без использования игрового объекта.
        /// </summary>
        /// <param name="startPoint">Начальная точка линии.</param>
        /// <param name="endPoint">Конечная точка линии.</param>
        /// <param name="weight">Вес линии.</param>
        public Line(Point startPoint, Point endPoint, int weight = 0)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            Weight = weight;
        }

        /// <summary>
        /// Конструктор класса с использованием игрового объекта.
        /// </summary>
        /// <param name="lineObj">Игровой объект линии.</param>
        /// <param name="startPoint">Начальная точка линии.</param>
        /// <param name="endPoint">Конечная точка линии.</param>
        /// <param name="weight">Вес линии.</param>
        public Line(GameObject lineObj, Point startPoint, Point endPoint, int weight = 0)
        {
            LineObj = lineObj;
            LineRenderer[] lineRenderers = lineObj.GetComponentsInChildren<LineRenderer>();
            LineRenderer = lineRenderers[0];
            LineRenderer.positionCount = 2;
            ArrowRenderer = lineRenderers[1];
            ArrowRenderer.positionCount = 3;
            WeightIF = lineObj.GetComponentInChildren<TMP_InputField>();
            StartPoint = startPoint;
            EndPoint = endPoint;
            Weight = weight;
            SetWeightIFPosition();
        }

        public void GoToTheAnotherPoint(Point point)
        {
            StartPoint = point;
            EndPoint = point;
            SetLineRendererPos();
        }

        public void SetStartPoint(Point point)
        {
            StartPoint = point;
            LineRenderer.SetPosition(0, StartPoint.Position);
            SetArrowRendererPos();
        }

        public void SetEndPoint(Point point)
        {
            EndPoint = point;
            LineRenderer.SetPosition(1, EndPoint.Position);
            SetArrowRendererPos();
        }

        public void SetLineRendererPos()
        {
            LineRenderer.SetPosition(0, StartPoint.Position);
            LineRenderer.SetPosition(1, EndPoint.Position);
        }

        private void SetArrowRendererPos()
        {
            Vector3[] linePositions = new Vector3[LineRenderer.positionCount];
            LineRenderer.GetPositions(linePositions);

            if (linePositions.Length >= 2)
            {
                Vector3 startPosition = linePositions[0];
                Vector3 endPosition = linePositions[^1];

                Vector3 arrowDirection = (endPosition - startPosition).normalized;
                Vector3 arrowEdge1 = Quaternion.Euler(0, 0, arrowAngle) * -arrowDirection * arrowSize;
                Vector3 arrowEdge2 = Quaternion.Euler(0, 0, -arrowAngle) * -arrowDirection * arrowSize;

                Vector3[] arrowPositions = new Vector3[3];
                arrowPositions[0] = endPosition + arrowEdge1;
                arrowPositions[1] = endPosition;
                arrowPositions[2] = endPosition + arrowEdge2;

                ArrowRenderer.SetPositions(arrowPositions);
            }
        }

        public void SetLineColor(Color color)
        {
            if (AreaController.CompareColor(LineRenderer.startColor, color)) return;
            LineRenderer.startColor = color;
            LineRenderer.endColor = color;
            ArrowRenderer.startColor = color;
            ArrowRenderer.endColor = color;
        }

        private void SetWeightIFPosition()
        {
            float coorX = (float)(StartPoint.Position.x + EndPoint.Position.x) / 2 - 10;
            float coorY = (float)(StartPoint.Position.y + EndPoint.Position.y) / 2 - 10;
            WeightIF.transform.position = new Vector3(coorX, coorY, 0);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Line otherLine = (Line)obj;

            return ReferenceEquals(this.StartPoint, otherLine.StartPoint) &&
                   ReferenceEquals(this.EndPoint, otherLine.EndPoint);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (StartPoint != null ? StartPoint.GetHashCode() : 0);
                hash = hash * 23 + (EndPoint != null ? EndPoint.GetHashCode() : 0);
                return hash;
            }
        }
    }
}
