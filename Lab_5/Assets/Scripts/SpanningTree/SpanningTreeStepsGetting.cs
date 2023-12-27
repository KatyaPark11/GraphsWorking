using Assets.Scripts.GraphComponents;
using System.Collections.Generic;
using System.Net;

namespace Assets.Scripts.SpanningTree
{
    /// <summary>
    /// Класс для получения шагов создания минимального остовного дерева.
    /// </summary>
    public class SpanningTreeStepsGetting
    {
        public static List<SpanningTreeStep> spanningTreeStep;
        /// <summary>
        /// Метод для получения шагов создания минимального остовного дерева.
        /// </summary>
        /// <param name="graph">Копия графа для создания минимального остовного дерева.</param>
        public static List<SpanningTreeStep> GetSpanningTreeSteps(Graph graph)
        {
            spanningTreeStep = new();
            if (graph.Type.Equals("Транспортная сеть"))
            {
                foreach (Line line in graph.Lines)
                {
                    string[] weights = line.Weight.Split('/');
                    string newWeight = weights[1];
                    line.Weight = newWeight;
                    line.WeightIF.text = newWeight;
                }
            }
            int countPoint = graph.Points.Count;
            KruskalAlgorithm graphKruskal = new(countPoint);

            for (int i = 0; i < graph.Lines.Count; i++)
            {
                int point1 = 0;
                int point2 = 0;

                for (int k = 0; k < countPoint; k++)
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
                graphKruskal.AddEdge(point1, point2, int.Parse(graph.Lines[i].Weight));
            }

            return graphKruskal.MinimumSpanningTree(graph);
        }
    }

    public class KruskalAlgorithm
    {
        public static List<SpanningTreeStep> spanningTreeSteps;
        int V;
        private readonly List<Edge> edges;

        public KruskalAlgorithm(int v)
        {
            V = v;
            edges = new List<Edge>();
        }

        public void AddEdge(int source, int destination, int weight)
        {
            edges.Add(new Edge(source, destination, weight));
        }

        public bool Compare(int i, int j, int weight)
        {
            foreach (Edge e in edges)
            {
                if ((j == e.Source) && (i == e.Destination) && (weight == e.Weight))
                {
                    return false;
                }
            }
            return true;
        }

        class Edge
        {
            public int Source, Destination, Weight;

            public Edge(int source, int destination, int weight)
            {
                Source = source;
                Destination = destination;
                Weight = weight;
            }
        }

        class Subset
        {
            public int Parent, Rank;
        }

        int Find(Subset[] subsets, int i)
        {
            if (subsets[i].Parent != i)
            {
                subsets[i].Parent = Find(subsets, subsets[i].Parent);
            }

            return subsets[i].Parent;
        }

        void Union(Subset[] subsets, int x, int y)
        {
            int xroot = Find(subsets, x);
            int yroot = Find(subsets, y);

            if (subsets[xroot].Rank < subsets[yroot].Rank)
                subsets[xroot].Parent = yroot;
            else if (subsets[xroot].Rank > subsets[yroot].Rank)
                subsets[yroot].Parent = xroot;
            else
            {
                subsets[yroot].Parent = xroot;
                subsets[xroot].Rank++;
            }
        }

        public List<SpanningTreeStep> MinimumSpanningTree(Graph graph)
        {
            spanningTreeSteps = new();
            string DescNext = "";
            List<Line> removedLines = new();
            List<Line> lightedOnLines = new();

            List<Edge> result = new();
            int i = 0, e = 0;
            edges.Sort((a, b) => a.Weight.CompareTo(b.Weight));

            Subset[] subsets = new Subset[V];
            for (int v = 0; v < V; v++)
            {
                subsets[v] = new Subset { Parent = v, Rank = 0 };
            }
            DescNext += "Сортируем вес ребер по возрастанию. Начинаем с наименьшего веса. ";
            List<Line> prevRemovedLines = new();

            while (e < V - 1)
            {
                List<Line> lightedOffLines = new();

                Edge nextEdge = edges[i++];

                int x = Find(subsets, nextEdge.Source);
                int y = Find(subsets, nextEdge.Destination);

                DescNext += $"Подмножества: {WriteLine(graph, subsets)}";
                DescNext = DescNext + $"\nПроверяем лежат ли вершины {graph.Points[nextEdge.Destination].Name} " +
                           $"и {graph.Points[nextEdge.Source].Name} в разных подмножествах. Вес их ребра - {nextEdge.Weight}. ";

                Point startPoint = graph.Points[nextEdge.Source];
                Point endPoint = graph.Points[nextEdge.Destination];
                graph.TryGetLine(startPoint, endPoint, out Line foundLine1);
                graph.TryGetLine(endPoint, startPoint, out Line foundLine2);
                if (foundLine1 != null)
                    lightedOnLines.Add(foundLine1);
                if (foundLine2 != null)
                    lightedOnLines.Add(foundLine2);

                if (prevRemovedLines.Count > 0)
                {
                    lightedOffLines.AddRange(prevRemovedLines);
                    foreach (Line line in prevRemovedLines)
                        lightedOnLines.Remove(line);
                }
                prevRemovedLines = new();

                if (x != y)
                {
                    DescNext = $"{DescNext}Вершины находятся в разных подмножествах, значит, ребро между ними оставляем " +
                               $"и соединяем их в одно подмножество.";
                    result.Add(nextEdge);
                    Union(subsets, x, y);

                    e++;
                }
                else
                {
                    DescNext += $"Вершины находятся в одном подмножестве, значит, ребро между ними не учитываем.";
                    if (foundLine1 != null)
                    {
                        prevRemovedLines.Add(foundLine1);
                        removedLines.Add(foundLine1);
                    }
                    if (foundLine2 != null)
                    {
                        prevRemovedLines.Add(foundLine2);
                        removedLines.Add(foundLine2);
                    }
                }

                List<Line> lightedOnLinesCopy = new(lightedOnLines);
                SpanningTreeStep spanningTree = new(lightedOnLinesCopy, lightedOffLines, null, DescNext);
                spanningTreeSteps.Add(spanningTree);
                DescNext = "";
            }

            AddStep(graph, subsets, removedLines);
            return spanningTreeSteps;
        }

        private void AddStep(Graph graph, Subset[] subsets, List<Line> removedLines)
        {
            string DescNext = $"Подмножества: {WriteLine(graph, subsets)}\n";
            DescNext += "Остальные пути нам не нужны, так как все вершины уже лежат в одном подмножестве. " +
                        "Таким образом, выделенные рёбра составляют минимальное остовное дерево.";
            List<Line> lightedOnLines = new();
            List<Line> lightedOffLines = new();

            int s = 0;
            for (int k = 0; k < graph.Points.Count; k++)
            {
                for (int j = 0; j < graph.Points[k].LinkedLines.Count; j++)
                {
                    foreach (var steps in spanningTreeSteps)
                    {
                        for (int u = 0; u < steps.LightedOnLines.Count; u++)
                        {
                            if (steps.LightedOnLines[u] == graph.Points[k].LinkedLines[j])
                            {
                                s = 1;
                            }
                        }
                    }
                    if (s == 0)
                    {
                        removedLines.Add(graph.Points[k].LinkedLines[j]);
                    }
                    s = 0;
                }
            }

            SpanningTreeStep spanningTree = new(lightedOnLines, lightedOffLines, removedLines, DescNext);
            spanningTreeSteps.Add(spanningTree);
        }

        private string WriteLine(Graph graph, Subset[] subsets)
        {
            string line = "";
            string[] strArray = new string[graph.Points.Count];
            int[] array = GetVertexSets(subsets);
            for (int j = 0; j < array.Length; j++)
            {
                for (int k = 0; k < graph.Points.Count; k++)
                {
                    if (array[j] == k)
                    {
                        strArray[k] = strArray[k] + $"{graph.Points[j].Name} ";
                    }
                }
            }
            foreach (string str in strArray)
            {
                if (str != null)
                {
                    line = line + "{" + $"{str.TrimEnd()}" + "} ";
                }
            }
            return line;
        }

        int[] GetVertexSets(Subset[] subsets)
        {
            int[] vertexSets = new int[V];

            for (int v = 0; v < V; v++)
            {
                vertexSets[v] = Find(subsets, v);
            }

            return vertexSets;
        }


        private static List<Line> Add(Graph graph, List<Line> lightedOnLines, int start, int end)
        {
            for (int l = 0; l < graph.Lines.Count; l++)
            {
                for (int k = 0; k < graph.Points.Count; k++)
                {
                    if (((graph.Lines[l].StartPoint == graph.Points[start]) && (graph.Lines[l].EndPoint == graph.Points[end])) ||
                         (graph.Lines[l].EndPoint == graph.Points[start]) && (graph.Lines[l].StartPoint == graph.Points[end]))
                    {
                        lightedOnLines.Add(graph.Lines[l]);
                    }
                }
            }
            return lightedOnLines;
        }
    }
}
