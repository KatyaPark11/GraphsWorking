using Assets.Scripts.GraphComponents;
using System.Collections.Generic;

namespace Assets.Scripts.TransportNetwork
{
    /// <summary>
    /// Класс для получения шагов поиска максимального потока транспортной сети.
    /// </summary>
    public class TransportStepsGetting
    {
        /// <summary>
        /// Метод для получения шагов поиска максимального потока транспортной сети.
        /// </summary>
        /// <param name="graph">Копия графа для поиска максимального потока транспортной сети.</param>
        /// <param name="sourcePointIndex">Индекс точки истока.</param>
        /// <param name="sinkPointIndex">Индекс точки стока.</param>
        public static List<TransportStep> GetTransportSteps(Graph graph, int sourcePointIndex, int sinkPointIndex)
        {
            Point sourcePoint = graph.Points[sourcePointIndex];
            Point sinkPoint = graph.Points[sinkPointIndex];
            /* Ваш кодик должен быть туть
            * Поля, которые могут понадобиться:
            * point.LinkedLines 
            * line.Weight (в данном случае это загруженность линии в формате число_задействованных_единиц/максимальное_число_единиц) 
            * Метод для проверки точки на то, является она начальной или конечной в указанной линии: point.IsStartPoint(Line line); */
            return new List<TransportStep>();
        }
    }
}
