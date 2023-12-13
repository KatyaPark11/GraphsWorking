using Assets.Scripts.GraphComponents;
using Assets.Scripts.GraphTraversal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

namespace Assets.Scripts.TransportNetwork
{
    /// <summary>
    /// Класс для получения шагов поиска максимального потока транспортной сети.
    /// </summary>
    public class TransportStepsGetting
    {
        public static List<TransportStep> transportSteps = new();
        /// <summary>
        /// Метод для получения шагов поиска максимального потока транспортной сети.
        /// </summary>
        /// <param name="graph">Копия графа для поиска максимального потока транспортной сети.</param>
        /// <param name="sourcePointIndex">Индекс точки истока.</param>
        /// <param name="sinkPointIndex">Индекс точки стока.</param>
        public static List<TransportStep> GetTransportSteps(Graph graph, int sourcePointIndex, int sinkPointIndex)
        {
            transportSteps = new();
            foreach (Line line in graph.Lines)
            {
                string[] weights = line.Weight.Split('/');
                string newWeight = $"0/{weights[1]}";
                line.Weight = newWeight;
                line.WeightIF.text = newWeight;
            }
            int maxFlow = MaxFlow(graph, sourcePointIndex, sinkPointIndex);
            transportSteps.Add(new TransportStep(null, null, null, $"Максимальный поток равен {maxFlow}", null));
            return transportSteps;
        }

        public static int MaxFlow(Graph graph, int source, int sink)
        {
            int[,] graphMatrix = new int[graph.Points.Count, graph.Points.Count];
            for (int i = 0; i < graph.Lines.Count; i++)
            {
                graphMatrix[graph.GetPointIndex(graph.Lines[i].StartPoint.Name), graph.GetPointIndex(graph.Lines[i].EndPoint.Name)] = int.Parse(graph.Lines[i].Weight.Split('/')[1]);
            }

            int[] parent = new int[graph.Points.Count];
            int maxFlow = 0;
            int[] temp = new int[graph.Points.Count];

            while (BFS(source, sink, parent, graph.Points.Count, graphMatrix))
            {
                List<Line> lightedOnThisStep = new();
                Dictionary<Line, int> lineChanges = new();
                int pathFlow = int.MaxValue;

                for (int v = sink; v != source; v = parent[v])
                {
                    int u = parent[v];
                    pathFlow = Math.Min(pathFlow, graphMatrix[u, v]);
                    temp[v] = u;

                    Line line = null;
                    foreach (Line l in graph.Points[u].LinkedLines)
                    {
                        if (l.EndPoint == graph.Points[v])
                        {
                            line = l;
                            break;
                        }
                    }
                    if (line != null)
                    {
                        lightedOnThisStep.Add(line);
                    }
                    transportSteps.Add(new TransportStep(lightedOnThisStep.ToList(), null, new Dictionary<Line, int>(lineChanges), 
                        $"Промежуточный путь найден, текущий минимальный поток: {pathFlow}", null));
                }

                for (int v = sink; v != source; v = temp[v])
                {
                    int u = temp[v];
                    graphMatrix[u, v] -= pathFlow;
                    graphMatrix[v, u] += pathFlow;

                    Line line = null;
                    foreach (Line l in graph.Points[u].LinkedLines)
                    {
                        if (l.EndPoint == graph.Points[v])
                        {
                            line = l;
                            break;
                        }
                    }
                    if (line != null && graphMatrix[u, v] >= 0)
                    {
                        int newFlow = int.Parse(line.Weight.Split('/')[0]) + pathFlow;
                        line.Weight = newFlow.ToString() + "/" + line.Weight.Split('/')[1];
                        lineChanges[line] = newFlow;
                    }
                }
                transportSteps.Add(new TransportStep(lightedOnThisStep, null, new Dictionary<Line, int>(lineChanges), $"Поток увеличен на {pathFlow}", null));

                maxFlow += pathFlow;
            }

            return maxFlow;
        }
            
        private static bool BFS(int source, int sink, int[] parent, int countPoint, int[,] residualGraph)
        {
            bool[] visited = new bool[countPoint];
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(source);
            visited[source] = true;
            parent[source] = -1;

            while (queue.Count > 0)
            {
                int u = queue.Dequeue();

                for (int v = 0; v < countPoint; v++)
                {
                    if (!visited[v] && residualGraph[u, v] > 0)
                    {
                        queue.Enqueue(v);
                        parent[v] = u;
                        visited[v] = true;
                    }
                }
            }

            return (visited[sink] == true);
        }
    }
}
