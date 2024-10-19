using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class LevelSnapManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip successAudioClip;
    [SerializeField] private AudioClip failedAudioClip;
    [SerializeField] private AudioClip PassAudioClip;
    [SerializeField] private int totalScore = 0;
    [SerializeField] private int requiredScore = 0;
    [SerializeField] List<ModelsData> modelsData = new List<ModelsData>();

    public UnityEvent onLevelComplete;
    public UnityEvent<int> onScoreUpdated;

    private void Start()
    {
        InitializeLevel();
    }
    private void InitializeLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        requiredScore = currentLevel switch
        {
            1 => 2,
            2 => 30,
            3 => 40,
            4 => 50


        };
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
}
