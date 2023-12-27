using Assets.Scripts.GraphComponents;
using Assets.Scripts.SpanningTree;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static Assets.Scripts.VarsHolder;

namespace Assets.Scripts.GraphEditors
{
    public class Saving : MonoBehaviour
    {
        public TMP_InputField GraphNameIF;
        public TMP_Dropdown DownloadDropdown;

        public void Save()
        {
            if (AlgorithmManager.SpanningTreeSteps != null)
                SaveSpanningTree();
            else
                UsualSave(MainGraph);
        }

        private void UsualSave(Graph graph)
        {
            string graphName = GraphNameIF.text;
            graph.Save("../", graphName);
            foreach (TMP_Dropdown.OptionData option in DownloadDropdown.options)
                if (option.text.Equals(graphName)) return;
            List<TMP_Dropdown.OptionData> options = DownloadDropdown.options.ToList();
            options.Add(new TMP_Dropdown.OptionData() { text = graphName });
            DownloadDropdown.options = options;
        }

        private void SaveSpanningTree()
        {
            SpanningTreeStep lastStep = AlgorithmManager.SpanningTreeSteps[^1];
            Graph graphCopy = new(MainGraph.Lines, MainGraph.Points)
            {
                Type = MainGraph.Type
            };
            for (int i = 0; i < graphCopy.Lines.Count; i++)
            {
                if (lastStep.RemovedLines.Contains(graphCopy.Lines[i]))
                {
                    graphCopy.RemoveLine(graphCopy.Lines[i], true);
                    i--;
                }
            }
            UsualSave(graphCopy);
        }
    }
}
