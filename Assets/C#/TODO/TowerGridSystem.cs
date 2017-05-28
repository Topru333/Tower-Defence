using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace TD
{
    public class TowerGridSystem : MonoBehaviour
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
        private static TowerGridSystem _instance;
        public static TowerGridSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject singleton = new GameObject();
                    _instance = singleton.AddComponent<TowerGridSystem>();
                    singleton.name = typeof(TowerGridSystem).ToString();
                }
                return _instance;
            }
        }
        private TowerCell[,] grid;
        private float cellSize = 1;
        private bool visualizeGrid=false;
        private Canvas canvas;
        private GridLayoutGroup gridLayoutGroup;
        private RawImage[,] cellVisualImages;
        [SerializeField]
        private RawImage cellAllowToBuild;
        [SerializeField]
        private RawImage cellNotAllowToBuild;

        // Use this for initialization
        void Awake()
        {
            cellAllowToBuild = Resources.Load<RawImage>("Prefabs/CellAllowToBuild");
            cellNotAllowToBuild = Resources.Load<RawImage>("Prefabs/CellDontAllowToBuild");
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
                cellVisualImages[i,j] = Instantiate(grid[i,j].state==CellState.CanBuild? cellAllowToBuild:cellNotAllowToBuild, gridLayoutGroup.transform);
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

    }
}