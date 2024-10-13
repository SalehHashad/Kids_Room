using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
[RequireComponent(typeof(MeshCollider),typeof(Outline))]
public class SocketWithTagCheck : XRSocketInteractor
{
    public string targetTag = string.Empty;
    public bool IsPlayAudio = false;
    public AudioClip _audioClip = null;
    public string partDescription = null;
    public bool IsAssignedScore = false;
    private void Start()
    {

        this.onSelectEnter.AddListener(OnObjectSelected);
        this.onSelectExit.AddListener(OnObjectExit);
    }
    private void OnObjectSelected(XRBaseInteractable arg0)
    {
        ScoreSystem.instance.AddObjectToSocket(this);
    }
    private void OnObjectExit(XRBaseInteractable arg0)
    {
        ScoreSystem.instance.RemoveObjectToSocket(this);
    }

    

    public AudioClip audioClip { get => _audioClip; set => _audioClip = value; }
    public string PartDescription { get => partDescription; set => partDescription = value; }

    public override bool CanHover(XRBaseInteractable interactable)
    {
        print("Hovered");
        return base.CanHover(interactable) && MatchUsingTag(interactable);
    }
    public override bool CanSelect(XRBaseInteractable interactable)
    {
        print("Selected");
        return base.CanSelect(interactable) && MatchUsingTag(interactable);
    }
    bool MatchUsingTag(XRBaseInteractable interactable)
    {
        if (!string.IsNullOrEmpty(targetTag)) 
            return interactable.CompareTag(targetTag);
        else
            return false;
    }
}
