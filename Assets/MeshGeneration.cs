using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshGeneration : MonoBehaviour
{
    Mesh mesh;

    void Awake()
    {
        // Initialize the mesh and assign it to the MeshFilter
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = GenerateVertices();
        mesh.triangles = GenerateTriangles();
        mesh.RecalculateNormals();
    }

    Vector3[] GenerateVertices()
    {
        return new Vector3[]
        {
            // Ïåðåäíÿÿ ãðàíü
            new Vector3(-0.5f, -0.5f, -0.5f), // 0
            new Vector3(0.5f, -0.5f, -0.5f), // 1
            new Vector3(0.5f, 0.5f, -0.5f), // 2
            new Vector3(-0.5f, 0.5f, -0.5f), // 3

            // Çàäíÿÿ ãðàíü
            new Vector3(-0.5f, -0.5f, 0.5f), // 4
            new Vector3(0.5f, -0.5f, 0.5f), // 5
            new Vector3(0.5f, 0.5f, 0.5f), // 6
            new Vector3(-0.5f, 0.5f, 0.5f), // 7

            // Ëåâàÿ ãðàíü
            new Vector3(-0.5f, -0.5f, -0.5f), // 8
            new Vector3(-0.5f, 0.5f, -0.5f), // 9
            new Vector3(-0.5f, 0.5f, 0.5f), // 10
            new Vector3(-0.5f, -0.5f, 0.5f), // 11

            // Ïðàâàÿ ãðàíü
            new Vector3(0.5f, -0.5f, -0.5f), // 12
            new Vector3(0.5f, 0.5f, -0.5f), // 13
            new Vector3(0.5f, 0.5f, 0.5f), // 14
            new Vector3(0.5f, -0.5f, 0.5f), // 15

            // Íèæíÿÿ ãðàíü
            new Vector3(-0.5f, -0.5f, -0.5f), // 16
            new Vector3(0.5f, -0.5f, -0.5f), // 17
            new Vector3(0.5f, -0.5f, 0.5f), // 18
            new Vector3(-0.5f, -0.5f, 0.5f), // 19

            // Âåðõíÿÿ ãðàíü
            new Vector3(-0.5f, 0.5f, -0.5f), // 20
            new Vector3(0.5f, 0.5f, -0.5f), // 21
            new Vector3(0.5f, 0.5f, 0.5f), // 22
            new Vector3(-0.5f, 0.5f, 0.5f) // 23
        };
    }

    int[] GenerateTriangles()
    {
        return new int[]
        {
            // Ïåðåäíÿÿ ãðàíü
            0, 2, 1, 0, 3, 2,

            // Çàäíÿÿ ãðàíü
            4, 5, 6, 4, 6, 7,

            // Ëåâàÿ ãðàíü
            8, 10, 9, 8, 11, 10,

            // Ïðàâàÿ ãðàíü
            12, 13, 14, 12, 14, 15,

            // Íèæíÿÿ ãðàíü
            16, 17, 18, 16, 18, 19,

            // Âåðõíÿÿ ãðàíü
            20, 22, 21, 20, 23, 22
        };
    }

    public void CutBlock(float startOffset, float overlapWidth)
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        float leftBoundary = -overlapWidth / 2; // Левая граница перекрытия
        float rightBoundary = overlapWidth / 2; // Правая граница перекрытия

        for (int i = 0; i < vertices.Length; i++)
        {
            // Обрезаем вершины, выходящие за пределы границ
            if (vertices[i].x < leftBoundary)
            {
                vertices[i].x = leftBoundary;
            }
            else if (vertices[i].x > rightBoundary)
            {
                vertices[i].x = rightBoundary;
            }
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        Debug.Log($"Блок обрезан: левая граница = {leftBoundary}, правая граница = {rightBoundary}");
    }
}