/*
 * @Author: delevin.ying 
 * @Date: 2020-05-20 15:40:14 
 * @Last Modified by:   delevin.ying 
 * @Last Modified time: 2020-05-20 15:40:14 
 */
using UnityEngine;
using UnityEngine.EventSystems;
public class HexMapEditor : MonoBehaviour
{
    public Color[] colors;
    public HexGrid hexGrid;
    private Color activeColor;
    private int activeElevation;

    public void SelectColor(int index)
    {
        activeColor = colors[index];
    }

    public void SelectElevation(float elevation)
    {
        activeElevation = (int)elevation;
    }

    private void Awake()
    {
        SelectColor(0);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            HandleInput();
        }
    }

    public void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            EditHexCell(hexGrid.GetCell(hit.point));
        }
    }

    private void EditHexCell(HexCell cell)
    {
        cell.color = activeColor;
        cell.Elevation = activeElevation;
        hexGrid.Refresh();
    }
}
