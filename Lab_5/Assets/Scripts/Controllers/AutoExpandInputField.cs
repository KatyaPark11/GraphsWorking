using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoExpandInputField : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public TMP_InputField inputField;
    public RectTransform rectTransform;

    private void Start()
    {
        inputField.onValueChanged.AddListener(UpdateSize);
    }

    private void UpdateSize(string text)
    {
        if (textMeshPro != null)
        {
            float width = textMeshPro.preferredWidth + 30f;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }
    }
}