using UnityEngine;
using System.Collections.Generic;

public class MarkerTool : MonoBehaviour
{
    public Camera m_camera;
    public BrushPool brushPool;

    public Material white;
    public Material blue;
    public Material red;

    private LineRenderer currentLineRenderer;
    private Vector2 lastPos;
    private List<GameObject> drawnObjects = new List<GameObject>();
    private bool activeState = true;

    private Material currentMaterial;

    private void Start()
    {
        currentMaterial = white;
    }

    private void Update()
    {
        Drawing();
        CheckUndo();
        CheckVisablility();
        CheckColor();
    }

    void Drawing()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CreateBrush();
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            PointToMousePos();
        }
        else
        {
            currentLineRenderer = null;
        }
    }

    void CreateBrush()
    {
        GameObject brushInstance = brushPool.GetBrush();
        brushInstance.GetComponent<LineRenderer>().material = currentMaterial;
        drawnObjects.Add(brushInstance);
        brushInstance.transform.parent = transform;
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        // because you gotta have 2 points to start a line renderer
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);

        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);
    }

    void AddAPoint(Vector2 pointPos)
    {
        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }

    void PointToMousePos()
    {
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        if (lastPos != mousePos)
        {
            AddAPoint(mousePos);
            lastPos = mousePos;
        }
    }

    void CheckColor()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentMaterial = white;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentMaterial = blue;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentMaterial = red;
        }
    }

    void CheckUndo()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Y)) //USD or European layout
        {
            UndoLastDraw();
        }
    }

    void CheckVisablility()
    {
        if (Input.GetKeyDown(KeyCode.V)) //USD or European layout
        {
            toggleLineVisablity();
        }
    }

    void UndoLastDraw()
    {
        if (drawnObjects.Count > 0)
        {
            GameObject lastDrawnObject = drawnObjects[drawnObjects.Count - 1];
            drawnObjects.RemoveAt(drawnObjects.Count - 1);
            brushPool.ReturnBrush(lastDrawnObject);
        }
    }

    public void toggleLineVisablity()
    {
        activeState = !activeState;
        foreach (GameObject lineObj in drawnObjects)
        {
            lineObj.SetActive(activeState);
        }
    }
}
