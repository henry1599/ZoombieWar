using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Survival
{
    public class CaveGenerator : MonoBehaviour
    {
        [HorizontalGroup("GridSize")]
        [Range(1, 2000), SerializeField] int width;
        
        [HorizontalGroup("GridSize")]
        [Range(1, 2000), SerializeField] int height;
        [SerializeField] float cellSize = 1;
        [SerializeField, Required] Transform pivot;
        [SerializeField, Required] MeshGenerator meshGenerator;
        [SerializeField, Required] MeshToSprite meshToSprite;
        [SerializeField, Required] SpriteRenderer spriteRenderer;


        [BoxGroup("Generate settings"), SerializeField] string seed;
        [BoxGroup("Generate settings"), SerializeField] bool useRandomSeed;
        [BoxGroup("Generate settings"), SerializeField] [Range(0, 100)] int randomFillPercent;
        [BoxGroup("Generate settings"), SerializeField] [Range(0, 8)] int surroundingWallCount;
        [BoxGroup("Generate settings"), SerializeField] [Range(0, 10)] int smoothCount;
        [BoxGroup("Generate settings"), SerializeField] [Range(0, 200)] int wallThresholdSize;
        [BoxGroup("Generate settings"), SerializeField] [Range(0, 200)] int roomThresholdSize;

        Cell[,] grid;
        [Button]
        public void Generate()
        {
            if (this.useRandomSeed)
            {
                this.seed = Time.time.ToString();
            }
            System.Random random = new System.Random(seed.GetHashCode());
            grid = new Cell[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Cell cell = new Cell(x, y, this.cellSize);
                    cell.SetupPosition(pivot, width, height);

                    // * Wall
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    {
                        cell.Flag = true;
                    }
                    else
                    {
                        cell.Flag = random.Next(0, 100) < randomFillPercent;
                    }
                    grid[x, y] = cell;
                }
            }

            for (int i = 0; i < smoothCount; i++)
            {
                SmoothMap();
            }
            ProcessMap();

            Mesh mesh = this.meshGenerator.GenerateMesh(grid, this.cellSize);
            Sprite sprite = this.meshToSprite.GenerateSpriteFromMesh(mesh, width, height);
            this.spriteRenderer.sprite = sprite;

        }
        void SmoothMap()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int neighbourWallTiles = GetSurroundingWallCount(x, y);
                    if (neighbourWallTiles > this.surroundingWallCount)
                    {
                        grid[x, y].Flag = true;
                    }
                    else if (neighbourWallTiles < this.surroundingWallCount)
                    {
                        grid[x, y].Flag = false;
                    }
                }
            }
        }

        private int GetSurroundingWallCount(int x, int y)
        {
            int wallCount = 0;
            for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
            {
                for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
                {
                    if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                    {
                        if (neighbourX != x || neighbourY != y)
                        {
                            if (grid[neighbourX, neighbourY].Flag)
                            {
                                wallCount++;
                            }
                        }
                    }
                    else
                    {
                        wallCount++;
                    }
                }
            }
            return wallCount;
        }
        List<Cell> GetRegionCells(int startX, int startY)
        {
            List<Cell> cells = new List<Cell>();
            bool[,] visited = new bool[width, height];
            bool targetFlag = grid[startX, startY].Flag;
            Queue<Cell> queue = new Queue<Cell>();
            queue.Enqueue(grid[startX, startY]);
            visited[startX, startY] = true;
            while (queue.Count > 0)
            {
                Cell cell = queue.Dequeue();
                cells.Add(cell);
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        int checkX = cell.X + x;
                        int checkY = cell.Y + y;
                        if (x == 0 || y == 0)
                        {
                            if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                            {
                                if (!visited[checkX, checkY] && grid[checkX, checkY].Flag == targetFlag)
                                {
                                    visited[checkX, checkY] = true;
                                    queue.Enqueue(grid[checkX, checkY]);
                                }
                            }
                        }
                    }
                }
            }
            return cells;
        }
        List<List<Cell>> GetRegions(bool flag)
        {
            List<List<Cell>> regions = new List<List<Cell>>();
            bool[,] visited = new bool[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (!visited[x, y] && grid[x, y].Flag == flag)
                    {
                        List<Cell> region = GetRegionCells(x, y);
                        foreach (var cell in region)
                        {
                            visited[cell.X, cell.Y] = true;
                        }
                        regions.Add(region);
                    }
                }
            }
            return regions;
        }
        void ProcessMap()
        {
            List<List<Cell>> wallRegions = GetRegions(true);
            foreach (var wallRegion in wallRegions)
            {
                if (wallRegion.Count < this.wallThresholdSize)
                {
                    foreach (var cell in wallRegion)
                    {
                        cell.Flag = false;
                    }
                }
            }
            List<List<Cell>> roomRegions = GetRegions(false);
            foreach (var roomRegion in roomRegions)
            {
                if (roomRegion.Count < this.roomThresholdSize)
                {
                    foreach (var cell in roomRegion)
                    {
                        cell.Flag = true;
                    }
                }
            }
        }

        void OnDrawGizmos()
        {
            if (grid == null) return;
            foreach (var cell in this.grid)
            {
                Gizmos.color = cell.Flag ? Color.black : Color.white;
                Gizmos.DrawCube(cell.Position, Vector3.one * cell.Size);
            }
        }
    }
    [SerializeField, InlineEditor]
    public class Cell
    {
        public int X;
        public int Y;
        public float Size;
        public Vector2 Position;
        public bool Flag;
        public Cell(int x, int y, float size = 1)
        {
            this.X = x;
            this.Y = y;
            this.Size = size;
            this.Flag = false;
        }
        public void SetupPosition(Transform pivot, int gridWidth, int gridHeight)
        {
            this.Position = new Vector2(X, Y) * Size + (Vector2)pivot.position - new Vector2(gridWidth, gridHeight) * Size * 0.5f;
        }
    }
}
