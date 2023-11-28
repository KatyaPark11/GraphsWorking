using Assets.Scripts.GraphComponents;
using System.Collections.Generic;

namespace Assets.Scripts.GraphTraversal
{
    /// <summary>
    /// Класс для получения шагов обхода графа.
    /// </summary>
    public class TraversalStepsGetting
    {
        /// <summary>
        /// Метод для получения шагов обхода графа в глубину.
        /// </summary>
        /// <param name="graph">Копия графа для обхода в глубину.</param>
        public static List<TraversalStep> GetDepthFirstSteps(Graph graph)
        {
            /* Ваш кодик должен быть туть
             * Поля, которые могут понадобиться:
             * graph.Points[index].LinkedLines 
             * Метод для проверки точки на то, является она начальной или конечной в указанной линии: point.IsStartPoint(Line line); */
            return new List<TraversalStep>();
        }

        /// <summary>
        /// Метод для получения шагов обхода графа в ширину.
        /// </summary>
        /// <param name="graph">Копия графа для обхода в ширину.</param>
        public static List<TraversalStep> GetBreadthFirstSteps(Graph graph)
        {
            /* Ваш кодик должен быть туть
             * Поля, которые могут понадобиться:
             * graph.Points[index].LinkedLines
             * Метод для проверки точки на то, является она начальной или конечной в указанной линии: point.IsStartPoint(Line line); */
            return new List<TraversalStep>();
        }
    }
}