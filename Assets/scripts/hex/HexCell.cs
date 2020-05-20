/*
 * @Author: delevin.ying 
 * @Date: 2020-05-08 17:26:03 
 * @Last Modified by: delevin.ying
 * @Last Modified time: 2020-05-20 15:58:04
 */

using UnityEngine;

public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;

    public Color color;

    [SerializeField]
    HexCell[] neighbors;

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        //反向
        cell.neighbors[(int)direction.HexDirectionOpposite()] = this;
    }
}