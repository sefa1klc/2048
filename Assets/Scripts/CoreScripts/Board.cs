
using System;
using System.Collections;
using System.Collections.Generic;
using CoreScripts;
using DG.Tweening;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private TileStateSObj[] _tileStateSObjs;

    [SerializeField] private float anmiationDuration = 0.1f;
    
    private List<Tile> tiles = new List<Tile>();

    private TileGrid grid;
    private bool isWaiting;

    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!isWaiting)// if isWaiting is False
        {
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveTiles(Vector2Int.up,0,1, 1,1);
            }
            else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveTiles(Vector2Int.down,0,1,grid.GetHight() - 2,-1);
            }
            else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveTiles(Vector2Int.right, grid.GetWidth() -2,-1,0,1);
            }
            else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveTiles(Vector2Int.left, 1,1,0,1);
            }
        }
        
    }

    public void ClearBoard()
    {
        foreach (Cell cell in grid.cells)
        {
            cell.tile = null;
        }

        foreach (Tile tile in tiles)
        {
            Destroy(tile.gameObject);//for destroy all tile prefab
        }

        tiles.Clear();// to clear tiles List
    }

    //TEMEL MEKANİK
    private void MoveTiles(Vector2Int direction, int startX, int incrementX,int startY, int incrementY)
    {
        bool isChanged = false;

        for (int x = startX; x >= 0 && x < grid.GetWidth(); x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.GetHight(); y += incrementY)
            {
                Cell cell = grid.GetCell(x, y);

                if(cell.isOccupied)
                {
                    isChanged |= MoveTile(cell.tile, direction);
                }
            }
        }
        
        //eğer arka arkaya hamleler yapılırsa animasyonlu geçiste gridlere tam oturmama gibi bir sorun olmaması için yazıyoruz.
        if (isChanged)
        {
            StartCoroutine(WaitForChanges());
        }
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        Cell newCell = null;
        Cell adjacentCell = grid.GetAdjacentCell(tile.cell, direction);

        while(adjacentCell != null)
        {
            if(adjacentCell.isOccupied)
            {
                if(CanMerge(tile, adjacentCell.tile))
                {
                    Merge(tile, adjacentCell.tile);
                    return true;
                }

                break;
            }

            newCell = adjacentCell;
            adjacentCell = grid.GetAdjacentCell(adjacentCell, direction);
        }

        if(newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }

        return false;
    }
    
    //Create tile with instantiate
    public void CreateTile()
    {
        Tile tile = Instantiate(_tilePrefab, grid.transform);
        tile.SetState(_tileStateSObjs[0],Consts.Numbers.Number_2);
        tile.SpawnTile(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }

    // control for 2 tiles can merge
    private bool CanMerge(Tile x, Tile y)
    {
        // isLock control to for example when we have 2-2-2-2 state if press the D button result is 8 we do not want like that.
        //we wnat one move one merge so 2-2-2-2 state result is 4-4 not 8
        return x.number == y.number && !y.isLocked; 
    }

    private void Merge(Tile x, Tile y)
    {
        tiles.Remove(x);
        x.Merge(y.cell);

        int index = Mathf.Clamp(IndexOF(y.tileState) +1 ,0, _tileStateSObjs.Length -1);
        int number = y.number * 2;
        
        y.SetState(_tileStateSObjs[index], number);
        
        AnimationTiles(y, anmiationDuration);
        
        //to increase score with which number is merging
        GameManager.Instance.IncreaseScore(number);
        
        AudioManager.Instance.Play(Consts.audio.mergeSound);
    }

    private int IndexOF(TileStateSObj tileState)
    {
        for (int i = 0; i < _tileStateSObjs.Length ; i++)
        {
            if (tileState == _tileStateSObjs[i])
            {
                return i;
            }
        }
        
        return -1;
    }
    
    //when two tile merge, Make a growing and shrinking animation
    private void AnimationTiles(Tile tileToAnimation, float anmiationDuration)
    {
        tileToAnimation.gameObject.transform.DOScale(1.25f, anmiationDuration).OnComplete(() =>
        {
            tileToAnimation.gameObject.transform.DOScale(1f, anmiationDuration);
        });
    }
    
    // I do not want to play anything while if a merge animation play .
    private IEnumerator WaitForChanges()
    {
        isWaiting = true;
        float waitingDuration = 0.1f;  
        yield return new WaitForSeconds(waitingDuration);

        isWaiting = false;

        foreach (Tile tile in tiles)
        {
            tile.isLocked = false;
        }
        
        if(tiles.Count != grid.GetSize())
        {
            CreateTile();
        }

        if (CheckForGameOver())
        {
            GameManager.Instance.GameOver();;
        }
    }

    private bool CheckForGameOver()
    {
        //check for tiles count < 16
        if (tiles.Count != grid.GetSize())
        {
            return false;
        }
        
        // check for all cells cannot merge any cell and has any empty cell
        foreach (Tile tile  in tiles )
        {
            Cell upCell = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            Cell downCell = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            Cell rightCell = grid.GetAdjacentCell(tile.cell, Vector2Int.right);
            Cell leftCell = grid.GetAdjacentCell(tile.cell, Vector2Int.left);

            if (upCell != null && CanMerge(tile, upCell.tile))
            {
                return false;
            }
            else if (downCell != null && CanMerge(tile, downCell.tile))
            {
                return false;
            }
            else if (rightCell != null && CanMerge(tile, rightCell.tile))
            {
                return false;
            }
            else if (leftCell != null && CanMerge(tile, leftCell.tile))
            {
                return false;
            }
            
        }

        return true;
    }
}
