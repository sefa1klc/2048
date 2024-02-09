using System.Collections;
using System.Security.Cryptography;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace CoreScripts
{
    public class Tile : MonoBehaviour
    {
        public TileStateSObj tileState { get; private set; }
        public Cell cell { get; private set; }
        public int number { get; private set; }
        public bool isLocked { get; set; }

        [SerializeField] private Image BackGroundImage;
        [SerializeField] private TMP_Text numberText;

        public void SetState(TileStateSObj tileState, int number)
        {
            //When the numbers change during moves, for example from 2 to 4, we draw the color and text of the new number.
            this.tileState = tileState;
            this.number = number;

            BackGroundImage.color = tileState.BackgroundColor;
            numberText.color = tileState.TextColor;
            numberText.text = number.ToString();
        }

        public void SpawnTile(Cell cell)
        {
            if (this.cell != null)
            {
                //First we check the is cell null. if it's not we have to null because if is not we cannot spawn new tile there.
                this.cell.tile = null;
            }

            this.cell = cell;
            this.cell.tile = this;

            transform.position = cell.transform.position;
        }
        
        public void MoveTo(Cell cell)
        {
            if (this.cell != null)
            {
                //First we check the is cell null. if it's not we have to null because if is not we cannot spawn new tile there.
                this.cell.tile = null;
            }

            this.cell = cell;
            this.cell.tile = this;

            //to do animation, asynchronous. it take the IEnumerator param
            StartCoroutine(Animate(cell.transform.position, false));
        }

        // when we press w-a-s-d, we need the animation for transform.position to destination 
        private IEnumerator Animate(Vector3 destination, bool isMerging)
        {
            float elapsed = 0f; // keeps the elapsed(geçmek,geçen) time
            float duration = 0.1f; // how long it will take

            Vector3 from = transform.position;

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(from, destination, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null; // this is for wait a frame. to avoid bug

            }

            transform.position = destination;

            if (isMerging)
            {
                Destroy(this.gameObject);
            }
            
        }

        public void Merge(Cell cell)
        {
            if (this.cell != null)
            {
                this.cell.tile = null;
            }

            this.cell = null;
            cell.tile.isLocked = true;
            
            //to do animation, asynchronous. it take the IEnumerator param
            StartCoroutine(Animate(cell.transform.position, true));
        }
    }
}