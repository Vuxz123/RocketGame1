using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace com.ethnicthv
{
    public class Cell
    {
        public bool Up;
        public bool Down;
        public bool Left;
        public bool Right;

        public bool Render;
        
        public Cell(bool up = false, bool down = false, bool left = false, bool right = false)
        {
            Render = true;
            Up = up;
            Down = down;
            Left = left;
            Right = right;
        }

        [BurstCompile]
        public static (float3[], int[], float2[]) CalculateMesh(bool up, bool down, bool left, bool right)
        {
            float3[] vertices = new float3[16];

            int triangleCount = 4 + (up ? 2 : 0) + (down ? 2 : 0) + (left ? 2 : 0) + (right ? 2 : 0);

            int[] triangles = new int[triangleCount * 3];

            vertices[0] = new float3(-0.5f, -0.5f, -0.5f);
            vertices[1] = new float3(0.5f, -0.5f, -0.5f);
            vertices[2] = new float3(0.5f, 0.5f, -0.5f);
            vertices[3] = new float3(-0.5f, 0.5f, -0.5f);
            vertices[4] = new float3(-0.5f, -0.5f, 0.5f);
            vertices[5] = new float3(0.5f, -0.5f, 0.5f);
            vertices[6] = new float3(0.5f, 0.5f, 0.5f);
            vertices[7] = new float3(-0.5f, 0.5f, 0.5f);
            
            vertices[8] = new float3(-0.5f, -0.5f, -0.5f);
            vertices[9] = new float3(0.5f, -0.5f, -0.5f);
            vertices[10] = new float3(0.5f, 0.5f, -0.5f);
            vertices[11] = new float3(-0.5f, 0.5f, -0.5f);
            vertices[12] = new float3(-0.5f, -0.5f, 0.5f);
            vertices[13] = new float3(0.5f, -0.5f, 0.5f);
            vertices[14] = new float3(0.5f, 0.5f, 0.5f);
            vertices[15] = new float3(-0.5f, 0.5f, 0.5f);
            
            float2[] uvs = new float2[vertices.Length];
            
            for (int i = 0; i < vertices.Length; i++)
            {
                if(i<8) uvs[i] = new float2(vertices[i].x + 0.5f, vertices[i].y + 0.5f);
                else uvs[i] = new float2(vertices[i].z + 0.5f, vertices[i].y + 0.5f);
            }

            // Bottom
            triangles[0] = 4;
            triangles[1] = 5;
            triangles[2] = 0;

            triangles[3] = 5;
            triangles[4] = 1;
            triangles[5] = 0;

            var currentIndex = 5;

            if (down)
            {
                triangles[++currentIndex] = 1;
                triangles[++currentIndex] = 2;
                triangles[++currentIndex] = 0;

                triangles[++currentIndex] = 0;
                triangles[++currentIndex] = 2;
                triangles[++currentIndex] = 3;
            }
            
            if (up)
            {
                triangles[++currentIndex] = 7;
                triangles[++currentIndex] = 6;
                triangles[++currentIndex] = 4;

                triangles[++currentIndex] = 6;
                triangles[++currentIndex] = 5;
                triangles[++currentIndex] = 4;
            }
            
            if (left)
            {
                triangles[++currentIndex] = 8;
                triangles[++currentIndex] = 11;
                triangles[++currentIndex] = 15;

                triangles[++currentIndex] = 8;
                triangles[++currentIndex] = 15;
                triangles[++currentIndex] = 12;
            }
            
            if (right)
            {
                triangles[++currentIndex] = 9;
                triangles[++currentIndex] = 13;
                triangles[++currentIndex] = 14;

                triangles[++currentIndex] = 9;
                triangles[++currentIndex] = 14;
                triangles[++currentIndex] = 10;
            }

            return (vertices, triangles, uvs);
        }
    }
}