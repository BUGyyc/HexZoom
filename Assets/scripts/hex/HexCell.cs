/*
 * @Author: delevin.ying 
 * @Date: 2020-05-08 17:26:03 
 * @Last Modified by: delevin.ying
 * @Last Modified time: 2020-05-20 17:17:59
 */

using UnityEngine;
using Hex;
public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;

    public Color color;

    public RectTransform uiRect;

    [SerializeField]
    HexCell[] neighbors;

    int elevation;

    public int Elevation
    {
        get
        {
            return elevation;
        }
        set
        {
            elevation = value;
            Vector3 position = transform.localPosition;
            position.y = value * HexMetrics.elevationStep;
            transform.localPosition = position;

            Vector3 uiPosition = uiRect.localPosition;
            uiPosition.z = elevation * (-HexMetrics.elevationStep);
            uiRect.localPosition = uiPosition;
        }
    }

    public HexCell GetNeighbor(HexDirection direction)
    {
        Debug.LogWarning("direction:  " + (int)direction + "   " + direction);
        return neighbors[(int)direction];
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        //反向
        cell.neighbors[(int)direction.HexDirectionOpposite()] = this;
    }

    public HexEdgeType GetEdgeType(HexDirection direction)
    {
        return HexMetrics.GetEdgeType(elevation, GetNeighbor(direction).elevation);
    }

    public HexEdgeType GetEdgeType(HexCell other)
    {
        return HexMetrics.GetEdgeType(elevation, other.Elevation);
    }
}