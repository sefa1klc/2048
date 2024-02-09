using UnityEngine;

namespace CoreScripts
{
    public class Cell : MonoBehaviour
    {
        public Vector2Int coordinates { get; set; } // This is Property . We can get or set this variable in other scripts.
        public Tile tile { get; set; }
        
        public bool isEmpty => tile == null;// variable bound(bağlamak) already to a condition.
        public bool isOccupied => tile != null;
    }
}