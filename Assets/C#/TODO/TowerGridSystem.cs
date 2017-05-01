using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
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
        private bool visualizeGrid;

        // Use this for initialization
        void Awake()
        {

        }

        public void LoadData(Stream stream)
        {
            using (StreamReader sr = new StreamReader(stream))
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

    }
}