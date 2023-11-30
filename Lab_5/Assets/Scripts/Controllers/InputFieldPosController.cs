using TMPro;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class InputFieldPosController : MonoBehaviour
    {
        public LineRenderer LineRenderer;
        public TMP_InputField InputField;
        
        private void Update()
        {
            UpdateInputFieldPosition();
        }

        private void UpdateInputFieldPosition()
        {
            Vector3 position1 = LineRenderer.GetPosition(0);
            Vector3 position2 = LineRenderer.GetPosition(1);

            Vector3 middlePosition = Vector3.Lerp(position1, position2, 0.75f);
            InputField.transform.position = middlePosition;
        }
    }
}