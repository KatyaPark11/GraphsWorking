using Assets.Scripts.GraphComponents;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class VarsHolder : MonoBehaviour
    {
        public Button[] SceneButtons;
        public TMP_Dropdown SceneGraphType;
        public int AbjacencyMatrixButIndex;
        public int DepthFirstTravButIndex;
        public int BreadthFirstTravButIndex;
        public int ShortestPathButIndex;
        public int TransportNetworkButIndex;

        public static Graph MainGraph = new();
        public static Button[] Buttons;
        public static Dictionary<string, Button> ButNameAlgorithmLauncherMap;
        public static TMP_Dropdown GraphType;

        private void Start()
        {
            Buttons = SceneButtons;
            GraphType = SceneGraphType;
            ButNameAlgorithmLauncherMap = new()
            {
                { "AbjMatrix", Buttons[AbjacencyMatrixButIndex] },
                { "DephtFirstTrav", Buttons[DepthFirstTravButIndex] },
                { "BreadthFirstTrav", Buttons[BreadthFirstTravButIndex] },
                { "ShortestPath", Buttons[ShortestPathButIndex] },
                { "TransportNetwork", Buttons[TransportNetworkButIndex] }
            };
        }
    }
}
