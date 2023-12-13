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
            return traversalStepList;
        }

        public static void DFS(char v, bool[] visited, Graph graph)
        {
            int index = graph.GetPointIndex(v);
            visited[index] = true;

            for (int i = 0; i < graph.Points[index].LinkedLines.Count; i++)
            {
                Line curLine = graph.Points[index].LinkedLines[i];
                int neighbourIndex = graph.GetPointIndex(curLine.EndPoint.Name);
                if (!visited[neighbourIndex])
                {
                    AddStepDFS(v, i, index, graph);
                    DFS(curLine.EndPoint.Name, visited, graph);
                }
            }
        }

        private static void AddStepDFS(char v, int lineIndex, int pointIndex, Graph graph)
        {
            string descNext;
            string descPrev;

            char targetVertex = graph.Points[pointIndex].LinkedLines[lineIndex].EndPoint.Name;
            List<Line> LightedOnLines = new() { graph.Points[pointIndex].LinkedLines[lineIndex] };

            if (lineIndex == 0)
            {
                descNext = "Всем вершинам графа присваивается статус не посещенной. " +
                           "Начинаем обход с выбранной вершины и помечаем ее как посещенную.\r\n" +
                           "Из вершины " + v + " переходим по первой смежной грани " +
                           "к вершине " + targetVertex + " и помечаем ее как посещенную.";
            }
            else
            {
                descNext = "Из вершины " + v + " переходим по следующей смежной грани " +
                           "к вершине " + targetVertex + " и помечаем ее как посещенную.";
            }

            descPrev = traversalStepList.Count > 0 ? "Вернулись к вершине " + v + " чтобы продолжить обход."
                                                     : "Начало обхода с вершины " + v + ".";

            TraversalStep traversalStep = new(LightedOnLines, null, descNext, descPrev);
            traversalStepList.Add(traversalStep);
        }

        /// <summary>
        /// Метод для получения шагов обхода графа в ширину.
        /// </summary>
        /// <param name="graph">Копия графа для обхода в ширину.</param>
        public static List<TraversalStep> GetBreadthFirstSteps(Graph graph)
        {
            traversalStepList = new();
            bool[] visited = new bool[graph.Points.Count];
            BFS('A', visited, graph);
            return traversalStepList;
        }

        public static void BFS(char start, bool[] visited, Graph graph)
        {
            Queue<char> queue = new();
            int startIndex = graph.GetPointIndex(start);
            queue.Enqueue(start);
            visited[startIndex] = true;

            while (queue.Count > 0)
            {
                char current = queue.Dequeue();
                int index = graph.GetPointIndex(current);

                for (int i = 0; i < graph.Points[index].LinkedLines.Count; i++)
                {
                    Line curLine = graph.Points[index].LinkedLines[i];
                    int neighbourIndex = graph.GetPointIndex(curLine.EndPoint.Name);

                    if (!visited[neighbourIndex])
                    {
                        queue.Enqueue(curLine.EndPoint.Name);
                        visited[neighbourIndex] = true;
                        AddStepBFS(current, i, index, graph);
                    }
                }
            }
        }

        private static void AddStepBFS(char v, int lineIndex, int pointIndex, Graph graph)
        {
            string descNext;
            string descPrev;
            List<Line> LightedOnLines = new() { graph.Points[pointIndex].LinkedLines[lineIndex] };
            if (lineIndex == 0)
                descNext = "Всем вершинам графа присваивается статус не посещенной. " +
                    "Выбирается первая вершина и помечается как посещенная (и заносится в очередь). " +
                    "Все ее соседние вершины заносятся в очередь, после чего она удаляется из очереди." +
                    $"Посещенной является вершина {v}, а не посещенной {graph.Points[pointIndex].LinkedLines[lineIndex].EndPoint.Name}.";
            else
                descNext = "Посещается первая вершина из очереди (если она не помечена как посещенная). " +
                        "Все ее соседние вершины заносятся в очередь, после чего она удаляется из очереди." +
                        $"Посещенной является вершина {v}, а не посещенной {graph.Points[pointIndex].LinkedLines[lineIndex].EndPoint.Name}.";
            descPrev = "Вернулись на предыдущий этап.";
            TraversalStep traversalStep = new(LightedOnLines, null, descNext, descPrev);
            traversalStepList.Add(traversalStep);
        }
    }
}