using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class HexGraphic : Graphic {

    public float size = 50;

    protected override void OnPopulateMesh(VertexHelper vh) {
        vh.Clear();

        UIVertex vert = UIVertex.simpleVert;

        Vector3[] vertices = CalculateHexVertices(size);

        for (int i = 0; i < vertices.Length; i++) {
            vert.position = vertices[i];
            vert.color = color;
            vh.AddVert(vert);
        }

        vh.AddTriangle(5, 0, 1);
        vh.AddTriangle(5, 1, 2);
        vh.AddTriangle(5, 2, 3);
        vh.AddTriangle(5, 3, 4);
    }

    Vector3[] CalculateHexVertices(float r) {

        Vector3[] vertices = new Vector3[6];
        for (int i = 0; i < 6; i++) {
            float theta = (Mathf.PI / 2) + (i * Mathf.PI / 3f);
            float x = r * Mathf.Cos(theta);
            float y = r * Mathf.Sin(theta);
            vertices[i] = new Vector3(x, y);
        }
        return vertices;
    }
}