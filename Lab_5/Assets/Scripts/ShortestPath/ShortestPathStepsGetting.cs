using Assets.Scripts.GraphComponents;
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
        public static List<ShortestPathStep> GetShortestPathSteps(Graph graph, int fromPointIndex, int toPointIndex)
        {
            Point[] parents = new Point[graph.Points.Count];
            int[] distances = new int[graph.Points.Count];
            for (int i = 0; i < distances.Length; i++)
                distances[i] = int.MaxValue;
            Point fromPoint = graph.Points[fromPointIndex];
            distances[fromPointIndex] = 0;
            Point toPoint = graph.Points[toPointIndex];
            List<ShortestPathStep> steps = new();

            List<Line> visited = new();

            Queue<Point> queue = new();
            queue.Enqueue(fromPoint);

            int minDist = int.MaxValue;

            List<Line> lightedOnLines = new();
            List<Line> lightedOffLines = new();
            Dictionary<Point, int> d = new();

            while (queue.Count != 0)
            {
                Point currentPoint = queue.Dequeue();

                if (currentPoint == toPoint)
                {
                    minDist = distances[graph.GetPointIndex(currentPoint.Name)];
                }

                if (currentPoint != toPoint)
                {
                    foreach (Line line in currentPoint.LinkedLines)
                    {
                        if (line.EndPoint != currentPoint && !visited.Contains(line))
                        {
                            if (distances[graph.GetPointIndex(currentPoint.Name)] + int.Parse(line.Weight) < distances[graph.GetPointIndex(line.EndPoint.Name)])
                            {
                                distances[graph.GetPointIndex(line.EndPoint.Name)] = distances[graph.GetPointIndex(currentPoint.Name)] + int.Parse(line.Weight);
                                parents[graph.GetPointIndex(line.EndPoint.Name)] = currentPoint;
                            }

                            if (d.ContainsKey(line.EndPoint))
                            {
                                d.Remove(line.EndPoint);
                                d.Add(line.EndPoint, distances[graph.GetPointIndex(line.EndPoint.Name)]);
                            }
                            else
                            {
                                d.Add(line.EndPoint, distances[graph.GetPointIndex(line.EndPoint.Name)]);
                            }
                            Dictionary<Point, int> copyD = new();
                            foreach (var e in d)
                            {
                                copyD.Add(e.Key, e.Value);
                            }

                            if (!visited.Contains(line))
                            {
                                lightedOnLines.Add(line);
                            }

                            List<Line> copyLines = new();
                            foreach (var e in lightedOnLines)
                            {
                                copyLines.Add(e);
                            }

                            steps.Add(new ShortestPathStep(copyLines, lightedOffLines, copyD,
                            $"Из узла {currentPoint.Name} переходим в узел {line.EndPoint.Name}. " +
                            $"Минимальное расстояние до узла {line.EndPoint.Name} равно {distances[graph.GetPointIndex(line.EndPoint.Name)]}",
                            $"Из узла {currentPoint.Name} переходим в узел {line.EndPoint.Name}. " +
                            $"Минимальное расстояние до узла {line.EndPoint.Name} равно {distances[graph.GetPointIndex(line.EndPoint.Name)]}"));

                            visited.Add(line);
                            queue.Enqueue(line.EndPoint);
                        }
                    }
                }
            }

            if (minDist != int.MaxValue)
            {
                RecoverPath(graph, steps, toPoint, fromPoint, d, parents);
                return steps;
            }

            steps.Add(new ShortestPathStep(lightedOnLines, lightedOffLines, d, "нет пути", "нет пути"));

            return steps;
        }

        private static void RecoverPath(Graph graph, List<ShortestPathStep> steps, Point end, Point begin, Dictionary<Point, int> d, Point[] parents)
        {
            List<Line> lightedOnLines = new();
            List<Line> lightedOffLines = new();

            foreach (Line line in graph.Lines)
                lightedOffLines.Add(line);

            Point currentPoint = end;
            while (parents[graph.GetPointIndex(currentPoint.Name)] != null)
            {
                foreach (Line line in currentPoint.LinkedLines)
                {
                    if (parents[graph.GetPointIndex(currentPoint.Name)].IsStartPoint(line))
                    {
                        lightedOnLines.Add(line);
                        lightedOffLines.Remove(line);

                        List<Line> copyLine = new();
                        foreach (var e in lightedOffLines)
                        {
                            copyLine.Add(e);
                        }

                        break;
                    }
                }

                currentPoint = parents[graph.GetPointIndex(currentPoint.Name)];
            }
            steps.Add(new ShortestPathStep(lightedOnLines, lightedOffLines, d,
                $"Показан кратчайший путь между вершинами {begin.Name} и {end.Name}",
                $"Показан кратчайший путь между вершинами {begin.Name} и {end.Name}"));
        }
    }
}
