using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDrag : MonoBehaviour
{
    Camera myCam;

    [SerializeField]
    RectTransform boxVisual;

    Rect selectionBox;

    Vector2 startPosition;
    Vector2 endPosition;

    Vector2 startPosition2D;
    Vector2 endPosition2D;

    // Start is called before the first frame update
    void Start()
    {
        myCam = Camera.main;
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
    }

    // Update is called once per frame
    void Update()
    {
        //Quand on clicque
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
            startPosition2D = myCam.ScreenToWorldPoint(Input.mousePosition);
            selectionBox = new Rect();
        }

        //Quand on maintient
        if (Input.GetMouseButton(0))
        {
            endPosition = Input.mousePosition;
            endPosition2D = myCam.ScreenToWorldPoint(Input.mousePosition);
            DrawVisual();
            DrawSelection();
        }
        //Quand on relache
        if (Input.GetMouseButtonUp(0))
        {
            SelectUnits();
            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            startPosition2D = Vector2.zero;
            endPosition2D = Vector2.zero;
            DrawVisual();
        }
    }
    
    void DrawVisual()
    {
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;

        Vector2 boxCenter = (boxStart + boxEnd) / 2;
        boxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        boxVisual.sizeDelta = boxSize;
    }

    void DrawSelection()
    {
        if(Input.mousePosition.x < startPosition.x)
        {
            selectionBox.xMin = startPosition.x;
            selectionBox.xMax = Input.mousePosition.x;
        }
        else
        {
            selectionBox.xMin = Input.mousePosition.x;
            selectionBox.xMax = startPosition.x;
        }
        if(Input.mousePosition.x < startPosition.y)
        {
            selectionBox.yMin = startPosition.y;
            selectionBox.yMax = Input.mousePosition.y;
        }
        else
        {
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = startPosition.y;
        }
    }

    void SelectUnits()
    {
        Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition2D, endPosition2D);

        foreach(Collider2D collider2D in collider2DArray)
        {
            Unit unit = collider2D.GetComponent<Unit>();
            if (unit != null)
            {
                UnitSelection.Instance.DragSelect(unit.gameObject);
            }
        }
    }
}
