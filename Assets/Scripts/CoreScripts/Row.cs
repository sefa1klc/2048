using System;
using UnityEngine;

namespace CoreScripts
{
    public class Row : MonoBehaviour
    {
        public Cell[] cells { get; private set; }

        private void Awake()
        {
            //if obj has the cell script, drop them "cells" array
            cells = GetComponentsInChildren<Cell>();
        }
    }
}