using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace TD
{
    public class TowerGridSystem : Singleton<TowerGridSystem>
    {
        public enum CellState
        {
            CantBuild,
            CanBuild,
            Builded,
            Unknown
        }
        public struct TowerCell
        {
            public CellState state;
            public Tower tower;
        }
        private TowerCell[,] grid;
        private float cellSize = 1;
        private bool visualizeGrid=false;
        private Canvas canvas;
        private GridLayoutGroup gridLayoutGroup;
        private RawImage[,] cellVisualImages;
        //private RawImage cellAllowToBuild;
        //private RawImage cellNotAllowToBuild;
        private RawImage cellDefault;

        // Use this for initialization
        void Awake()
        {
            //cellAllowToBuild = Resources.Load<RawImage>("Prefabs/CellAllowToBuild");
            //cellNotAllowToBuild = Resources.Load<RawImage>("Prefabs/CellDontAllowToBuild");
            cellDefault = Resources.Load<RawImage>("Prefabs/CellDefault");
            GameObject go = new GameObject("GridVisualApperance");
            go.transform.SetParent(transform);
            canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            gridLayoutGroup = go.AddComponent<GridLayoutGroup>();
        }

        public void LoadData(StreamReader sr)
        {
            string[] size= sr.ReadLine().Split(' ');
            int w, h;float gsize;
            if (!int.TryParse(size[0], out w)||!int.TryParse(size[1], out h)|| !float.TryParse(size[2], out gsize))
                throw new IOException("Ошибка при чтении потока");
            cellSize = gsize;
            grid = new TowerCell[h, w];

            int i = 0, j = 0;

            Utilities.ReadBlock(sr, (string _line) =>
            {
                string[] numbers = _line.Split(' ');
                    
                for (j = 0; j < numbers.Length; j++) {
                    int _state;
                    if (int.TryParse(numbers[j], out _state))
                        grid[i, j] = new TowerCell { state = (CellState)_state, tower = null };
                    else
                        throw new IOException("Ошибка при чтении потока");
                }
                i++;
            } );
            gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
            canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(cellSize * h, cellSize * w);

            gridLayoutGroup.transform.SetPositionAndRotation(new Vector3(cellSize*h/2,0.5f,cellSize*w/2), Quaternion.Euler(90,0,0));
            cellVisualImages = new RawImage[h,w];
            for (j = w-1; j >= 0; j--)
            for (i = 0; i < h; i++)
            {
                cellVisualImages[i,j] = Instantiate(cellDefault, gridLayoutGroup.transform);
            }
        }

        //возвращает состояние ячейки находящейся в данной позиции в 3х-мерном пространстве.
        public CellState GetCellInfo(Vector3 position)
        {
            int cellID_x = Mathf.FloorToInt((position.x - transform.position.x) / cellSize);
            int cellID_y = Mathf.FloorToInt((position.z - transform.position.z) / cellSize);
            if ((cellID_x >= 0 && cellID_x < grid.GetLength(0)) && (cellID_y >= 0 && cellID_y < grid.GetLength(1)))
                return grid[cellID_x, cellID_y].state;
            return CellState.Unknown;
        }
        //Строит башню в ячейке сетки.
        public bool BuildTowerAt(Vector3 position, GameObject Tower)
        {
            CellState cstate = GetCellInfo(position);
            if (cstate == CellState.Unknown || cstate == CellState.CantBuild || cstate == CellState.Builded)
                return false;
            int cellID_x = Mathf.FloorToInt((position.x - transform.position.x) / cellSize);
            int cellID_y = Mathf.FloorToInt((position.z - transform.position.z) / cellSize);

            grid[cellID_x, cellID_y].tower = Instantiate(Tower, new Vector3((cellID_x + 0.5f) * cellSize + transform.position.x, 0, (cellID_y + 0.5f) * cellSize + transform.position.z), Quaternion.identity).GetComponent<Tower>();
            grid[cellID_x, cellID_y].state = CellState.Builded;

            return true;
        }

        // Возвращает ячейку находящейся в данной позиции в 3х-мерном пространстве.
        public bool GetTowerCellAt(Vector3 position,out int cellX, out int cellY)
        {
            cellX=cellY= 0;
            int cellID_x = Mathf.FloorToInt((position.x - transform.position.x) / cellSize);
            int cellID_y = Mathf.FloorToInt((position.z - transform.position.z) / cellSize);
            if ((cellID_x >= 0 && cellID_x < grid.GetLength(0)) && (cellID_y >= 0 && cellID_y < grid.GetLength(1)))
            {
                cellX = cellID_x;cellY= cellID_y;
                return true;
            }
            return false;
        }
        // Возвращает ячейку находящейся в данной позиции в 3х-мерном пространстве.
        public void SellTowerAt(int cellX, int cellY)
        {
            grid[cellX,cellY].state = CellState.CanBuild;
            LevelManager.Instance.CurrentLevel.GiveMoney(grid[cellX, cellY].tower.GetSellPrice());
            Destroy(grid[cellX, cellY].tower.gameObject);
            grid[cellX, cellY].tower = null;
        }

        public void UpgradeTowerAt(int cellX, int cellY)
        {            
            grid[cellX, cellY].tower.Upgrade();
        }

        private void OnDrawGizmos()
        {
            int m= grid.GetLength(0), n= grid.GetLength(1);
            for (int i = 0; i <= m; i++)
                Gizmos.DrawRay(new Vector3(i * cellSize + transform.position.x, 0, transform.position.z), new Vector3(0, 0, (n) * cellSize));
            for (int i = 0; i <= n; i++)
                Gizmos.DrawRay(new Vector3(transform.position.x, 0, i * cellSize + transform.position.z), new Vector3((m) * cellSize, 0, 0));
        }

        public void ToggleGridVizualization()
        {
            visualizeGrid = !visualizeGrid;
            canvas.gameObject.SetActive(visualizeGrid);
        }
        // Подсвечивает ячейку
        public void HighlightCell(Vector3 position, out int x, out int y)
        {
            x = y = 0;
            x = Mathf.FloorToInt((position.x - transform.position.x) / cellSize);
            y = Mathf.FloorToInt((position.z - transform.position.z) / cellSize);
            cellVisualImages[x, y].color = grid[x, y].state == CellState.CanBuild ? new Color(0, 1, 0, 0.5f): new Color(1, 0, 0, 0.5f);
        }

        // Убирает подсветку ячейки
        public void ResetHighlightOfCell(int x, int y)
        {
            cellVisualImages[x, y].color = new Color(1, 1, 1, 0.5f);
        }

        public void GetBBox(out Vector3 a, out Vector3 b, out Vector3 c, out Vector3 d)
        {
            float width  = grid.GetLength(0) * cellSize, 
                  height = grid.GetLength(1) * cellSize;

            a = new Vector3(transform.position.x, 0, transform.position.z);
            b = new Vector3(transform.position.x + width, 0, transform.position.z);
            c = new Vector3(transform.position.x, 0, transform.position.z+height);
            d = new Vector3(transform.position.x + width, 0, transform.position.z+height);
        }
    }
}