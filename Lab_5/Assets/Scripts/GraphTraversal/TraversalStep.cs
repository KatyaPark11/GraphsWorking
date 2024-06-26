﻿using Assets.Scripts.GraphComponents;
using System.Collections.Generic;

namespace Assets.Scripts.GraphTraversal
{
    /// <summary>
    /// Класс для реализации шага обхода графов.
    /// </summary>
    public class TraversalStep
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
        /// Пояснение к этому шагу, если он был следующим.
        /// </summary>
        public readonly string DescNext;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="lightedOnLines">Список подсвечиваемых на данном шаге линий.</param>
        /// <param name="lightedOffLines">Список возвращаемых к исходному состоянию на данном шаге линий.</param>
        /// <param name="descNext">Пояснение к этому шагу, если он был следующим.</param>
        public TraversalStep(List<Line> lightedOnLines, List<Line> lightedOffLines, string descNext)
        {
            LightedOnLines = lightedOnLines;
            LightedOffLines = lightedOffLines;
            DescNext = descNext;
        }
    }
}
