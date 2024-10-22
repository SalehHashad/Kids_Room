using Oculus.Interaction;
using UnityEngine;

public class CustomSnapPoint : SnapInteractable
{
    public string expectedObjectTag; // The tag of the expected object.
    [SerializeField] private LevelSnapManager levelManager; // Reference to your LevelSnapManager.

    
    private void Reset()
    {
        levelManager = FindObjectOfType<LevelSnapManager>();
    }
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
            //InteractorRemoved(interactor);
            levelManager.HandleFailedAnswer();
        }
    }

    public void CheckIsIt(SnapInteractor interactor)
    {
        if(interactor.tag == "Umberlla_Stand")
        {
            Debug.Log(interactor.name);
            FindObjectOfType<Umberrlla_Tag>(true).gameObject.SetActive(true);
        }
    }
     
}
