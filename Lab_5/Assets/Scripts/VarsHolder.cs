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
        public GameObject SceneShortestPathBegginer;
        public GameObject SceneTransportNetworkBegginer;
        public GameObject SceneSavableAlgActiveController;
        public GameObject SceneUnsavableAlgActiveController;
        public int AbjacencyMatrixButIndex;
        public int DepthFirstTravButIndex;
        public int BreadthFirstTravButIndex;
        public int ShortestPathButIndex;
        public int TransportNetworkButIndex;
        public int SceneSaveButIndex;

        public static Graph MainGraph = new();
        public static Button[] Buttons;
        public static Dictionary<string, Button> ButNameAlgorithmLauncherMap;
        public static TMP_Dropdown GraphType;
        public static GameObject ShortestPathBegginer;
        public static GameObject TransportNetworkBegginer;
        public static GameObject SavableAlgActiveController;
        public static GameObject UnsavableAlgActiveController;
        public static int SaveButIndex;
        public static int FromPointIndex;
        public static int ToPointIndex;
        public static int SourcePointIndex;
        public static int SinkPointIndex;

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
            SaveButIndex = SceneSaveButIndex;
            ShortestPathBegginer = SceneShortestPathBegginer;
            TransportNetworkBegginer = SceneTransportNetworkBegginer;
            SavableAlgActiveController = SceneSavableAlgActiveController;
            UnsavableAlgActiveController = SceneUnsavableAlgActiveController;
        }
    }
}
