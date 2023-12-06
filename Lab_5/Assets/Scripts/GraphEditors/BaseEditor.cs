using Assets.Scripts.Controllers;
using Assets.Scripts.GraphComponents;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.VarsHolder;

public abstract class BaseEditor : MonoBehaviour
{
    public Button Button;

    protected virtual void OnEnable()
    {
        foreach (Button button in Buttons)
        {
            if (button != Button && button.interactable)
                button.interactable = false;
        }
        if (GraphType.interactable)
            GraphType.interactable = false;

        if (!MainGraph.Type.Equals("Обычный граф"))
            foreach (Line line in MainGraph.Lines)
                line.WeightIF.gameObject.SetActive(false);
    }

    protected virtual void OnDisable()
    {
        foreach (Button button in Buttons)
        {
            if (!button.interactable)
                button.interactable = true;
        }
        if (!GraphType.interactable)
            GraphType.interactable = true;

        switch (MainGraph.Type)
        {
            case "Обычный граф":
                TypeController.InteractSimpleGraph();
                break;
            case "Взвешенный граф":
                TypeController.InteractWeightedGraph();
                break;
            case "Транспортная сеть":
                TypeController.InteractTransportNetwork();
                break;
        }

        if (!MainGraph.Type.Equals("Обычный граф"))
        {
            foreach (Line line in MainGraph.Lines)
            {
                line.WeightIF.gameObject.SetActive(true);
                line.UpdateInputFieldPosition();
            }
        }
    }
}
