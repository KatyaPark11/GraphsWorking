using Assets.Scripts.GraphComponents;
using System.Collections.Generic;

namespace Assets.Scripts.SpanningTree
{
    /// <summary>
    /// Класс для получения шагов создания минимального остовного дерева.
    /// </summary>
    public class SpanningTreeStepsGetting
    {
        /// <summary>
        /// Метод для получения шагов создания минимального остовного дерева.
        /// </summary>
        /// <param name="graph">Копия графа для создания минимального остовного дерева.</param>
        public static List<SpanningTreeStep> GetSpanningTreeSteps(Graph graph)
        {
            /* Ваш кодик должен быть туть. Используйте алгоритм Дейкстры для вычисления кратчайшего пути.
            * Поля, которые могут понадобиться:
            * point.LinkedLines 
            * line.Weight (В данном случае это вес линии)
            * Метод, удаляющий линию из списка: graph.RemoveLine(int lineIndex); второй параметр оставляем по умолчанию! 
            * Метод для проверки точки на то, является она начальной или конечной в указанной линии: point.IsStartPoint(Line line); */
            return new List<SpanningTreeStep>();
        }
    }
}
