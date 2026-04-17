using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections;
using LightSide;

[System.Serializable]
public class QuestionData
{
    public string questionText;
    public string[] options = new string[4];
    public int correctAnswerIndex;
    public string hintText;
    public int hintCost = 10;
}

public class QuizManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CanvasGroup mainPanelGroup;
    [SerializeField] private RectTransform mainPanelRect;
    [SerializeField] private UniText questionText;
    [SerializeField] private Button[] optionButtons;
    [SerializeField] private UniText[] optionTexts;

    [Header("Hint Popup References")]
    [SerializeField] private CanvasGroup hintPopupGroup;
    [SerializeField] private RectTransform hintPopupRect;
    [SerializeField] private UniText hintTextDisplay;
    [SerializeField] private Button buyHintButton;
    [SerializeField] private Button closeHintPopupButton;

    [Header("Settings & Data")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color wrongColor = Color.red;
    [SerializeField] private float animationDuration = 0.5f;

    public static event Action OnQuizPassed;
    public static event Action OnQuizFailed;

    private QuestionData currentQuestion;
    private bool isInputLocked = false;

    private void Start()
    {
        InitializeUI();
        SetupButtons();
    }

    private void InitializeUI()
    {
        mainPanelGroup.alpha = 0;
        mainPanelGroup.interactable = false;
        mainPanelGroup.blocksRaycasts = false;
        mainPanelRect.localScale = Vector3.zero;

        hintPopupGroup.alpha = 0;
        hintPopupGroup.interactable = false;
        hintPopupGroup.blocksRaycasts = false;
        hintPopupRect.localScale = Vector3.zero;

        hintTextDisplay.gameObject.SetActive(false);
    }

    private void SetupButtons()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
        }

        buyHintButton.onClick.AddListener(ConfirmBuyHint);
        closeHintPopupButton.onClick.AddListener(HideHintPopup);
    }

    public void ShowQuestion(QuestionData question)
    {
        currentQuestion = question;
        questionText.Text = question.questionText;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionTexts[i].Text = question.options[i];
            optionButtons[i].image.color = normalColor;
            optionButtons[i].transform.localScale = Vector3.one;
        }

        isInputLocked = false;
        hintTextDisplay.gameObject.SetActive(false);
        buyHintButton.gameObject.SetActive(true);

        mainPanelGroup.blocksRaycasts = true;
        mainPanelGroup.DOFade(1, animationDuration);
        mainPanelRect.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack).OnComplete(() =>
        {
            mainPanelGroup.interactable = true;
        });
    }

    private void OnOptionSelected(int index)
    {
        if (isInputLocked) return;
        isInputLocked = true;

        Button selectedButton = optionButtons[index];
        bool isCorrect = (index == currentQuestion.correctAnswerIndex);

        if (isCorrect)
        {
            AnimateWin(selectedButton);
        }
        else
        {
            AnimateLose(selectedButton);
        }
    }

    #region Animations & Game Flow

    private void AnimateWin(Button correctButton)
    {
        correctButton.image.DOColor(correctColor, 0.3f);
        correctButton.transform.DOScale(1.1f, 0.3f).SetLoops(3, LoopType.Yoyo).OnComplete(() =>
        {
            HideMainPanel(() =>
            {
                OnQuizPassed?.Invoke();
                GameManager.Instance.TriggerWin();
            });
        });
    }

    private void AnimateLose(Button wrongButton)
    {
        wrongButton.image.DOColor(wrongColor, 0.3f);
        wrongButton.GetComponent<RectTransform>().DOShakeAnchorPos(0.5f, new Vector2(15, 0), 10, 90, false, true).OnComplete(() =>
        {
            HideMainPanel(() =>
            {
                OnQuizFailed?.Invoke();
                GameManager.Instance.TriggerDeath();
            });
        });
    }

    private void HideMainPanel(TweenCallback onCompleteAction)
    {
        mainPanelGroup.interactable = false;
        mainPanelGroup.DOFade(0, animationDuration);
        mainPanelRect.DOScale(Vector3.zero, animationDuration).SetEase(Ease.InBack).OnComplete(onCompleteAction);
    }

    #endregion

    #region Hint System

    public void OnHintButtonClicked()
    {
        if (isInputLocked) return;
        ShowHintPopup();
    }

    private void ShowHintPopup()
    {
        hintPopupGroup.blocksRaycasts = true;
        hintPopupGroup.DOFade(1, 0.3f);
        hintPopupRect.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            hintPopupGroup.interactable = true;
        });
    }

    private void HideHintPopup()
    {
        hintPopupGroup.interactable = false;
        hintPopupGroup.DOFade(0, 0.3f);
        hintPopupRect.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            hintPopupGroup.blocksRaycasts = false;
        });
    }

    private void ConfirmBuyHint()
    {
        Debug.Log($"Attempting to buy hint for {currentQuestion.hintCost} coins.");
        Debug.Log($"Player has {PlayerDataManager.Instance.GetCoins()} coins.");
        bool success = PlayerDataManager.Instance.SpendCoins(currentQuestion.hintCost);

        if (success)
        {
            buyHintButton.gameObject.SetActive(false);
            hintTextDisplay.Text = currentQuestion.hintText;

            hintTextDisplay.gameObject.SetActive(true);
            hintTextDisplay.canvasRenderer.SetAlpha(0);
            hintTextDisplay.CrossFadeAlpha(1, 0.5f, false);
        }
        else
        {
            buyHintButton.GetComponent<RectTransform>().DOShakeAnchorPos(0.4f, new Vector2(10, 0), 20);
        }
    }

    #endregion
}