/*
 * @Author: delevin.ying 
 * @Date: 2020-05-09 14:44:30 
 * @Last Modified by: delevin.ying
 * @Last Modified time: 2020-05-20 17:00:20
 */
using UnityEngine;
using System.Collections.Generic;
using Hex;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    private Mesh mesh;
    private MeshCollider meshCollider;
    private List<Vector3> vertices;
    private List<int> triangles;
    private List<Color> colors;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        meshCollider = this.gameObject.AddComponent<MeshCollider>();
        mesh.name = "HexMesh";
        vertices = new List<Vector3>();
        colors = new List<Color>();
        triangles = new List<int>();
    }

    public void Triangulate(HexCell[] cells)
    {
        mesh.Clear();
        vertices.Clear();
        triangles.Clear();
        colors.Clear();
        for (int i = 0; i < cells.Length; i++)
        {
            triangulateCell(cells[i]);
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors = colors.ToArray();
        mesh.RecalculateNormals();
        meshCollider.sharedMesh = mesh;
    }

    private void triangulateCell(HexCell cell)
    {
        for (HexDirection hexDirection = HexDirection.NE; hexDirection <= HexDirection.NW; hexDirection++)
        {
            triangulateCell(hexDirection, cell);
        }
    }

    private void triangulateCell(HexDirection hexDirection, HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;
        addTriangle(
            center,
            center + HexMetrics.GetFirstCorner(hexDirection),
            center + HexMetrics.GetSecondCorner(hexDirection)
        );
        HexCell preNeighbor = cell.GetNeighbor(hexDirection.Previous()) ?? cell;
        HexCell neighbor = cell.GetNeighbor(hexDirection) ?? cell;
        HexCell nextNeighbor = cell.GetNeighbor(hexDirection.Next()) ?? cell;
        addTriangleColor(
            cell.color,
            (cell.color + preNeighbor.color + neighbor.color) / 3f,
            (cell.color + neighbor.color + nextNeighbor.color) / 3f
        );
    }

    private void addTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }

    private void addTriangleColor(Color c1, Color c2, Color c3)
    {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
    }
}