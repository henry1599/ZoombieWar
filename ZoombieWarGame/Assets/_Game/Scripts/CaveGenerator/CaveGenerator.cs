using System;
using System.Collections;
using System.Collections.Generic;
using HenryDev;
using HenryDev.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Survival
{
    public enum eWallDirection
    {
        None = 0,
        Up = 1 << 1,
        Down = 1 << 2,
        Left = 1 << 3,
        Right = 1 << 4,
        UpLeft = Up | Left,
        UpRight = Up | Right,
        DownLeft = Down | Left,
        DownRight = Down | Right,
        All = Up | Down | Left | Right
    }
    public class CaveGenerator : MonoBehaviour
    {
        [HorizontalGroup("GridSize")]
        [Range(1, 2000), SerializeField] int width;
        
        [HorizontalGroup("GridSize")]
        [Range(1, 2000), SerializeField] int height;
        
        [HorizontalGroup("Grid Real Size")]
        [ReadOnly, ShowInInspector] int realWidth => this.width + this.borderSize * 2;
        
        [HorizontalGroup("Grid Real Size")]
        [ReadOnly, ShowInInspector] int realHeight => this.height + this.borderSize * 2;
        [SerializeField] float cellSize = 1;
        [SerializeField, Required] Transform pivot;
        [SerializeField, Required] Transform tileContainer;


        [Header("Setttings")]
        [BoxGroup("Generate settings"), SerializeField]                     string seed;
        [BoxGroup("Generate settings"), SerializeField]                     bool useRandomSeed;
        [BoxGroup("Generate settings"), SerializeField, Range(0, 100)]      int randomFillPercent;
        [BoxGroup("Generate settings"), SerializeField, Range(0, 8)]        int surroundingWallCount;
        [BoxGroup("Generate settings"), SerializeField, Range(0, 10)]       int smoothCount;
        [BoxGroup("Generate settings"), SerializeField, Range(0, 200)]      int wallThresholdSize;
        [BoxGroup("Generate settings"), SerializeField, Range(0, 200)]      int roomThresholdSize;
        [BoxGroup("Generate settings"), SerializeField, Range(1, 50)]       int borderSize;


        [Space(10)]
        [BoxGroup("Visual settings"), SerializeField] GameObject walTilePrefab;
        [BoxGroup("Visual settings"), SerializeField] GameObject groundTilePrefab;
        [BoxGroup("Visual settings"), SerializeField, PreviewField] Sprite groundSprite;
        [HorizontalGroup("Up tile"), SerializeField, PreviewField, LabelText("Up Left")]        Sprite upLeftTileSPrite;
        [HorizontalGroup("Up tile"), SerializeField, PreviewField, LabelText("Up")]             Sprite upTileSprite;
        [HorizontalGroup("Up tile"), SerializeField, PreviewField, LabelText("Up Right")]       Sprite upRightTileSprite;
        [HorizontalGroup("Mid tile"), SerializeField, PreviewField, LabelText("Left")]          Sprite leftTileSprite;
        [HorizontalGroup("Mid tile"), SerializeField, PreviewField, LabelText("Center")]        Sprite centerTileSprite;
        [HorizontalGroup("Mid tile"), SerializeField, PreviewField, LabelText("Right")]         Sprite rightTileSprite;
        [HorizontalGroup("Down tile"), SerializeField, PreviewField, LabelText("Down Left")]    Sprite downLeftTileSprite;
        [HorizontalGroup("Down tile"), SerializeField, PreviewField, LabelText("Down")]         Sprite downTileSprite;
        [HorizontalGroup("Down tile"), SerializeField, PreviewField, LabelText("Down Right")]   Sprite downRightTileSprite;


        Cell[,] grid;
        Region mainRoom;


        public Region MainRoom => mainRoom;

        public void Generate(Action onFinished = null)
        {
            if (this.useRandomSeed)
            {
                this.seed = Time.time.ToString();
            }
            System.Random random = new System.Random(seed.GetHashCode());
            grid = new Cell[this.realWidth, this.realHeight];
            for (int x = 0; x < this.realHeight; x++)
            {
                for (int y = 0; y < this.realHeight; y++)
                {
                    Cell cell = new Cell(x, y, this.cellSize);
                    cell.SetupPosition(pivot, this.realWidth, this.realHeight);

                    bool isRoom = x > this.borderSize - 1 && x < this.realWidth - this.borderSize - 1 && y > this.borderSize - 1 && y < this.realHeight - this.borderSize - 1;
                    if (isRoom)
                        cell.Flag = random.Next(0, 100) < randomFillPercent;
                    else
                        cell.Flag = true;
                    grid[x, y] = cell;
                }
            }

            for (int i = 0; i < smoothCount; i++)
            {
                SmoothMap();
            }
            CutoutSmallRoomAndWall();
            ProcessWallDirection();
            SpawnTiles();
            GetMainRoom();

            onFinished?.Invoke();
        }
        void SmoothMap()
        {
            for (int x = 0; x < this.realWidth; x++)
            {
                for (int y = 0; y < this.realHeight; y++)
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
        void SpawnTiles()
        {
            this.tileContainer.DeleteChildren();
            LoopGrid(cell =>
            {
                if (cell.Flag)
                {
                    var wallGo = Instantiate(this.walTilePrefab, cell.Position, Quaternion.identity, this.tileContainer);
                    wallGo.transform.localScale = Vector3.one * cell.Size;
                    var wallTile = wallGo?.GetComponent<WallTile>();
                    wallTile?.Setup(cell, GetWallSprite(cell.WallDirection));
                }
            });
            var groundGo = Instantiate(this.groundTilePrefab, this.pivot.position, Quaternion.identity, this.tileContainer);
            groundGo.transform.localScale = new Vector3(this.grid.GetLength(0), this.grid.GetLength(1), 1f) * this.cellSize;
            var groundSprite = groundGo?.GetComponent<SpriteRenderer>();
            if (groundSprite == null)
                return;
            groundSprite.sprite = this.groundSprite;
        }

        private int GetSurroundingWallCount(int x, int y)
        {
            int wallCount = 0;
            for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
            {
                for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
                {
                    if (neighbourX >= 0 && neighbourX < this.realWidth && neighbourY >= 0 && neighbourY < this.realHeight)
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
            bool[,] visited = new bool[this.realWidth, this.realHeight];
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
                            if (checkX >= 0 && checkX < this.realWidth && checkY >= 0 && checkY < this.realHeight)
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
        void GetMainRoom()
        {
            List<Region> rooms = GetRegions(false);
            int maxSize = 0;
            foreach (var room in rooms)
            {
                if (room.Size > maxSize)
                {
                    maxSize = room.Size;
                    this.mainRoom = room;
                }
            }
        }
        public List<Region> GetRegions(bool flag)
        {
            List<Region> rooms = new List<Region>();
            bool[,] visited = new bool[this.realWidth, this.realHeight];
            for (int x = 0; x < this.realWidth; x++)
            {
                for (int y = 0; y < this.realHeight; y++)
                {
                    if (!visited[x, y] && grid[x, y].Flag == flag)
                    {
                        List<Cell> region = GetRegionCells(x, y);
                        foreach (var cell in region)
                        {
                            visited[cell.X, cell.Y] = true;
                        }
                        rooms.Add(new Region(region));
                    }
                }
            }
            return rooms;
        }
        void CutoutSmallRoomAndWall()
        {
            List<Region> wallRegions = GetRegions(true);
            foreach (var wallRegion in wallRegions)
            {
                if (wallRegion.Size < this.wallThresholdSize)
                {
                    foreach (var cell in wallRegion.Cells)
                    {
                        cell.Flag = false;
                    }
                }
            }
            List<Region> roomRegions = GetRegions(false);
            foreach (var roomRegion in roomRegions)
            {
                if (roomRegion.Size < this.roomThresholdSize)
                {
                    foreach (var cell in roomRegion.Cells)
                    {
                        cell.Flag = true;
                    }
                }
            }
        }
        void ProcessWallDirection()
        {
            for (int x = 0; x < this.realWidth; x++)
            {
                for (int y = 0; y < this.realHeight; y++)
                {
                    if (!grid[x, y].Flag)
                    {
                        grid[x, y].WallDirection = eWallDirection.None;
                        continue;
                    }
                    eWallDirection wallDirection = eWallDirection.None;
                    for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
                    {
                        for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
                        {
                            if (neighbourX >= 0 && neighbourX < this.realWidth && neighbourY >= 0 && neighbourY < this.realHeight)
                            {
                                if (neighbourX == x && neighbourY == y)
                                    continue;
                                if (grid[neighbourX, neighbourY].Flag)
                                    continue;
                                if (neighbourX == x && neighbourY == y - 1)
                                    wallDirection |= eWallDirection.Down;
                                if (neighbourX == x && neighbourY == y + 1)
                                    wallDirection |= eWallDirection.Up;
                                if (neighbourX == x - 1 && neighbourY == y)
                                    wallDirection |= eWallDirection.Left;
                                if (neighbourX == x + 1 && neighbourY == y)
                                    wallDirection |= eWallDirection.Right;
                            }
                        }
                    }
                    grid[x, y].WallDirection = wallDirection;
                }
            }
        }
        Sprite GetWallSprite(eWallDirection direction) => direction switch
        {
            eWallDirection.Up           => this.upTileSprite,
            eWallDirection.Down         => this.downTileSprite,
            eWallDirection.Left         => this.leftTileSprite,
            eWallDirection.Right        => this.rightTileSprite,
            eWallDirection.UpLeft       => this.upLeftTileSPrite,
            eWallDirection.UpRight      => this.upRightTileSprite,
            eWallDirection.DownLeft     => this.downLeftTileSprite,
            eWallDirection.DownRight    => this.downRightTileSprite,
            _ => this.centerTileSprite
        };
        void OnDrawGizmos()
        {
            // if (grid == null) return;
            // foreach (var cell in this.grid)
            // {
            //     Gizmos.color = cell.Flag ? Color.black : Color.white;
            //     Gizmos.DrawCube(cell.Position, Vector3.one * cell.Size);
            // }
        }
        void LoopGrid(Action<Cell> action)
        {
            for (int x = 0; x < this.realWidth; x++)
            {
                for (int y = 0; y < this.realHeight; y++)
                {
                    action(grid[x, y]);
                }
            }
        }
        void LoopGrid(Action<int, int> action)
        {
            for (int x = 0; x < this.realWidth; x++)
            {
                for (int y = 0; y < this.realHeight; y++)
                {
                    action(x, y);
                }
            }
        }
        void LoopGridLocal(Action<Cell> action)
        {
            for (int x = this.borderSize; x < this.width + this.borderSize; x++)
            {
                for (int y = this.borderSize; y < this.height + this.borderSize; y++)
                {
                    action(grid[x, y]);
                }
            }
        }
        void LoopGridLocal(Action<int, int> action)
        {
            for (int x = this.borderSize; x < this.width + this.borderSize; x++)
            {
                for (int y = this.borderSize; y < this.height + this.borderSize; y++)
                {
                    action(x, y);
                }
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
        public eWallDirection WallDirection;
        public Sprite VisualSprite;
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
    [SerializeField, InlineEditor]
    public class Region
    {
        public int Size;
        public List<Cell> Cells;
        public Region(List<Cell> cells)
        {
            this.Cells = cells;
            this.Size = cells.Count;
        }
        public Cell GetCenterCell()
        {
            return Cells[Cells.Count / 2];
        }
    }
}
