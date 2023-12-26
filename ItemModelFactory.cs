using R2API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;

namespace ItemLuck;

public static class ItemModelFactory
{
    private struct CubeVertices
    {
        public int backBottomLeftOffset;
        public int backBottomRightOffset;
        public int backTopLeftOffset;
        public int backTopRightOffset;
        
        public int frontBottomLeftOffset;
        public int frontBottomRightOffset;
        public int frontTopLeftOffset;
        public int frontTopRightOffset;
    }

    public static GameObject Create(Texture2D texture, string name, float depth, float scale)
    {
        Dictionary<Vector3, int> vertices = new Dictionary<Vector3, int>();
        List<Vector2> uvs = new List<Vector2>();

        float offset = scale / 2;
        float pixelWidth = 1f / texture.width;
        float pixelHeight = 1f / texture.height;

        List<int> triangles = new List<int>();

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                var color = texture.GetPixel(x, y);
                if (color.a > 0)
                {
                    var pos = AddVertices(x, y);

                    AddQuad(pos.frontBottomLeftOffset, pos.frontTopLeftOffset, pos.frontTopRightOffset, pos.frontBottomRightOffset);
                    AddQuad(pos.backBottomLeftOffset, pos.backBottomRightOffset, pos.backTopRightOffset, pos.backTopLeftOffset);

                    if (x == 0 || texture.GetPixel(x - 1, y).a == 0)
                    {
                        AddQuad(pos.backBottomLeftOffset, pos.backTopLeftOffset, pos.frontTopLeftOffset, pos.frontBottomLeftOffset);
                    }

                    if (x == texture.width - 1 || texture.GetPixel(x + 1, y).a == 0)
                    {
                        AddQuad(pos.frontBottomRightOffset, pos.frontTopRightOffset, pos.backTopRightOffset, pos.backBottomRightOffset);
                    }

                    if (y == 0 || texture.GetPixel(x, y - 1).a == 0)
                    {
                        AddQuad(pos.frontBottomRightOffset, pos.backBottomRightOffset, pos.backBottomLeftOffset, pos.frontBottomLeftOffset);
                    }

                    if (y == texture.height - 1 || texture.GetPixel(x, y + 1).a == 0)
                    {
                        AddQuad(pos.backTopLeftOffset, pos.backTopRightOffset, pos.frontTopRightOffset, pos.frontTopLeftOffset);
                    }
                }
            }
        }

        CubeVertices AddVertices(int x, int y)
        {
            var x1 = scale * x * pixelWidth - offset;
            var x2 = scale * (x + 1) * pixelWidth - offset;

            var y1 = scale * y * pixelHeight - offset;
            var y2 = scale * (y + 1) * pixelHeight - offset;


            var z1 = scale * depth;
            var z2 = -scale * depth;

            return new CubeVertices
            {
                backBottomLeftOffset = GetOrAddVertex(new Vector3(x1, y1, z1), x, y),
                backBottomRightOffset = GetOrAddVertex(new Vector3(x2, y1, z1), x + 1, y),
                backTopLeftOffset = GetOrAddVertex(new Vector3(x1, y2, z1), x, y + 1),
                backTopRightOffset = GetOrAddVertex(new Vector3(x2, y2, z1), x + 1, y + 1),

                frontBottomLeftOffset = GetOrAddVertex(new Vector3(x1, y1, z2), x, y),
                frontBottomRightOffset = GetOrAddVertex(new Vector3(x2, y1, z2), x + 1, y),
                frontTopLeftOffset = GetOrAddVertex(new Vector3(x1, y2, z2), x, y + 1),
                frontTopRightOffset = GetOrAddVertex(new Vector3(x2, y2, z2), x + 1, y + 1)
            };
        }

        int GetOrAddVertex(Vector3 vector, int x, int y)
        {
            if (vertices.TryGetValue(vector, out int index))
            {
                return index;
            }

            uvs.Add(new Vector2(x * pixelWidth, y * pixelHeight));

            int newIndex = vertices.Count;
            vertices[vector] = newIndex;
            return newIndex;
        }

        void AddQuad(int index1, int index2, int index3, int index4)
        {
            AddTriangle(index1, index2, index3);
            AddTriangle(index3, index4, index1);
        }

        void AddTriangle(int index1, int index2, int index3)
        {
            triangles.Add(index1);
            triangles.Add(index2);
            triangles.Add(index3);
        }

        Mesh mesh = new Mesh();
        mesh.indexFormat = IndexFormat.UInt32;
        mesh.SetVertices(vertices.OrderBy(x => x.Value).Select(x => x.Key).ToList());
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);

        mesh.Optimize();

        GameObject itemModel = new GameObject();
        var meshFilter = itemModel.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        var meshRenderer = itemModel.AddComponent<MeshRenderer>();
        meshRenderer.material.mainTexture = texture;

        var prefab = PrefabAPI.InstantiateClone(itemModel, name, registerNetwork: false);

        return prefab;
    }
}
