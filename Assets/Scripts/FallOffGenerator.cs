/*
 * Most of the code below inspired by "Sebastian Lague"
 * Video link: https://www.youtube.com/watch?v=COmtTyLCd6I&list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3&index=11&ab_channel=SebastianLague
 * not the same exact code tho be careful
 */

using System;
using UnityEngine;

public class FallOffGenerator
{
    public static float[,] Generate(Vector2Int size, float falloffStart, float falloffEnd)
    {
        float[,] heightMap = new float[size.x, size.y];
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                Vector2 position = new Vector2(
                    (float)x / size.x * 2 - 1,
                    (float)y / size.y * 2 - 1
                     );

                float t = Mathf.Max(Mathf.Abs(position.x), Mathf.Abs(position.y));

                if (t < falloffStart)
                {
                    heightMap[x, y] = 0;
                }else if (t > falloffEnd)
                {
                    heightMap[x, y] = 1;
                }
                else
                {
                    heightMap[x, y] = Mathf.SmoothStep(0, 1, Mathf.InverseLerp(falloffStart, falloffEnd, t));
                }
            }
        }
        return heightMap;
    }
}

[Serializable]
public class FalloffSettings
{
    [Range(0, 1)]public float falloffStart = 0.5f;
    [Range(0, 1)]public float falloffEnd = 1f;
}
