using Assets.Scripts.GraphComponents;
using System.Collections.Generic;

namespace Assets.Scripts.TransportNetwork
{
    /// <summary>
    /// Класс для реализации шага поиска максимального потока транспортной сети.
    /// </summary>
    public class TransportStep
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
        /// Словарь, сопоставляющий линию с изменённым в соответствии с данным шагом числом используемых единиц.
        /// </summary>
        public Dictionary<Line, int> LineUsedNumOfUnitsPairs { get; private set; }
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
        /// <param name="lineUsedNumOfUnitsPairs">Словарь, сопоставляющий линию с изменённым в соответствии с данным шагом числом используемых единиц.</param>
        /// <param name="descNext">Пояснение к этому шагу, если он был следующим.</param>
        /// <param name="descPrev">Пояснение к этому шагу, если он был предыдущим.</param>
        public TransportStep(List<Line> lightedOnLines, List<Line> lightedOffLines, Dictionary<Line, int> lineUsedNumOfUnitsPairs, string descNext, string descPrev)
        {
            LightedOnLines = lightedOnLines;
            LightedOffLines = lightedOffLines;
            LineUsedNumOfUnitsPairs = lineUsedNumOfUnitsPairs;
            DescNext = descNext;
            DescPrev = descPrev;
        }
    }
}
