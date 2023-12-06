using UnityEngine.UI;
using static Assets.Scripts.VarsHolder;

namespace Assets.Scripts.Controllers
{
    public class AdjacencyMatrixController : BaseEditor
    {
        protected override void OnEnable()
        {
            foreach (Button button in Buttons)
            {
                if (button.interactable)
                    button.interactable = false;
            }
            if (GraphType.interactable)
                GraphType.interactable = false;
        }
    }
}
