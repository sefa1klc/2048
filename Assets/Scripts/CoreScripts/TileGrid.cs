
using UnityEngine;
using Random = System.Random;

namespace CoreScripts
{
    public class TileGrid : MonoBehaviour
    {
        public Row[] rows { get; private set; }
        public Cell[] cells { get; private set; }

        [SerializeField] private int size => cells.Length;
        [SerializeField] private int hight => rows.Length;
        [SerializeField] private int width => size / hight;
        

        private void Awake()
        {
            rows = GetComponentsInChildren<Row>();
            cells = GetComponentsInChildren<Cell>();
        }

        #region getter

        public int GetSize()
        {
            return size;
        }
        public int GetHight()
        {
            return hight;
        }
        public int GetWidth()
        {
            return width;
        }

        #endregion

        private void Start()
        {
            //Y axis
            for (int i = 0; i < rows.Length; i++)
            {
                // X axis
                for (int j = 0; j < rows[i].cells.Length; j++)
                {
                    rows[i].cells[j].coordinates = new Vector2Int(j,i);
                }
            }
        }

        public Cell GetRandomEmptyCell()
        {
            
            int index = UnityEngine.Random.Range(0, cells.Length);// random function use with unityEngine
            int startingIndex = index;

            //occupied(dolu) if cells[index] is occupied pass this cell and use next
            while (cells[index].isOccupied) 
            {
                index++;
                //if index is out of bound reset it
                if(index >= cells.Length){ index = 0;}
                
                //if index = cells.length return null
                if (index == cells.Length)
                {
                    return null;
                }
            }
            return cells[index];
        }

        public Cell GetCell(int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < hight)
            {
                return rows[y].cells[x];
            }
            else
            {
                return null;
            }
        }
        
        //cell izin hemen yanındaki celli bulmamızı sağlar
        public Cell GetAdjacentCell(Cell cell, Vector2Int direction)
        {
            Vector2Int coordinates = cell.coordinates;
            coordinates.x += direction.x;
            coordinates.y -= direction.y;

            return GetCell(coordinates.x, coordinates.y);
;        }
    }
}