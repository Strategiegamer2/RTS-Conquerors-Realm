using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour
{
    public RectTransform selectionBox;
    public Color selectedColor = Color.red;
    public Color normalColor = Color.white;
    private Vector2 startPos;
    private Vector2 endPos;
    private List<GameObject> selectedUnits = new List<GameObject>();
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            selectionBox.gameObject.SetActive(false); // Hide the selection box initially
        }

        if (Input.GetMouseButton(0))
        {
            endPos = Input.mousePosition;
            if (Vector2.Distance(startPos, endPos) > 10) // Adjust the threshold as needed
            {
                selectionBox.gameObject.SetActive(true);
                UpdateSelectionBox();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (selectionBox.gameObject.activeInHierarchy)
            {
                SelectUnits();
                selectionBox.gameObject.SetActive(false);
            }
            else
            {
                SelectOrDeselectSingleUnit(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            MoveSelectedUnits();
        }
    }

    void UpdateSelectionBox()
    {
        Vector2 boxStart = startPos;
        Vector2 boxEnd = endPos;

        Vector2 boxCenter = (boxStart + boxEnd) / 2;

        // Convert screen point to local point for the canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(selectionBox.parent as RectTransform, boxCenter, null, out Vector2 localCenter);
        selectionBox.localPosition = localCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));
        selectionBox.sizeDelta = boxSize;
    }

    void SelectUnits()
    {
        if (!(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            DeselectUnits();  // Clear previous selection if not holding Control
        }

        Rect selectionRect = new Rect(Mathf.Min(startPos.x, endPos.x), Mathf.Min(startPos.y, endPos.y),
                                      Mathf.Abs(startPos.x - endPos.x), Mathf.Abs(startPos.y - endPos.y));

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
                        selectedUnits.Add(unit);
                        unit.GetComponent<Renderer>().material.color = selectedColor;  // Change to selected color
                    }
                }
            }
        }
    }

    void SelectOrDeselectSingleUnit(bool multiSelect)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Unit"))
            {
                if (!multiSelect)
                {
                    DeselectUnits();  // Clear previous selection if not multi-selecting
                }
                GameObject selectedUnit = hit.collider.gameObject;
                if (selectedUnits.Contains(selectedUnit))
                {
                    selectedUnits.Remove(selectedUnit);
                    if (selectedUnit != null)
                    {
                        selectedUnit.GetComponent<Renderer>().material.color = normalColor;  // Change back to normal color
                    }
                }
                else
                {
                    selectedUnits.Add(selectedUnit);
                    selectedUnit.GetComponent<Renderer>().material.color = selectedColor;  // Change to selected color
                }
            }
        }
        else
        {
            if (!multiSelect)
            {
                DeselectUnits();  // Deselect all units if clicking on an empty space
            }
        }
    }

    void DeselectUnits()
    {
        for (int i = selectedUnits.Count - 1; i >= 0; i--)
        {
            if (selectedUnits[i] != null)
            {
                selectedUnits[i].GetComponent<Renderer>().material.color = normalColor;  // Change back to normal color
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
}
