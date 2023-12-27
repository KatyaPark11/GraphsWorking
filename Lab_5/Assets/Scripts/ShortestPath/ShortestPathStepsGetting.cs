using Assets.Scripts.GraphComponents;
using Assets.Scripts.SpanningTree;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.ShortestPath
{
    /// <summary>
    /// Класс для получения шагов поиска кратчайшего пути.
    /// </summary>
    public class ShortestPathStepsGetting
    {
        public static List<ShortestPathStep> shortestPathStep = new();
        /// <summary>
        /// Метод для получения шагов поиска кратчайшего пути.
        /// </summary>
        /// <param name="graph">Копия графа для поиска кратчайшего пути.</param>
        /// <param name="fromPointIndex">Индекс начальной точки маршрута.</param>
        /// <param name="toPointIndex">Индекс конечной точки маршрута.</param>
        public static List<ShortestPathStep> GetShortestPathSteps(Graph graph, int fromPointIndex, int toPointIndex)
        {
            int[,] graph1 = new int[graph.Points.Count, graph.Points.Count];
            for (int i = 0; i < graph.Lines.Count; i++)
            {
                int point1 = 0;
                int point2 = 0;

                for (int k = 0; k < graph.Points.Count; k++)
                {
                    if (graph.Lines[i].StartPoint == graph.Points[k])
                    {
                        point1 = k;
                    }
                    if (graph.Lines[i].EndPoint == graph.Points[k])
                    {
                        point2 = k;
                    }
                }
                graph1[point1, point2] = int.Parse(graph.Lines[i].Weight);
                graph1[point2, point1] = int.Parse(graph.Lines[i].Weight);
            }
            DijkstraAlgorithm dijkstraAlgorithm = new DijkstraAlgorithm(graph1, graph.Points.Count);
            shortestPathStep = dijkstraAlgorithm.Run(fromPointIndex, toPointIndex, graph);
            return shortestPathStep;
        }
    }

    public class DijkstraAlgorithm
    {
        public static List<ShortestPathStep> shortestPathStep;
        private int[,] graph;
        private int verticesCount;
        int k = 0;

        public DijkstraAlgorithm(int[,] graph, int verticesCount)
        {
            shortestPathStep = new();
            this.graph = graph;
            this.verticesCount = verticesCount;
        }

        private int FindMinDistance(int[] distances, bool[] visited, Graph graph1, int[] array)
        {
            Dictionary<Point, int> d = new();

            int min = int.MaxValue;
            int minIndex = -1;

            for (int v = 0; v < verticesCount; v++)
            {
                if (!visited[v] && distances[v] <= min)
                {
                    min = distances[v];
                    minIndex = v;
                }
            }

            string str = "";
            for (int v = 0; v < verticesCount; v++)
            {
                if (visited[v])
                {
                    str = str + graph1.Points[v].Name + " ";
                }
            }

            d.Add(graph1.Points[minIndex], min);
            string descNext = $"Ищем вершину с минимальным расстоянием, " +
                $"которая еще не была посещена. Эта вершина - {graph1.Points[minIndex].Name} с минимальным расстоянием до неё {min}.\n";
            if (str != "")
            {
                descNext += $"Посещенные вершины - {str}.";
            }
            List<Line> lines = new();

            for (int i = 0; i < graph1.Lines.Count; i++)
            {
                if ((graph1.Lines[i].StartPoint.Name == graph1.Points[array[1]].Name && graph1.Lines[i].EndPoint.Name == graph1.Points[minIndex].Name) || 
                     graph1.Lines[i].StartPoint.Name == graph1.Points[minIndex].Name && graph1.Lines[i].EndPoint.Name == graph1.Points[array[1]].Name)
                {
                    lines.Add(graph1.Lines[i]);
                }
            }
            if (k == 0)
                shortestPathStep.Add(new ShortestPathStep(lines, null, d, descNext));

            return minIndex;
        }

        public List<ShortestPathStep> Run(int source, int sink, Graph graph1)
        {
            int[] previous = new int[verticesCount];
            int[] distances = new int[verticesCount];
            bool[] visited = new bool[verticesCount];

            Dictionary<Point, int> d = new()
            {
                { graph1.Points[source], 0 }
            };
            string descNext = "Инициализируем расстояния до всех вершин как \"бесконечность\", кроме исходной вершины: расстояние до нее равно 0.";
            shortestPathStep.Add(new ShortestPathStep(null, null, d, descNext));

            for (int i = 0; i < verticesCount; i++)
            {
                previous[i] = -1;
                distances[i] = int.MaxValue;
                visited[i] = false;
            }

            distances[source] = 0;
            for (int count = 0; count < verticesCount - 1; count++)
            {
                int[] array = new int[3];
                int u = FindMinDistance(distances, visited, graph1, array);
                visited[u] = true;

                if (u == sink)
                {
                    k = 1;
                }

                for (int v = 0; v < verticesCount; v++)
                {
                    if (!visited[v] && graph[u, v] != 0 && distances[u] != int.MaxValue &&
                        distances[u] + graph[u, v] < distances[v])
                    {
                        List<Line> lines = new();
                        Dictionary<Point, int> dictionary = new();
                        distances[v] = distances[u] + graph[u, v];

                        dictionary.Add(graph1.Points[v], distances[v]);
                        array[0] = distances[v];
                        array[1] = u;
                        array[2] = v;

                        for (int i = 0; i < graph1.Lines.Count; i++)
                        {
                            if ((graph1.Lines[i].StartPoint.Name == graph1.Points[u].Name && graph1.Lines[i].EndPoint.Name == graph1.Points[v].Name) || 
                                 graph1.Lines[i].StartPoint.Name == graph1.Points[v].Name && graph1.Lines[i].EndPoint.Name == graph1.Points[u].Name)
                            {
                                lines.Add(graph1.Lines[i]);
                            }
                        }
                        previous[v] = u;
                        if (k == 0)
                            shortestPathStep.Add(new ShortestPathStep(lines, null, dictionary, $"Нашли путь от {graph1.Points[u].Name} до {graph1.Points[v].Name} с расстоянием в {graph[u, v]}. " +
                                                 $"Общий путь от начального пункта до точки {graph1.Points[v].Name} равен {distances[v]}"));
                    }
                }
            }

            RecoverPath(graph1, source, sink, previous, distances);


            return shortestPathStep;
        }

        private static void RecoverPath(Graph graph, int source, int sink, int[] previous, int[] distances)
        {
            List<Line> lightedOnLines = new();
            List<Line> lightedOffLines = new();
            int current = sink;
            while (current != -1)
            {
                if (previous[current] != -1)
                    for (int i = 0; i < graph.Lines.Count; i++)
                    {
                        if ((graph.Lines[i].StartPoint.Name == graph.Points[current].Name && graph.Lines[i].EndPoint.Name == graph.Points[previous[current]].Name) || 
                             graph.Lines[i].StartPoint.Name == graph.Points[previous[current]].Name && graph.Lines[i].EndPoint.Name == graph.Points[current].Name)
                        {
                            lightedOnLines.Add(graph.Lines[i]);
                        }
                        else
                        {
                            lightedOffLines.Add(graph.Lines[i]);
                        }
                    }
                current = previous[current];
            }
            shortestPathStep.Add(new ShortestPathStep(lightedOnLines, lightedOffLines, null, 
                                 $"Кратчайшее расстояние от вершины {graph.Points[source].Name} до вершины {graph.Points[sink].Name} равно {distances[sink]}."));
        }
    }
}
