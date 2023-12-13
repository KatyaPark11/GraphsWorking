using Assets.Scripts.Controllers;
using Assets.Scripts.GraphComponents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.AlgorithmManager;
using static Assets.Scripts.GraphCopyManager;
using static Assets.Scripts.VarsHolder;

namespace Assets.Scripts.ShortestPath
{
    public class ToPointIndexGetting : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    GameObject go = hit.collider.gameObject;
                    if (go.CompareTag("Point"))
                    {
                        ToPointIndex = MainGraph.GetPointIndex(go);
                        ShortestPathBegginer.SetActive(false);
                        UnsavableAlgActiveController.SetActive(true);
                        Buttons[SaveButIndex].interactable = true;
                        NextStepButton.onClick.RemoveAllListeners();
                        NextStepButton.onClick.AddListener(DoShortestPathStep);
                        CreateTextsOverPoints();
                        ShortestPathSteps = ShortestPathStepsGetting.GetShortestPathSteps(GraphCopy, FromPointIndex, ToPointIndex);
                        CurrentStepsCount = ShortestPathSteps.Count;
                        CurrentStepNum = 0;
                        gameObject.SetActive(false);
                    }
                }
            }
        }

        private void CreateTextsOverPoints()
        {
            foreach (Point point in GraphCopy.Points)
            {
                GameObject backgroundGO = new("Background");
                Image background = backgroundGO.AddComponent<Image>();
                background.GetComponent<RectTransform>().sizeDelta = new Vector2(15f, 15f);
                backgroundGO.transform.SetParent(point.PointObj.transform);
                background.transform.position = point.PointObj.transform.position + new Vector3(0, 20f, 0);

                GameObject textObject = new("TextOverObject");
                TextMeshProUGUI textMesh = textObject.AddComponent<TextMeshProUGUI>();
                textMesh.text = "∞";
                textMesh.alignment = TextAlignmentOptions.Center;
                textMesh.fontSize = 10;
                textMesh.color = Color.blue;

                textObject.transform.SetParent(backgroundGO.transform);
                textMesh.transform.position = backgroundGO.transform.position;
            }
        }
    }
}
