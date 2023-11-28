using Assets.Scripts.GraphComponents;
using System.Collections.Generic;

namespace Assets.Scripts.ShortestPath
{
    public class ShortestPathStep
    {
        /// <summary>
        /// Список подсвечиваемых на данном шаге линий.
        /// </summary>
        public List<Line> LightedOnLines { get; private set; }
        /// <summary>
        /// Список возвращаемых к исходному состоянию на данном шаге линий.
        /// </summary>
        public List<Line> LightedOffLines { get; private set; }
        /// <summary>
        /// Словарь, сопоставляющий точку с изменённым в соответствии с данным шагом весом кратчайшего пути.
        /// </summary>
        public Dictionary<Point, int> PointShortestPathPairs { get; private set; }
        /// <summary>
        /// Пояснение к этому шагу, если он был следующим.
        /// </summary>
        public readonly string DescNext;
        /// <summary>
        /// Пояснение к этому шагу, если он был предыдущим.
        /// </summary>
        public readonly string DescPrev;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="lightedOnLines">Список подсвечиваемых на данном шаге линий.</param>
        /// <param name="lightedOffLines">Список возвращаемых к исходному состоянию на данном шаге линий.</param>
        /// <param name="pointShortestPathPairs">Словарь, сопоставляющий точку с изменённой в соответствии с данным шагом длиной кратчайшего пути.</param>
        /// <param name="descNext">Пояснение к этому шагу, если он был следующим.</param>
        /// <param name="descPrev">Пояснение к этому шагу, если он был предыдущим.</param>
        public ShortestPathStep(List<Line> lightedOnLines, List<Line> lightedOffLines, Dictionary<Point, int> pointShortestPathPairs, string descNext, string descPrev)
        {
            LightedOnLines = lightedOnLines;
            LightedOffLines = lightedOffLines;
            PointShortestPathPairs = pointShortestPathPairs;
            DescNext = descNext;
            DescPrev = descPrev;
        }
    }
}
