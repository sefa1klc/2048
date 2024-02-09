using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   [SerializeField] private Board _board;
   private int score;
   private int bestScore;
   
   public event Action onGameOver;
   public event Action<int> onScoreUpdate;
   public event Action onBestScoreUpdate;
   
   //Game Manager is singleton
   public static GameManager Instance { get; private set; }

   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      NewGame();
   }

   public void NewGame()
   {
      SetScore(0);
      onBestScoreUpdate?.Invoke();
      
      _board.ClearBoard();
      _board.CreateTile();
      _board.CreateTile();

      _board.enabled = true;
   }
    
   //do not any move when gameover
   public void GameOver()
   {
      _board.enabled = false;
      onGameOver?.Invoke();//learn what is event!!!!
   }

   public void IncreaseScore(int points)
   {
      SetScore(score + points);
   }
   
   private void SetScore(int score)
   {
      this.score = score;
      onScoreUpdate?.Invoke(score);
      
      SaveBestScore();
   }

   //to save and load 
   private void SaveBestScore()
   {
      bestScore = LoadHighScore();

      if (score > bestScore)
      {
         PlayerPrefs.SetInt((Consts.SaveValues.Best_score), score);
      }
   }
   public int LoadHighScore()
   {
      return PlayerPrefs.GetInt(Consts.SaveValues.Best_score, 0);
   }
}
