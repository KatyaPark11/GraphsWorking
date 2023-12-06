using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.GraphComponents
{
    /// <summary>
    /// Класс для реализации линии (ребра) графа.
    /// </summary>
    [Serializable]
    public class Line
    {
        /// <summary>
        /// Игровой объект линии.
        /// </summary>
        [NonSerialized]
        public GameObject LineObj;
        /// <summary>
        /// Линия, соединяющая две точки линии.
        /// </summary>
        [NonSerialized]
        public LineRenderer LineRenderer;
        /// <summary>
        /// Стрелка, указывающая направление линии.
        /// </summary>
        [NonSerialized]
        public LineRenderer ArrowRenderer;
        /// <summary>
        /// Поле ввода для указания веса/загруженности (в зависимости от типа графа) линии.
        /// </summary>
        [NonSerialized]
        public TMP_InputField WeightIF;
        /// <summary>
        /// Название линии.
        /// </summary>
        public string Name;
        /// <summary>
        /// Начальная точка линии.
        /// </summary>
        public Point StartPoint;
        /// <summary>
        /// Конечная точка линии.
        /// </summary>
        public Point EndPoint;
        /// <summary>
        /// Вес/загруженность (в зависимости от типа графа) линии.
        /// </summary>
        public string Weight;
        /// <summary>
        /// Выбрана ли линия.
        /// </summary>
        [NonSerialized]
        public bool IsSelected;

        private const float arrowSize = 15f;
        private const float arrowAngle = 30f;

        /// <summary>
        /// Конструктор класса без использования игрового объекта.
        /// </summary>
        /// <param name="startPoint">Начальная точка линии.</param>
        /// <param name="endPoint">Конечная точка линии.</param>
        /// <param name="weight">Вес линии.</param>
        public Line(Point startPoint, Point endPoint, string weight = "0")
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
        public Line(GameObject lineObj, Point startPoint, Point endPoint, string type = "Обычный граф")
        {
            string weight = "0";
            ObjsInitialization(lineObj, startPoint, endPoint);

            WeightIF = lineObj.GetComponentInChildren<TMP_InputField>();
            if (type.Equals("Обычный граф"))
            {
                ArrowRenderer.gameObject.SetActive(false);
                WeightIF.gameObject.SetActive(false);
            }
            else if (type.Equals("Транспортная сеть"))
            {
                weight = "0/0";
            }
            SetWeight(weight);
        }

        /// <summary>
        /// Конструктор класса с использованием игрового объекта с уже заданным весом.
        /// </summary>
        /// <param name="lineObj">Игровой объект линии.</param>
        /// <param name="startPoint">Начальная точка линии.</param>
        /// <param name="endPoint">Конечная точка линии.</param>
        /// <param name="weight">Вес линии.</param>
        public Line(GameObject lineObj, Point startPoint, Point endPoint, string type = "Обычный граф", string weight = "0")
        {
            ObjsInitialization(lineObj, startPoint, endPoint);

            WeightIF = lineObj.GetComponentInChildren<TMP_InputField>();
            if (type.Equals("Обычный граф"))
            {
                ArrowRenderer.gameObject.SetActive(false);
                WeightIF.gameObject.SetActive(false);
            }
            SetWeight(weight);
        }

        private void ObjsInitialization(GameObject lineObj, Point startPoint, Point endPoint)
        {
            LineObj = lineObj;
            LineRenderer[] lineRenderers = lineObj.GetComponentsInChildren<LineRenderer>();
            LineRenderer = lineRenderers[0];
            ArrowRenderer = lineRenderers[1];
            Name = lineObj.name;
            StartPoint = startPoint;
            EndPoint = endPoint;
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
            LineObj.transform.position = StartPoint.Position;
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

        public void SetArrowRendererPos()
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
            if (CompareColor(LineRenderer.startColor, color)) return;
            LineRenderer.startColor = color;
            LineRenderer.endColor = color;
            ArrowRenderer.startColor = color;
            ArrowRenderer.endColor = color;
        }

        private void SetWeight(string weight)
        {
            Weight = weight;
            WeightIF.text = weight;
            SetWeightIFPosition();
        }

        private void SetWeightIFPosition()
        {
            float coorX = (float)(StartPoint.Position.x + EndPoint.Position.x) / 2 - 10;
            float coorY = (float)(StartPoint.Position.y + EndPoint.Position.y) / 2 - 10;
            WeightIF.transform.position = new Vector3(coorX, coorY, 0);
        }

        public void LightOn()
        {
            IsSelected = true;
            LineRenderer.sortingLayerName = "LineTop";
            ArrowRenderer.sortingLayerName = "LineTop";
            SetLineColor(Color.black);
        }

        public void LightOff()
        {
            IsSelected = false;
            LineRenderer.sortingLayerName = "Default";
            ArrowRenderer.sortingLayerName = "Default";
            SetLineColor(Color.red);
        }

        public void UpdateInputFieldPosition()
        {
            Vector3 position1 = LineRenderer.GetPosition(0);
            Vector3 position2 = LineRenderer.GetPosition(1);

            Vector3 middlePosition = Vector3.Lerp(position1, position2, 0.75f);
            WeightIF.transform.position = middlePosition;
        }

        public static bool CompareColor(Color a, Color b)
        {
            const float accdelta = 0.01f;
            bool result = false;
            if (Math.Abs(a.r - b.r) < accdelta)
                if (Math.Abs(a.g - b.g) < accdelta)
                    if (Math.Abs(a.b - b.b) < accdelta) result = true;

            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Line otherLine = (Line)obj;

            return ReferenceEquals(this.StartPoint, otherLine.StartPoint) &&
                   ReferenceEquals(this.EndPoint, otherLine.EndPoint);
        }

        public override int GetHashCode() => HashCode.Combine(17, StartPoint?.GetHashCode() ?? 0, EndPoint?.GetHashCode() ?? 0);
    }
}
