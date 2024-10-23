using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

public class GrabComponentSetup : MonoBehaviour
{
    [SerializeField] private LevelSnapManager levelSnapManager;

    private void OnValidate()
    {
        if (levelSnapManager == null)
        {
            levelSnapManager = FindObjectOfType<LevelSnapManager>();
        }
    }

    private bool HasRequiredSetup(Transform child)
    {
        Transform snapInteractorChild = child.Find("SnapInteractor");
        if (!snapInteractorChild) return false;
        if (!snapInteractorChild.GetComponent<SnapInteractor>()) return false;
        return true;
    }

#if UNITY_EDITOR
    private void AddTagToUnity(string tagName)
    {
        UnityEditor.SerializedObject tagManager = new UnityEditor.SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        UnityEditor.SerializedProperty tagsProp = tagManager.FindProperty("tags");
        bool found = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            UnityEditor.SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(tagName))
            {
                found = true;
                break;
            }
        }
        if (!found)
        {
            tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
            UnityEditor.SerializedProperty newTagProp = tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1);
            newTagProp.stringValue = tagName;
            tagManager.ApplyModifiedProperties();
            Debug.Log($"Added new tag: {tagName}");
        }
    }
#endif

    [ContextMenu("Add Grab Components To Children")]
    private void AddGrabComponentsToChildren()
    {
        if (levelSnapManager == null)
        {
            Debug.LogError("Please assign LevelSnapManager in the inspector!");
            return;
        }

        bool componentsAdded = false;

        foreach (Transform child in transform)
        {
            if (HasRequiredSetup(child))
            {
                continue;
            }

            componentsAdded = true;

            Transform snapInteractorTransform = child.Find("SnapInteractor");
            GameObject snapInteractorObj;

            if (snapInteractorTransform == null)
            {
                snapInteractorObj = new GameObject("SnapInteractor");
                snapInteractorObj.transform.SetParent(child);
                snapInteractorObj.transform.localPosition = Vector3.zero;
                snapInteractorObj.transform.localRotation = Quaternion.identity;
                snapInteractorObj.transform.localScale = Vector3.one;
            }
            else
            {
                snapInteractorObj = snapInteractorTransform.gameObject;
            }

            // Add SnapInteractor component if it doesn't exist
            SnapInteractor snapInteractor = snapInteractorObj.GetComponent<SnapInteractor>();
            if (snapInteractor == null)
            {
                snapInteractor = snapInteractorObj.AddComponent<SnapInteractor>();
            }

            // Get the tag from ComponentSetup and apply it
            ComponentSetup componentSetup = FindObjectOfType<ComponentSetup>();
            if (componentSetup != null)
            {
                string targetTag = child.name;
#if UNITY_EDITOR
                AddTagToUnity(targetTag);
#endif
                snapInteractorObj.tag = targetTag;
            }
        }

        if (componentsAdded)
        {
            Debug.Log("Grab components and SnapInteractor children added to new children!");
        }
        else
        {
            Debug.Log("All children already have the required grab setup!");
        }
    }

    [ContextMenu("Print Child Components")]
    private void PrintChildComponents()
    {
        foreach (Transform child in transform)
        {
            Transform snapInteractorChild = child.Find("SnapInteractor");
            if (snapInteractorChild != null)
            {
                Debug.Log($"Has SnapInteractor Child: true");
                Debug.Log($"SnapInteractor has SnapInteractor component: {snapInteractorChild.GetComponent<SnapInteractor>() != null}");
                Debug.Log($"SnapInteractor tag: {snapInteractorChild.tag}");
            }
            else
            {
                Debug.Log("Has SnapInteractor Child: false");
            }
            Debug.Log("------------------------");
        }
    }
}