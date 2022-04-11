using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderMovement : MonoBehaviour
{
    public Vector3 m_CoordOrderMovement;
    public float m_Speed;
    private Vector3 targetPosition;
    private Vector3 LastPosition;

    Coroutine _coroutine;

    // Start is called before the first frame update
    void Start()
    {
        m_CoordOrderMovement = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        //Vector3 newDirection = Vector3.RotateTowards(m_Tank.transform.forward, m_CoordOrderMovement, step, 0.0f);
        if (Input.GetMouseButtonDown(1))
        {
            m_CoordOrderMovement = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            foreach (var unit in UnitSelection.Instance.unitsSelected)
            {
                
                MoveCoroutine(unit, m_CoordOrderMovement);
            }
        }

        //m_Tank.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    void MoveCoroutine(GameObject unit, Vector3 targetPos)
    {
        if (!unit.GetComponent<Unit>().isMoving)
        {
            StartCoroutine(MoveTo(unit, targetPos));
            LastPosition = targetPos;
            print(LastPosition + " " + targetPos);
        }
        else
        {
            StopCoroutine(MoveTo(unit, LastPosition));
            unit.GetComponent<Unit>().isMoving = false;
            StartCoroutine(MoveTo(unit,targetPos));
        }
    }

    IEnumerator MoveTo(GameObject unit, Vector3 targetPos)
    {
        unit.GetComponent<Unit>().isMoving = true;
        while (unit.GetComponent<Unit>().isMoving == true)
        {
            if (unit.transform.position.x != targetPos.x && unit.transform.position.y != targetPos.y)
            {
                float step = m_Speed * Time.deltaTime;

                unit.transform.position = Vector2.MoveTowards(unit.transform.position, targetPos, step);
            }
            else
            {
                unit.GetComponent<Unit>().isMoving = false;
            }
            yield return null;
        }
        

    }
    
}
