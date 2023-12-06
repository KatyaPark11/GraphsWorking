using Assets.Scripts.GraphComponents;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class AreaController
    {
        public bool IsUnreachableArea()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject go = hit.collider.gameObject;
                return go.CompareTag("Unreachable");
            }
            return false;
        }

        public void SetCurAreaColor(Line line)
        {
            if (IsUnreachableArea())
                line?.SetLineColor(Color.gray);
            else
                line?.SetLineColor(Color.red);
        }

        public void SetCurAreaColor(Point point)
        {
            if (IsUnreachableArea())
                foreach (Line line in point.LinkedLines)
                    line.SetLineColor(Color.gray);
            else
                foreach (Line line in point.LinkedLines)
                    line.SetLineColor(Color.red);
        }
    }
}
