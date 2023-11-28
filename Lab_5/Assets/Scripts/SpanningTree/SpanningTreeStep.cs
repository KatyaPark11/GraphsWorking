using Assets.Scripts.GraphComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.SpanningTree
{
    public class SpanningTreeStep
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
        /// Список удаляемых на данном шаге линий.
        /// </summary>
        public List<Line> RemovedLines { get; private set; }
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
        /// <param name="removedLines">Список удаляемых на данном шаге линий.</param>
        /// <param name="descNext">Пояснение к этому шагу, если он был следующим.</param>
        /// <param name="descPrev">Пояснение к этому шагу, если он был предыдущим.</param>
        public SpanningTreeStep(List<Line> lightedOnLines, List<Line> lightedOffLines, List<Line> removedLines, string descNext, string descPrev)
        {
            LightedOnLines = lightedOnLines;
            LightedOffLines = lightedOffLines;
            RemovedLines = removedLines;
            DescNext = descNext;
            DescPrev = descPrev;
        }
    }
}
