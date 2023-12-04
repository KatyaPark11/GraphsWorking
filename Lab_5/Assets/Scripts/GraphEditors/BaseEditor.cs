using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseEditor : MonoBehaviour
{
    public Button[] OtherButtons;
    public TMP_Dropdown GraphType;

    protected virtual void OnEnable()
    {
        foreach (Button button in OtherButtons)
        {
            if (button.interactable)
                button.interactable = false;
        }
        if (GraphType.interactable)
            GraphType.interactable = false;
    }

    protected virtual void OnDisable()
    {
        foreach (Button button in OtherButtons)
        {
            if (!button.interactable)
                button.interactable = true;
        }
        if (!GraphType.interactable)
            GraphType.interactable = true;
    }
}
