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

            string descNext = "Пройденные вершины - ";
            for (int i = 0; i < graph.Points.Count; i++)
            {
                descNext = descNext + $"{graph.Points[i].Name} ";
            }
            descNext = descNext.TrimEnd() + "\nВсе вершины пройдены. Обход графа в глубину закончен.";
            TraversalStep traversalStep = new(null, null, descNext);
            traversalStepList.Add(traversalStep);

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
                    AddStepDFS(v, i, index, graph, visited);
                    DFS(curLine.EndPoint.Name, visited, graph);
                }
            }
        }

        private static void AddStepDFS(char v, int lineIndex, int pointIndex, Graph graph, bool[] visited)
        {
            string descNext;

            string notPassed = "";
            string passed = "";
            for (int i = 0; i < graph.Points.Count; i++)
            {
                if (visited[i])
                {
                    passed += $"{graph.Points[i].Name} ";
                }
                if (!visited[i])
                {
                    notPassed += $"{graph.Points[i].Name} ";
                }
            }
            descNext = $"Пройденные вершины - {passed.TrimEnd()}. Непройденные вершины - {notPassed.TrimEnd()}.\n";


            char targetVertex = graph.Points[pointIndex].LinkedLines[lineIndex].EndPoint.Name;
            List<Line> LightedOnLines = new() { graph.Points[pointIndex].LinkedLines[lineIndex] };

            if (lineIndex == 0)
            {
                descNext = descNext + "Всем вершинам графа присваивается статус не посещенной. " +
                           "Начинаем обход с выбранной вершины и помечаем ее как посещенную.\r\n" +
                           "Из вершины " + v + " переходим по первой смежной грани " +
                           "к вершине " + targetVertex + " и помечаем ее как посещенную.";
            }
            else
            {
                descNext = descNext + "Из вершины " + v + " переходим по следующей смежной грани " +
                           "к вершине " + targetVertex + " и помечаем ее как посещенную.";
            }

            TraversalStep traversalStep = new(LightedOnLines, null, descNext);
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

            string descNext = $"Очередь - {graph.Points[0].Name}" +
                $"\nПройденные вершины - {graph.Points[0].Name}\nНепройденные вершины - ";
            for (int i = 1; i < graph.Points.Count; i++)
            {
                descNext = descNext + $"{graph.Points[i].Name} ";
            }
            descNext = descNext.TrimEnd() + "\nВсем вершинам графа присваивается значение не посещенная, кроме начальной.";
            TraversalStep traversalStep = new(null, null, descNext);
            traversalStepList.Add(traversalStep);

            BFS('A', visited, graph);

            descNext = "Пройденные вершины - ";
            for (int i = 0; i < graph.Points.Count; i++)
            {
                descNext = descNext + $"{graph.Points[i].Name} ";
            }
            descNext = descNext.TrimEnd() + "\nВсе вершины пройдены. Обход грфафа в ширину закончен.";
            TraversalStep traversalStep1 = new(null, null, descNext);
            traversalStepList.Add(traversalStep1);

            return traversalStepList;
        }

        public static void BFS(char start, bool[] visited, Graph graph)
        {
            Queue<char> queue = new();

            List<char> que = new List<char> { start };
            int count = 0;

            int startIndex = graph.GetPointIndex(start);
            queue.Enqueue(start);
            visited[startIndex] = true;

            while (queue.Count > 0)
            {
                que.Remove(queue.Peek());

                char current = queue.Dequeue();
                int index = graph.GetPointIndex(current);

                for (int i = 0; i < graph.Points[index].LinkedLines.Count; i++)
                {
                    Line curLine = graph.Points[index].LinkedLines[i];
                    int neighbourIndex = graph.GetPointIndex(curLine.EndPoint.Name);

                    if (!visited[neighbourIndex])
                    {
                        queue.Enqueue(curLine.EndPoint.Name);
                        que.Add(curLine.EndPoint.Name);
                        visited[neighbourIndex] = true;
                        AddStepBFS(current, i, index, graph, visited, que, count);
                        count++;
                    }
                }
            }
        }

        private static void AddStepBFS(char v, int lineIndex, int pointIndex, Graph graph, bool[] visited, List<char> que, int count)
        {
            string descNext;

            List<Line> LightedOnLines = new() { graph.Points[pointIndex].LinkedLines[lineIndex] };
            if (count < graph.Points[pointIndex].LinkedLines.Count)
                descNext = "Выбирается вершина и помечается как посещенная (и заносится в очередь).";
            else
                descNext = "Посещается первая вершина из очереди (если она не помечена как посещенная). " +
                        "Её соседние вершины заносятся в очередь, после чего она удаляется из очереди.";

            string notPassed = "";
            string passed = "";
            for (int i = 0; i < graph.Points.Count; i++)
            {
                if (visited[i])
                {
                    passed = passed + $"{graph.Points[i].Name} ";
                }
                if (!visited[i])
                {
                    notPassed = notPassed + $"{graph.Points[i].Name} ";
                }
            }
            descNext += $"\nПройденные вершины - {passed.TrimEnd()}. Непройденные вершины - {notPassed.TrimEnd()}\n";

            passed = "";
            foreach (char c in que)
            {
                passed = passed + $"{c} ";
            }
            descNext += $"Очередь - {passed.TrimEnd()}.";

            TraversalStep traversalStep = new(LightedOnLines, null, descNext);
            traversalStepList.Add(traversalStep);
        }
    }
}