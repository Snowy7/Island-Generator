/*
 * Most of the code below inspired by "Sebastian Lague"
 * Video link: https://www.youtube.com/watch?v=WP-Bm65Q-1Y&list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3&index=2&ab_channel=SebastianLague
 * not the same exact code tho be careful
 */

using System;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves,
        float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octavesOffset = new Vector2[octaves];

        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) - offset.y;

            octavesOffset[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0) scale = 0.0001f;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;
        
        float minValue = float.MaxValue;
        float maxValue = float.MinValue;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth) / scale * frequency + octavesOffset[i].x * frequency;
                    float sampleY = (y - halfHeight) / scale * frequency - octavesOffset[i].y * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight < minValue) minValue = noiseHeight;
                if (noiseHeight > maxValue) maxValue = noiseHeight;
                noiseMap[x, y] = noiseHeight;
            }
        }
        
        // Normalize:
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minValue, maxValue, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}

[Serializable]
public class NoiseSettings
{
    public float scale = 50f;
    [Range(0, 20)]
    public int octaves = 6;
    [Range(0, 1)] public float persistance = .6f;
    public float lactuanirty = 2f;
    public int seed;
    public Vector2 offset;

    public void ValidateValues()
    {
        scale = Mathf.Max(scale, 0.01f);
        octaves = Mathf.Max(octaves, 1);
        lactuanirty = Mathf.Max(lactuanirty, 1);
        persistance = Mathf.Clamp01(persistance);
    }
}
