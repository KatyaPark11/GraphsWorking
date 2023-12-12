using UnityEngine;

public class AlgActiveController : MonoBehaviour
{
    public GameObject[] EnabledObjs;
    public GameObject[] DisabledObjs;

    private void OnEnable()
    {
        foreach (var obj in DisabledObjs)
            obj.SetActive(false);
        foreach (var obj in EnabledObjs)
            obj.SetActive(true);
    }

    private void OnDisable()
    {
        foreach (var obj in EnabledObjs)
            obj.SetActive(false);
        foreach (var obj in DisabledObjs)
            obj.SetActive(true);
    }
}