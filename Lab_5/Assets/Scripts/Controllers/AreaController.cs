using System;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class AreaController
    {
        public static bool IsUnreachableArea()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject go = hit.collider.gameObject;
                return go.CompareTag("Unreachable");
            }
            return false;
        }

        public static bool CompareColor(Color a, Color b)
        {
            const float accdelta = 0.01f;
            bool result = false;
            if (Math.Abs(a.r - b.r) < accdelta)
                if (Math.Abs(a.g - b.g) < accdelta)
                    if (Math.Abs(a.b - b.b) < accdelta) result = true;

            return result;
        }
    }
}
