using Assets.Scripts.GraphComponents;
using System.Collections.Generic;

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

            spanningTreeStep = graphKruskal.MinimumSpanningTree(graph);

            string DescNext = "Остальные пути удаляем, так как они принадлежат какому-то множеству.";
            string DescPrev = "";
            List<Line> lightedOnLines = new();
            List<Line> lightedOffLines = new();
            List<Line> removedLines = new();

            int s = 0;
            for (int k = 0; k < graph.Points.Count; k++)
            {
                for (int j = 0; j < graph.Points[k].LinkedLines.Count; j++)
                {
                    foreach (var steps in spanningTreeStep)
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

            SpanningTreeStep spanningTree = new(lightedOnLines, lightedOffLines, removedLines, DescNext, DescPrev);
            spanningTreeStep.Add(spanningTree);
            return spanningTreeStep;
        }
    }

    public class KruskalAlgorithm
    {
        public static List<SpanningTreeStep> spanningTreeSteps;
        int V;
        List<Edge> edges;

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
            string DescNext;
            string DescPrev = "";

            List<Edge> result = new List<Edge>();
            int i = 0, e = 0;
            edges.Sort((a, b) => a.Weight.CompareTo(b.Weight));

            Subset[] subsets = new Subset[V];
            for (int v = 0; v < V; v++)
            {
                subsets[v] = new Subset { Parent = v, Rank = 0 };
            }

            while (e < V - 1)
            {
                List<Line> lightedOnLines = new List<Line>();
                List<Line> lightedOffLines = new List<Line>();
                List<Line> removedLines = new List<Line>();

                Edge nextEdge = edges[i++];

                int x = Find(subsets, nextEdge.Source);
                int y = Find(subsets, nextEdge.Destination);
                DescNext = $"Проверяем лежат ли вершины {graph.Points[nextEdge.Destination].Name} " +
                    $"и {graph.Points[nextEdge.Source].Name} в разных подмножествах. ";

                if (x != y)
                {
                    DescNext = $"{DescNext}Вершины находятся в разных подмножествах, значит, ребро между ними оставляем";

                    lightedOnLines = Add(graph, lightedOnLines, nextEdge.Source, nextEdge.Destination);
                    result.Add(nextEdge);
                    Union(subsets, x, y);

                    e++;
                }
                else
                {
                    DescNext += $"Вершины находятся в одном подмножестве, значит, ребро между ними удаляем";
                    removedLines = Add(graph, lightedOnLines, nextEdge.Source, nextEdge.Destination);
                }

                SpanningTreeStep spanningTree = new(lightedOnLines, lightedOffLines, removedLines, DescNext, DescPrev);
                spanningTreeSteps.Add(spanningTree);
                DescNext = "";
            }

            return spanningTreeSteps;
        }

        private static List<Line> Add(Graph graph, List<Line> lightedOnLines, int start, int end)
        {
            for (int l = 0; l < graph.Lines.Count; l++)
            {
                for (int k = 0; k < graph.Points.Count; k++)
                {
                    if ((graph.Lines[l].StartPoint == graph.Points[start]) && (graph.Lines[l].EndPoint == graph.Points[end]))
                    {
                        lightedOnLines.Add(graph.Lines[l]);
                    }
                }
            }
            return lightedOnLines;
        }
    }
}
