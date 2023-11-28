using Assets.Scripts.GraphComponents;
using Assets.Scripts.TransportNetwork;
using System.Collections.Generic;

namespace Assets.Scripts.ShortestPath
{
    /// <summary>
    /// Класс для получения шагов поиска кратчайшего пути.
    /// </summary>
    public class ShortestPathStepsGetting
    {
        /// <summary>
        /// Метод для получения шагов поиска кратчайшего пути.
        /// </summary>
        /// <param name="graph">Копия графа для поиска кратчайшего пути.</param>
        /// <param name="fromPointIndex">Индекс начальной точки маршрута.</param>
        /// <param name="toPointIndex">Индекс конечной точки маршрута.</param>
        public static List<TransportStep> GetShortestPathSteps(Graph graph, int fromPointIndex, int toPointIndex)
        {
            Point fromPoint = graph.Points[fromPointIndex];
            Point toPoint = graph.Points[toPointIndex];
            /* Ваш кодик должен быть туть. Используйте алгоритм Дейкстры для вычисления кратчайшего пути.
            * Поля, которые могут понадобиться:
            * point.LinkedLines 
            * line.Weight (В данном случае это вес линии)
            * Метод для проверки точки на то, является она начальной или конечной в указанной линии: point.IsStartPoint(Line line); */
            return new List<TransportStep>();
        }
    }
}
