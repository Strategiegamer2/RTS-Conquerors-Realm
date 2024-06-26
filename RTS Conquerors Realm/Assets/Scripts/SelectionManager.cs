using UnityEngine;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour
{
    public Color selectedColor = Color.red; // Color to indicate selected units
    public Color normalColor = Color.white; // Color to indicate normal units
    private Vector2 startPos; // Start position of the selection box
    private List<GameObject> selectedUnits = new List<GameObject>(); // List of currently selected units
    private Camera mainCamera; // Reference to the main camera

    private bool isSelecting = false; // Flag to check if selection is ongoing
    private float minBoxSize = 10f; // Minimum size of the selection box to be shown

    private Dictionary<int, List<GameObject>> unitGroups = new Dictionary<int, List<GameObject>>(); // Dictionary to store unit groups

    void Start()
    {
        mainCamera = Camera.main; // Get the main camera
    }

    void Update()
    {
        HandleInput(); // Handle mouse input each frame
        HandleGroupSelection(); // Handle group selection input each frame
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            // Create a rect from both start and current mouse positions
            var rect = GetScreenRect(startPos, Input.mousePosition);
            if (rect.width > minBoxSize && rect.height > minBoxSize)
            {
                DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
                DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
            }
        }
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition; // Record the start position
            isSelecting = true; // Enable selection

            if (!(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                DeselectUnits(); // Clear previous selection if not holding Control
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isSelecting)
            {
                var rect = GetScreenRect(startPos, Input.mousePosition);
                if (rect.width > minBoxSize && rect.height > minBoxSize)
                {
                    SelectUnits(); // Select units within the selection box
                }
                else
                {
                    SelectSingleUnit(); // Select a single unit if not dragging
                }
                isSelecting = false; // Disable selection
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            HandleRightClick(); // Handle right-click input
        }
    }

    void HandleRightClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject target = hit.collider.gameObject;
            if (target.CompareTag("Wood") || target.CompareTag("Crystal"))
            {
                foreach (GameObject unit in selectedUnits)
                {
                    if (unit.GetComponent<Worker>() != null)
                    {
                        unit.GetComponent<Worker>().AssignResource(target);
                    }
                }
            }
            else if (target.CompareTag("Base"))
            {
                foreach (GameObject unit in selectedUnits)
                {
                    if (unit.GetComponent<Worker>() != null)
                    {
                        unit.GetComponent<Worker>().DropOffEarly();
                    }
                }
            }
        }
    }

    void HandleGroupSelection()
    {
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    SetGroup(i);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    SelectGroup(i);
                }
            }
        }
    }

    void SetGroup(int groupNumber)
    {
        unitGroups[groupNumber] = new List<GameObject>(selectedUnits);
    }

    void SelectGroup(int groupNumber)
    {
        if (unitGroups.ContainsKey(groupNumber))
        {
            DeselectUnits();
            selectedUnits = new List<GameObject>(unitGroups[groupNumber]);

            foreach (GameObject unit in selectedUnits)
            {
                if (unit != null)
                {
                    unit.GetComponent<Renderer>().material.color = selectedColor;
                }
            }
        }
    }

    void SelectUnits()
    {
        Rect selectionRect = GetScreenRect(startPos, Input.mousePosition);

        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject unit in units)
        {
            if (unit == null) continue; // Add null check
            Vector3 screenPos = mainCamera.WorldToScreenPoint(unit.transform.position);
            if (selectionRect.Contains(screenPos, true))
            {
                Ray ray = mainCamera.ScreenPointToRay(screenPos);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == unit)
                    {
                        AddUnitToSelection(unit);
                    }
                }
            }
        }
    }

    void SelectSingleUnit()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject unit = hit.collider.gameObject;
            if (unit.CompareTag("Unit"))
            {
                AddUnitToSelection(unit);
            }
        }
    }

    void AddUnitToSelection(GameObject unit)
    {
        if (!selectedUnits.Contains(unit))
        {
            selectedUnits.Add(unit);
            unit.GetComponent<Renderer>().material.color = selectedColor; // Change to selected color
        }
    }

    void DeselectUnits()
    {
        for (int i = selectedUnits.Count - 1; i >= 0; i--)
        {
            if (selectedUnits[i] != null)
            {
                selectedUnits[i].GetComponent<Renderer>().material.color = normalColor; // Change back to normal color
            }
        }
        selectedUnits.Clear();
    }

    void MoveSelectedUnits()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Ground"))
        {
            Vector3 targetPosition = hit.point;
            ArrangeUnitsInFormation(targetPosition);
        }
    }

    void ArrangeUnitsInFormation(Vector3 targetPosition)
    {
        int unitCount = selectedUnits.Count;
        if (unitCount == 0) return;

        int rows = Mathf.CeilToInt(Mathf.Sqrt(unitCount));
        float spacing = 2f; // Adjust spacing as needed

        for (int i = 0; i < unitCount; i++)
        {
            if (selectedUnits[i] == null) continue; // Add null check

            int row = i / rows;
            int column = i % rows;
            Vector3 offset = new Vector3(column * spacing, 0, row * spacing);
            Vector3 formationPosition = targetPosition + offset;

            selectedUnits[i].GetComponent<UnitMovement>().MoveTo(formationPosition);
        }
    }

    // Utility functions for drawing the selection rectangle
    Rect GetScreenRect(Vector2 screenPosition1, Vector2 screenPosition2)
    {
        // Move origin from bottom left to top left
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        // Create rectangle
        return new Rect(
            Mathf.Min(screenPosition1.x, screenPosition2.x),
            Mathf.Min(screenPosition1.y, screenPosition2.y),
            Mathf.Abs(screenPosition1.x - screenPosition2.x),
            Mathf.Abs(screenPosition1.y - screenPosition2.y)
        );
    }

    void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, Texture2D.whiteTexture);
        GUI.color = Color.white;
    }

    void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        // Top
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Left
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right
        DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        // Bottom
        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }
}