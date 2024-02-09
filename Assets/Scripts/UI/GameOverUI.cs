using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button tryAgainButton;
    [SerializeField] private float fadeDuration = 1f;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        //Same as selecting the function in the inspector - onClick()
        tryAgainButton.onClick.AddListener(() =>
        { 
            GameManager.Instance.NewGame();
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false; //to cannot press the button while playing game
        });
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        GameManager.Instance.onGameOver += GameManager_OnGameOver;
    }

    private void GameManager_OnGameOver()
    {
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        float delaySecond = 1f;
        yield return new WaitForSeconds(delaySecond);// to wait when the gameover
        _canvasGroup.DOFade(1f, fadeDuration);//for fade animation
        _canvasGroup.interactable = true;//to press the button
    }
}