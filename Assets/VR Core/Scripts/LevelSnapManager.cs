using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class LevelSnapManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip successAudioClip;
    [SerializeField] private AudioClip failedAudioClip;
    [SerializeField] private AudioClip PassAudioClip;

    [Header("Score Settings")]
    [SerializeField] private int totalScore = 0;
    [SerializeField] private int requiredScore = 0;
    [SerializeField] private TextMeshProUGUI ScoreTMP;
    [SerializeField] List<ModelsData> modelsData = new List<ModelsData>();

    [Header("Image View Settings")]
    [SerializeField] private Image referenceImage;
    [SerializeField] private TextMeshProUGUI viewsRemainingText;
    public int remainingViews;
    private float viewDuration;
    private bool isImageVisible = false;
    private Coroutine hideImageCoroutine;

    private ShowImage_Tag showImage;
    private ScoreButton_Tag showScoreButton;

    public UnityEvent onLevelComplete;
    public UnityEvent<int> onScoreUpdated;


    private void Awake()
    {
        InitializeObjects();
    }

    private void InitializeObjects()
    {
        showImage = FindObjectOfType<ShowImage_Tag>(true);
        showScoreButton = FindObjectOfType<ScoreButton_Tag>(true);
    }
    private void Start()
    {
        InitializeLevelSettings();
        ScoreTMP.text = totalScore.ToString();

        if (referenceImage != null)
        {
            referenceImage.gameObject.SetActive(false);
        }
        UpdateViewsText();
    }
    private void InitializeLevelSettings()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;

        switch (currentLevel)
        {
            case 0:
                requiredScore = 6;
                remainingViews = 4;
                viewDuration = 10f;
                break;
            case 1:
                // here you can also add the audio intro for theis level like "This is level two you need to grab 24 object and so on ..."
                requiredScore = 18;
                remainingViews = 3;
                viewDuration = 8f;
                break;
            case 2:
                requiredScore = 27;
                remainingViews = 2;
                viewDuration = 6f;
                break;
            case 3:
                requiredScore = 51;
                remainingViews = 1;
                viewDuration = 5f;
                break;
            default:
                requiredScore = 0;
                remainingViews = 0;
                viewDuration = 0f;
                break;
        }
    }

    public void ShowReferenceImage()
    {
        if (remainingViews > 0 && !isImageVisible && referenceImage != null)
        {
            showImage.gameObject.SetActive(false);
            showScoreButton.gameObject.SetActive(false);
            isImageVisible = true;
            remainingViews--;
            UpdateViewsText();

            referenceImage.gameObject.SetActive(true);

            if (hideImageCoroutine != null)
            {
                StopCoroutine(hideImageCoroutine);
            }

            hideImageCoroutine = StartCoroutine(HideImageAfterDelay());
        }
    }

    private IEnumerator HideImageAfterDelay()
    {
        yield return new WaitForSeconds(viewDuration);
        if (referenceImage != null)
        {
            referenceImage.gameObject.SetActive(false);
            showImage.gameObject.SetActive(true);
            showScoreButton.gameObject.SetActive(true);
        }
        isImageVisible = false;
    }

    private void UpdateViewsText()
    {
        if (viewsRemainingText != null)
        {
            viewsRemainingText.text = $"Remaining Views: {remainingViews}";
        }
    }

    public void HandleCorrectSnap(string objectTag)
    {
        var snapPoint = modelsData.Find(sp => sp.ObjetcTag == objectTag && !sp.IsMatched);

        if (snapPoint != null)
        {
            if (successAudioClip != null && audioSource != null)
            {
                audioSource.PlayOneShot(successAudioClip);
            }

            totalScore += 1;
            snapPoint.IsMatched = true;
            onScoreUpdated?.Invoke(totalScore);
            UpdateScoreUI();
            Debug.Log("Your score is : " + totalScore);
            CheckLevelCompletion();
        }
        else
        {
            Debug.Log("No matching snap point found for object tag: " + objectTag);
        }
    }

    public void HandleFailedAnswer()
    {
        audioSource.PlayOneShot(failedAudioClip);
    }

    private void CheckLevelCompletion()
    {
        if (totalScore >= requiredScore)
        {
            onLevelComplete?.Invoke();
            Debug.Log("You have pass this level ");
            StartCoroutine(PassTheLevel());
        }
    }

    IEnumerator PassTheLevel()
    {
        yield return new WaitForSeconds(successAudioClip.length);
        audioSource.PlayOneShot(PassAudioClip);
        yield return new WaitForSeconds(PassAudioClip.length);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void UpdateScoreUI()
    {
        ScoreTMP.text = totalScore.ToString();
    }
}