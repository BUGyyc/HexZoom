/*
 * @Author: delevin.ying 
 * @Date: 2020-05-09 14:44:30 
 * @Last Modified by: delevin.ying
 * @Last Modified time: 2020-05-20 17:51:36
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
            TriangulateCell(cells[i]);
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors = colors.ToArray();
        mesh.RecalculateNormals();
        meshCollider.sharedMesh = mesh;
    }

    private void TriangulateCell(HexCell cell)
    {
        for (HexDirection hexDirection = HexDirection.NE; hexDirection <= HexDirection.NW; hexDirection++)
        {
            TriangulateCell(hexDirection, cell);
        }
    }

    private void TriangulateCell(HexDirection hexDirection, HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;
        Vector3 v1 = center + HexMetrics.GetFirstSolidCorner(hexDirection);
        Vector3 v2 = center + HexMetrics.GetSecondSolidCorner(hexDirection);
        AddTriangle(center, v1, v2);
        AddTriangleColor(cell.color);

        Vector3 v3 = center + HexMetrics.GetFirstCorner(hexDirection);
        Vector3 v4 = center + HexMetrics.GetSecondCorner(hexDirection);

        AddQuad(v1, v2, v3, v4);

        HexCell preNeighbor = cell.GetNeighbor(hexDirection.Previous()) ?? cell;
        HexCell neighbor = cell.GetNeighbor(hexDirection) ?? cell;
        HexCell nextNeighbor = cell.GetNeighbor(hexDirection.Next()) ?? cell;

        AddQuadColor(
            cell.color,
            cell.color,
            (cell.color + neighbor.color + preNeighbor.color) / 3f,
            (cell.color + neighbor.color + nextNeighbor.color) / 3f
        );
    }

    private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }

    private void AddTriangleColor(Color c1)
    {
        colors.Add(c1);
        // colors.Add(c2);
        // colors.Add(c3);
    }

    /// <summary>
    /// 创建相邻梯形
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="v3"></param>
    /// <param name="v4"></param>
    private void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        vertices.Add(v4);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 3);
    }

    private void AddQuadColor(Color c1, Color c2, Color c3, Color c4)
    {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
        colors.Add(c4);
    }
}