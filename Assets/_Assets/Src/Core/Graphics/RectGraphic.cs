using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class RectGraphic : Graphic {

    public float lineWidth = 2;
    public float margin = 0;

    protected override void OnPopulateMesh(VertexHelper vh) {
        lineWidth = Mathf.Max(0, lineWidth);

        Vector2 corner1 = Vector2.zero;
        Vector2 corner2 = Vector2.zero;

        corner1.x = 0f;
        corner1.y = 0f;
        corner2.x = 1f;
        corner2.y = 1f;

        corner1.x -= rectTransform.pivot.x;
        corner1.y -= rectTransform.pivot.y;
        corner2.x -= rectTransform.pivot.x;
        corner2.y -= rectTransform.pivot.y;

        corner1.x *= rectTransform.rect.width;
        corner1.y *= rectTransform.rect.height;
        corner2.x *= rectTransform.rect.width;
        corner2.y *= rectTransform.rect.height;

        var bottomLeft = corner1 + new Vector2(-margin, -margin);
        var bottomRight = new Vector2(corner2.x, corner1.y) + new Vector2(margin, -margin);
        var topRight = corner2 + new Vector2(margin, margin);
        var topLeft = new Vector2(corner1.x, corner2.y) + new Vector2(-margin, margin);

        vh.Clear();

        UIVertex vert = UIVertex.simpleVert;

        int vertId = 0;

        AddSquare(vh, bottomLeft, topLeft + lineWidth * Vector2.right, ref vertId);
        AddSquare(vh, topLeft + lineWidth * Vector2.down, topRight, ref vertId);
        AddSquare(vh, bottomRight + lineWidth * Vector2.left, topRight, ref vertId);
        AddSquare(vh, bottomLeft, bottomRight + lineWidth * Vector2.up, ref vertId);
    }

    void AddSquare(VertexHelper vh, Vector2 bottomLeft, Vector2 topRight, ref int vertId) {
        var bottomRight = new Vector2(topRight.x, bottomLeft.y);
        var topLeft = new Vector2(bottomLeft.x, topRight.y);
        
        UIVertex vert = UIVertex.simpleVert;
        
        // 0
        vert.position = bottomLeft;
        vert.color = color;
        vh.AddVert(vert);

        // 1
        vert.position = topLeft;
        vert.color = color;
        vh.AddVert(vert);
        
        // 2
        vert.position = topRight;
        vert.color = color;
        vh.AddVert(vert);

        // 3
        vert.position = bottomRight;
        vert.color = color;
        vh.AddVert(vert);

        vh.AddTriangle(vertId, vertId + 1, vertId + 2);
        vh.AddTriangle(vertId, vertId + 2, vertId + 3);

        vertId += 4;
    }
}