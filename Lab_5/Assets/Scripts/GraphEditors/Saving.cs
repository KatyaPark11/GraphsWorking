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
            string graphName = GraphNameIF.text;
            MainGraph.Save("../", graphName);
            foreach (TMP_Dropdown.OptionData option in DownloadDropdown.options)
                if (option.text.Equals(graphName)) return;
            List<TMP_Dropdown.OptionData> options = DownloadDropdown.options.ToList();
            options.Add(new TMP_Dropdown.OptionData() { text = graphName });
            DownloadDropdown.options = options;
        }
    }
}
