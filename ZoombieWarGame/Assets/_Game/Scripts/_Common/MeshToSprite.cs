using UnityEngine;

public class MeshToSprite : MonoBehaviour
{
    public Sprite GenerateSpriteFromMesh(Mesh mesh, int with, int height)
    {
        Texture2D texture = GenerateTextureFromMesh(mesh, with, height);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }
    Texture2D GenerateTextureFromMesh(Mesh mesh, int textureWidth, int textureHeight)
    {
        Texture2D texture = new Texture2D(textureWidth, textureHeight);
        Color[] pixels = new Color[textureWidth * textureHeight];

        // Clear the texture to a default color (optional)
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.clear;  // Set to transparent initially
        }

        // Project vertices to 2D UV space and fill the texture
        Vector3[] vertices = mesh.vertices;
        Color[] vertexColors = mesh.colors;

        for (int i = 0; i < vertexColors.Length; i++)
        {
            // Convert vertex position to UV coordinates (normalized between 0 and 1)
            Vector2 uv = new Vector2((vertices[i].x - mesh.bounds.min.x) / mesh.bounds.size.x,
                                     (vertices[i].y - mesh.bounds.min.y) / mesh.bounds.size.y);

            // Convert UV coordinates to texture coordinates
            int x = Mathf.FloorToInt(uv.x * textureWidth);
            int y = Mathf.FloorToInt(uv.y * textureHeight);

            // Assign the vertex color to the corresponding pixel in the texture
            if (x >= 0 && x < textureWidth && y >= 0 && y < textureHeight)
            {
                int pixelIndex = y * textureWidth + x;
                pixels[pixelIndex] = vertexColors[i];
            }
        }

        // Apply the pixel colors to the texture
        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }
}
