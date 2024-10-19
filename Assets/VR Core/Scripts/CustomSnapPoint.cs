// This goes on the snap point (destination)
using Oculus.Interaction;
using UnityEngine;

public class CustomSnapPoint : SnapInteractable
{
    public string expectedObjectTag; // The tag of the expected object.
    [SerializeField] private LevelSnapManager levelManager; // Reference to your LevelSnapManager.


    //private void Awake()
    //{
    //    levelManager = FindObjectOfType<LevelSnapManager>();
    //}
    protected override void InteractorAdded(SnapInteractor interactor)
    {
        Debug.Log("the interactor GameObject tag is : " + interactor.gameObject.tag);
        if (interactor == null)
        {
            Debug.LogError("Interactor does not have a valid SnapInteractable.");
            return;
        }
        if (interactor.gameObject.tag == expectedObjectTag)
        {
            Debug.Log("The game object " + interactor.gameObject.name + " is snapped correctly.");
            levelManager.HandleCorrectSnap(expectedObjectTag);
        }
        else
        {
            Debug.LogError("Incorrect object snapped or object is untagged. Object tag: " + interactor.gameObject.tag);
            InteractorRemoved(interactor);
        }
    }

    protected override void InteractorRemoved(SnapInteractor interactor)
    {
        base.InteractorRemoved(interactor);
        levelManager.HandleFailedAnswer();
    }
}
