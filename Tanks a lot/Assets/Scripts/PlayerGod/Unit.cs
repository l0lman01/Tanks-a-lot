using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool isMoving = false;

    void Start()
    {
        UnitSelection.Instance.unitList.Add(this.gameObject);
    }
    void OnDestroy()
    {
        UnitSelection.Instance.unitList.Remove(this.gameObject);
    }
}
