/*
 * @Author: delevin.ying 
 * @Date: 2020-05-20 15:40:14 
 * @Last Modified by: delevin.ying
 * @Last Modified time: 2020-05-22 19:48:27
 */
using UnityEngine;
using UnityEngine.EventSystems;
public class HexMapEditor : MonoBehaviour
{
    public Color[] colors;

    public HexGrid hexGrid;

    private Color activeColor;

    int activeElevation;

    void Awake()
    {
        SelectColor(0);
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            EditCell(hexGrid.GetCell(hit.point));
            //hexGrid.ColorCell(hit.point, activeColor);
            //hexGrid.TouchCell(hit.point);
        }
    }

    void EditCell(HexCell cell)
    {
        cell.color = activeColor;
        cell.Elevation = activeElevation;
        hexGrid.Refresh();
    }

    public void SelectColor(int index)
    {
        activeColor = colors[index];
    }

    public void SetEvelation(float evelation)
    {
        activeElevation = (int)evelation;
        Debug.Log(activeElevation);
    }
}
