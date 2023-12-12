using Assets.Scripts.GraphComponents;
using System.Collections.Generic;

namespace Assets.Scripts.GraphTraversal
{
    /// <summary>
    /// Класс для получения шагов обхода графа.
    /// </summary>
    public class TraversalStepsGetting
    {
        public static List<TraversalStep> traversalStepList;
        /// <summary>
        /// Метод для получения шагов обхода графа в глубину.
        /// </summary>
        /// <param name="graph">Копия графа для обхода в глубину.</param>
        public static List<TraversalStep> GetDepthFirstSteps(Graph graph)
        {
            traversalStepList = new();
            bool[] visited = new bool[graph.Points.Count];
            DFS('A', visited, graph);

            /* Ваш кодик должен быть туть
             * Поля, которые могут понадобиться:
             * graph.Points[index].LinkedLines 
             * Метод для проверки точки на то, является она начальной или конечной в указанной линии: point.IsStartPoint(Line line); */
            return traversalStepList;
        }

        public static void DFS(char v, bool[] visited, Graph graph)
        {
            int index = FindIndex(v, graph);

            for (int i = 0; i < graph.Points[index].LinkedLines.Count; i++)
            {
                if (!visited[index] || v == 'A')
                {
                    visited[index] = true;
                    AddStepDFS(v, i, index, graph);
                    DFS(graph.Points[i].LinkedLines[i].EndPoint.Name, visited, graph);
                }
            }
        }

        /// <summary>
        /// Метод для получения шагов обхода графа в ширину.
        /// </summary>
        /// <param name="graph">Копия графа для обхода в ширину.</param>
        public static List<TraversalStep> GetBreadthFirstSteps(Graph graph)
        {
            traversalStepList = new();
            BFS('A', graph.Points.Count, graph);

            /* Ваш кодик должен быть туть
             * Поля, которые могут понадобиться:
             * graph.Points[index].LinkedLines
             * Метод для проверки точки на то, является она начальной или конечной в указанной линии: point.IsStartPoint(Line line); */
            return traversalStepList;
        }

        public static void BFS(char start, int amount, Graph graph)
        {
            int index = FindIndex(start, graph);
            bool[] visited = new bool[amount];
            Queue<char> queue = new();

            visited[index] = true;
            queue.Enqueue(start);

            while (queue.Count != 0)
            {
                start = queue.Dequeue();

                for (int i = 0; i < graph.Points[index].LinkedLines.Count; i++)
                {
                    char neighbor = graph.Points[index].LinkedLines[i].EndPoint.Name;
                    int index1 = FindIndex(neighbor, graph);
                    if (!visited[index1])
                    {
                        AddStepBFS(start, i, index, graph);
                        visited[index1] = true;
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }

        private static void AddStepBFS(char v, int i, int index, Graph graph)
        {
            string descNext;
            string descPrev;
            List<Line> LightedOnLines = new() { graph.Points[index].LinkedLines[i] };
            List<Line> LightedOffLines = new();
            if (i != 0)
            {
                LightedOffLines.Add(graph.Points[index].LinkedLines[i - 1]);
            }
            else if (i == 0)
            {
                descNext = "Всем вершинам графа присваивается значение не посещенная. " +
                    "Выбирается первая вершина и помечается как посещенная (и заносится в очередь)." +
                    "Посещается первая вершина из очереди (если она не помечена как посещенная). " +
                    "Все ее соседние вершины заносятся в очередь. После этого она удаляется из очереди." +
                    $"Посещенной является вершина {v}, а не посещенной {graph.Points[index].LinkedLines[i].EndPoint.Name}.";
            }
            descNext = ". Посещается первая вершина из очереди (если она не помечена как посещенная). " +
                    "Все ее соседние вершины заносятся в очередь. После этого она удаляется из очереди." +
                    $"Посещенной является вершина {v}, а не посещенной {graph.Points[index].LinkedLines[i].EndPoint.Name}.";
            descPrev = "Вернулись на предыдущий этап.";
            TraversalStep traversalStep = new(LightedOnLines, LightedOffLines, descNext, descPrev);
            traversalStepList.Add(traversalStep);
        }

        private static void AddStepDFS(char v, int i, int index, Graph graph)
        {
            string descNext;
            string descPrev;
            List<Line> LightedOnLines = new() { graph.Points[index].LinkedLines[i] };
            List<Line> LightedOffLines = new();
            if (i != 0)
            {
                LightedOffLines.Add(graph.Points[index].LinkedLines[i - 1]);
            }
            else if (i == 0)
            {
                descNext = "Всем вершинам графа присваивается значение не посещенная. " +
                    "Выбирается первая вершина и помечается как посещенная.\r\n" +
                    "Для последней помеченной как посещенная вершины выбирается смежная вершина, " +
                    "являющаяся первой помеченной как не посещенная, и ей присваивается значение посещенная. " +
                    $"Посещенной является вершина {v}, а не посещенной {graph.Points[index].LinkedLines[i].EndPoint.Name}.";
            }
            descNext = "Для последней помеченной как посещенная вершины выбирается смежная вершина, " +
                    "являющаяся первой помеченной как не посещенная, и ей присваивается значение посещенная. " +
                    $"Посещенной является вершина {v}, а не посещенной {graph.Points[index].LinkedLines[i].EndPoint.Name}.";
            descPrev = "Вернулись на предыдущий этап.";
            TraversalStep traversalStep = new(LightedOnLines, LightedOffLines, descNext, descPrev);
            traversalStepList.Add(traversalStep);
        }

        private static int FindIndex(char v, Graph graph)
        {
            for (int j = 0; j < graph.Points.Count; j++)
            {
                char name = graph.Points[v - 'A'].Name;
                if (v == name)
                {
                    return j;
                }
            }
            return 0;
        }
    }
}