using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelTile : MonoBehaviour
{
    public RotationType Rotation;
    public enum RotationType
    {
        OnlyRotation,
        TwoRotations,
        FourRotations
    }

    public float voxelSize = 0.1f;
    public int tileSize = 16;
    [Range(0, 100)] public int Weight = 50;

    [HideInInspector] public byte[] ColorsRight;
    [HideInInspector] public byte[] ColorsForward;
    [HideInInspector] public byte[] ColorsLeft;
    [HideInInspector] public byte[] ColorsBack;

    public void CalculateSidesColors()
    {
        ColorsRight = new byte[tileSize * tileSize];
        ColorsForward = new byte[tileSize * tileSize];
        ColorsLeft = new byte[tileSize * tileSize];
        ColorsBack = new byte[tileSize * tileSize];

        for (int y = 0; y < tileSize; y++) 
        {
            for (int i = 0; i < tileSize; i++)
            {
                ColorsRight[y * tileSize + i] = GetVoxelColor(y, i, Vector3.right);
                ColorsForward[y * tileSize + i] = GetVoxelColor(y, i, Vector3.forward);
                ColorsLeft[y * tileSize + i] = GetVoxelColor(y, i, Vector3.left);
                ColorsBack[y * tileSize + i] = GetVoxelColor(y, i, Vector3.back);
            }
        }
    }

    public void Rotate90()
    {
        transform.Rotate(0, 90, 0);

        byte[] colorsRightNew = new byte[tileSize * tileSize];
        byte[] colorsForwardNew = new byte[tileSize * tileSize];
        byte[] colorsLeftNew = new byte[tileSize * tileSize];
        byte[] colorsBackNew = new byte[tileSize * tileSize];

        for (int layer = 0; layer < tileSize; layer++)
        {
            for (int offset = 0; offset < tileSize; offset++)
            {
                colorsRightNew[layer * tileSize + offset] = ColorsForward[layer * tileSize + tileSize - offset - 1];
                colorsForwardNew[layer * tileSize + offset] = ColorsLeft[layer * tileSize + offset];
                colorsLeftNew[layer * tileSize + offset] = ColorsBack[layer * tileSize + tileSize - offset - 1];
                colorsBackNew[layer * tileSize + offset] = ColorsRight[layer * tileSize + offset];
            }
        }

        ColorsRight = colorsRightNew;
        ColorsForward = colorsForwardNew;
        ColorsLeft = colorsLeftNew;
        ColorsBack = colorsBackNew;
    }

    private byte GetVoxelColor(int verticalLayer, int horizontalOffset, Vector3 direction) 
    { 
        var meshCollider = GetComponentInChildren<MeshCollider>();
        float vox = voxelSize;
        float half = voxelSize/ 2;

        Vector3 rayStart;
        if (direction == Vector3.right)
        {
            rayStart = meshCollider.bounds.min +
                               new Vector3(-half, 0, half + horizontalOffset * vox);
        }
        else if (direction == Vector3.forward)
        {
            rayStart = meshCollider.bounds.min +
                               new Vector3(half + horizontalOffset * vox, 0, -half);
        }
        else if (direction == Vector3.left)
        {
            rayStart = meshCollider.bounds.min +
                               new Vector3(half + vox * tileSize, 0, -half - (-tileSize - horizontalOffset - 1) * vox - vox * tileSize);
        }
        else if (direction == Vector3.back)
        {
            rayStart = meshCollider.bounds.min +
                               new Vector3(-half - (-tileSize - horizontalOffset - 1) * vox - vox * tileSize, 0, half + vox * tileSize);
        }
        else
        {
            throw new ArgumentException("Wrong direction value", nameof(direction));
        }

        rayStart.y = meshCollider.bounds.min.y + half + verticalLayer * vox;
        //Debug.DrawRay(rayStart, direction * 0.1f, Color.red, 5);

        if (Physics.Raycast(new Ray(rayStart, direction), out RaycastHit hit, vox)) 
        {
            Mesh mesh = meshCollider.sharedMesh;

            int hitTriangleVertex = mesh.triangles[hit.triangleIndex * 3];
            byte colorIndex = (byte)(mesh.uv[hitTriangleVertex].x * 256);
            return colorIndex;
        }

        return 0;
    }
}
