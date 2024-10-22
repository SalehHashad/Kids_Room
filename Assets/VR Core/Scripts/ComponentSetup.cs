using UnityEngine;
using System.Collections.Generic;
using Oculus.Interaction;

public class ComponentSetup : MonoBehaviour
{
    [SerializeField] private LevelSnapManager levelSnapManager;
    [SerializeField] private bool setupComplete;

    private void OnValidate()
    {
        if (levelSnapManager == null)
        {
            levelSnapManager = FindObjectOfType<LevelSnapManager>();
        }
    }

    private bool HasRequiredSetup(Transform child)
    {
        if (child.gameObject.tag != child.name) return false;
        if (!child.GetComponent<Rigidbody>()) return false;
        if (!child.GetComponent<Collider>()) return false;
        Transform snapChild = child.Find(child.name);
        if (!snapChild) return false;
        if (snapChild.gameObject.tag != snapChild.name) return false;
        CustomSnapPoint snapPoint = snapChild.GetComponent<CustomSnapPoint>();
        if (!snapPoint) return false;
        if (snapPoint.expectedObjectTag != child.name) return false;
        return true;
    }

#if UNITY_EDITOR
    private void ClearModelsDataList()
    {
        if (levelSnapManager != null)
        {
            UnityEditor.SerializedObject serializedManager = new UnityEditor.SerializedObject(levelSnapManager);
            UnityEditor.SerializedProperty modelsDataProp = serializedManager.FindProperty("modelsData");
            modelsDataProp.ClearArray();
            serializedManager.ApplyModifiedProperties();
        }
    }

    private void AddModelData(string objectTag, bool isMatched)
    {
        if (levelSnapManager != null)
        {
            UnityEditor.SerializedObject serializedManager = new UnityEditor.SerializedObject(levelSnapManager);
            UnityEditor.SerializedProperty modelsDataProp = serializedManager.FindProperty("modelsData");

            modelsDataProp.InsertArrayElementAtIndex(modelsDataProp.arraySize);
            UnityEditor.SerializedProperty element = modelsDataProp.GetArrayElementAtIndex(modelsDataProp.arraySize - 1);
            UnityEditor.SerializedProperty tagProp = element.FindPropertyRelative("ObjetcTag");
            UnityEditor.SerializedProperty matchedProp = element.FindPropertyRelative("IsMatched");

            tagProp.stringValue = objectTag;
            matchedProp.boolValue = isMatched;

            serializedManager.ApplyModifiedProperties();
        }
    }

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

    [ContextMenu("Add Components To Children")]
    private void AddComponentsToChildren()
    {
        if (levelSnapManager == null)
        {
            Debug.LogError("Please assign LevelSnapManager in the inspector!");
            return;
        }

#if UNITY_EDITOR
        ClearModelsDataList();
#endif
        bool componentsAdded = false;

        foreach (Transform child in transform)
        {
            if (HasRequiredSetup(child))
            {
#if UNITY_EDITOR
                AddModelData(child.name, false);
#endif
                continue;
            }

            componentsAdded = true;

            child.gameObject.tag = child.name;

            Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = child.gameObject.AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.isKinematic = true;
            }

            Collider collider = child.gameObject.GetComponent<Collider>();
            if (collider == null)
            {
                collider = child.gameObject.AddComponent<BoxCollider>();
                collider.isTrigger = true;
            }

            Transform existingSnapChild = child.Find(child.name);
            GameObject snapChild;

            if (existingSnapChild == null)
            {
                snapChild = new GameObject(child.name);
                snapChild.transform.SetParent(child);
                snapChild.transform.localPosition = Vector3.zero;
                snapChild.transform.localRotation = Quaternion.identity;
                snapChild.transform.localScale = Vector3.one;
            }
            else
            {
                snapChild = existingSnapChild.gameObject;
            }

#if UNITY_EDITOR
            AddTagToUnity(snapChild.name);
#endif
            snapChild.tag = snapChild.name;

            CustomSnapPoint snapPoint = snapChild.GetComponent<CustomSnapPoint>();
            if (snapPoint == null)
            {
                snapPoint = snapChild.AddComponent<CustomSnapPoint>();
                snapPoint.expectedObjectTag = child.name;
                snapPoint.MaxSelectingInteractors = 1;
            }

#if UNITY_EDITOR
            AddModelData(child.name, false);
#endif
        }

        if (componentsAdded)
        {
            Debug.Log("Components, tags, and ModelsData entries added to new children!");
        }
        else
        {
            Debug.Log("All children already have the required setup!");
        }
    }

    [ContextMenu("Print ModelsData")]
    private void PrintModelsData()
    {
#if UNITY_EDITOR
        if (levelSnapManager != null)
        {
            UnityEditor.SerializedObject serializedManager = new UnityEditor.SerializedObject(levelSnapManager);
            UnityEditor.SerializedProperty modelsDataProp = serializedManager.FindProperty("modelsData");
            for (int i = 0; i < modelsDataProp.arraySize; i++)
            {
                UnityEditor.SerializedProperty element = modelsDataProp.GetArrayElementAtIndex(i);
                UnityEditor.SerializedProperty tagProp = element.FindPropertyRelative("ObjetcTag");
                UnityEditor.SerializedProperty matchedProp = element.FindPropertyRelative("IsMatched");
                Debug.Log($"Index {i}: Tag = {tagProp.stringValue}, Matched = {matchedProp.boolValue}");
            }
        }
#endif
    }
}