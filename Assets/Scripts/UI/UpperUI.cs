
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpperUI : MonoBehaviour
{
  [SerializeField] private TMP_Text scoreText;
  [SerializeField] private TMP_Text BestscoreText;
  [SerializeField] private Button newGameButton;

  private void Awake()
  {
    newGameButton.onClick.AddListener(() =>
    {
      GameManager.Instance.NewGame();
    });
  }

  private void Start()
  {
    //to see when start game
    LoadBestScoreText();
    
    GameManager.Instance.onScoreUpdate += GameManager_ONScoreUpdate;
    GameManager.Instance.onBestScoreUpdate += GameManager_OnBestScoreUpdate;
  }

  private void GameManager_OnBestScoreUpdate()
  {
    LoadBestScoreText();
  }

  private void GameManager_ONScoreUpdate(int score)
  {
    scoreText.text = score.ToString();
  }

  private void LoadBestScoreText()
  {
    BestscoreText.text = GameManager.Instance.LoadHighScore().ToString();
  }
}
