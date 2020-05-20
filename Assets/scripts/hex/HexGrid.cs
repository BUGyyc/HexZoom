/*
 * @Author: delevin.ying 
 * @Date: 2020-05-08 17:26:35 
 * @Last Modified by: delevin.ying
 * @Last Modified time: 2020-05-20 16:56:39
 */
using UnityEngine;
using UnityEngine.UI;
using Hex;
public class HexGrid : MonoBehaviour
{
    public int width = 6;
    public int height = 6;
    public HexCell cellPrefab;
    public Text hexCellLabel;
    public Color defaultColor = Color.white;
    public Color touchedColor = Color.black;


    /// <summary>
    ///
    /// </summary>
    private HexMesh hexMesh;
    private HexCell[] cells;
    private Canvas gridCanvas;
    private void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();
        cells = new HexCell[width * height];
        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    private void Start()
    {
        hexMesh.Triangulate(cells);
    }

    private void CreateCell(int x, int z, int i)
    {
        float l_x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        float l_z = z * (HexMetrics.outerRadius * 1.5f);
        Vector3 position = new Vector3(l_x, 0f, l_z);
        HexCell cell = Instantiate<HexCell>(cellPrefab);
        cells[i] = cell;
        cell.color = defaultColor;
        cell.transform.parent = this.gameObject.transform;
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

        if (x > 0)
        {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }
        if (z > 0)
        {
            if ((z & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - width]);
                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - width]);
                if (x < width - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
                }
            }
        }

        Text label = Instantiate<Text>(hexCellLabel);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition =
            new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
    }

    private void Update()
    {
        // if (Input.GetMouseButton(0))
        // {
        //     inputHandler();
        // }
    }

    private void inputHandler()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            touchCell(hit.point);
        }
    }

    private void touchCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        Debug.Log("touched at " + coordinates.ToString());
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        HexCell cell = cells[index];
        cell.color = touchedColor;
        hexMesh.Triangulate(cells);
    }

    public void ColorCell(Vector3 position, Color color)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        Debug.Log("touched at " + coordinates.ToString());
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        HexCell cell = cells[index];
        cell.color = color;
        hexMesh.Triangulate(cells);
    }
}

public enum HexDirection
{
    NE,
    E,
    SE,
    SW,
    W,
    NW
}

public static class HexDirectionExtensions
{
    public static HexDirection HexDirectionOpposite(this HexDirection hexDirection)
    {
        return (int)hexDirection < 3 ? (hexDirection + 3) : (hexDirection - 3);
    }

    public static HexDirection Previous(this HexDirection hexDirection)
    {
        return (hexDirection == HexDirection.NE) ? HexDirection.NW : hexDirection - 1;
    }

    public static HexDirection Next(this HexDirection hexDirection)
    {
        return (hexDirection == HexDirection.NW) ? HexDirection.NE : hexDirection - 1;
    }
}