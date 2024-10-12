using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
[RequireComponent(typeof(AudioSource))]
public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem instance;

    [SerializeField] int score = 0;
    [SerializeField] int targetScore = 11;
    [SerializeField] bool congEachTime = false;
    [SerializeField] bool isScoreInTMP = false;
    [SerializeField] AudioClip audioClip;
    [SerializeField] TextMeshProUGUI descriptionTMP;

    AudioSource audioSource;
    private void Awake()
    {
        if(instance == null)
            instance = this;
    }
    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void AddObjectToSocket(SocketWithTagCheck socketWithTagCheck)
    {
        if (!socketWithTagCheck.IsAssignedScore)
        {
            score++;
            socketWithTagCheck.IsAssignedScore = true;
        }

        if (congEachTime)
        {
            audioSource.clip = audioClip;
            if (isScoreInTMP)
                descriptionTMP.text = score.ToString();
            audioSource.Play();
        }
        else
        {
                audioSource.clip = audioClip;
                audioSource.Play();
        }
        
        if (!isScoreInTMP)
            descriptionTMP.text = socketWithTagCheck.partDescription;
        if (socketWithTagCheck.IsPlayAudio)
        {
            audioSource.clip = socketWithTagCheck.audioClip;
            audioSource.Play();
        }
        if (score == targetScore)
        {
            print("Cong Win this Level");
            WinTheGame();
        }
     }
        
    public void RemoveObjectToSocket(SocketWithTagCheck socketWithTagCheck)
    {
        if (socketWithTagCheck.IsAssignedScore)
        {
            score--;
            if (isScoreInTMP)
                descriptionTMP.text = score.ToString();
            socketWithTagCheck.IsAssignedScore = false;
        }
    }

    void WinTheGame()
    {
        descriptionTMP.text = "congratulations you won this level";
    }
}
