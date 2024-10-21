using UnityEngine;
using System.Collections.Generic;
using Oculus.Interaction;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ComponentSetup : MonoBehaviour
{
    [SerializeField] private LevelSnapManager levelSnapManager;

    private void OnValidate()
    {
        if (levelSnapManager == null)
        {
            levelSnapManager = FindObjectOfType<LevelSnapManager>();
        }
    }

#if UNITY_EDITOR
    private void AddTagToUnity(string tagName)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");
        bool found = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(tagName))
            {
                found = true;
                break;
            }
        }
        if (!found)
        {
            tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
            SerializedProperty newTagProp = tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1);
            newTagProp.stringValue = tagName;
            tagManager.ApplyModifiedProperties();
            Debug.Log($"Added new tag: {tagName}");
        }
    }
#endif

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

    [ContextMenu("Add Components To Children")]
    private void AddComponentsToChildren()
    {
        if (levelSnapManager == null)
        {
            Debug.LogError("Please assign LevelSnapManager in the inspector!");
            return;
        }
        ClearModelsDataList();
        bool componentsAdded = false;
        
        foreach (Transform child in transform)
        {
            if (HasRequiredSetup(child))
            {
                AddModelData(child.name, false);
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
            AddModelData(child.name, false);
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
    private void ClearModelsDataList()
    {
        if (levelSnapManager != null)
        {
            SerializedObject serializedManager = new SerializedObject(levelSnapManager);
            SerializedProperty modelsDataProp = serializedManager.FindProperty("modelsData");
            modelsDataProp.ClearArray();
            serializedManager.ApplyModifiedProperties();
        }
    }
    private void AddModelData(string objectTag, bool isMatched)
    {
        if (levelSnapManager != null)
        {
            SerializedObject serializedManager = new SerializedObject(levelSnapManager);
            SerializedProperty modelsDataProp = serializedManager.FindProperty("modelsData");

            modelsDataProp.InsertArrayElementAtIndex(modelsDataProp.arraySize);
            SerializedProperty element = modelsDataProp.GetArrayElementAtIndex(modelsDataProp.arraySize - 1);
            SerializedProperty tagProp = element.FindPropertyRelative("ObjetcTag");
            SerializedProperty matchedProp = element.FindPropertyRelative("IsMatched");

            tagProp.stringValue = objectTag;
            matchedProp.boolValue = isMatched;

            serializedManager.ApplyModifiedProperties();
        }
    }

    [ContextMenu("Print ModelsData")]
    private void PrintModelsData()
    {
        if (levelSnapManager != null)
        {
            SerializedObject serializedManager = new SerializedObject(levelSnapManager);
            SerializedProperty modelsDataProp = serializedManager.FindProperty("modelsData");
            for (int i = 0; i < modelsDataProp.arraySize; i++)
            {
                SerializedProperty element = modelsDataProp.GetArrayElementAtIndex(i);
                SerializedProperty tagProp = element.FindPropertyRelative("ObjetcTag");
                SerializedProperty matchedProp = element.FindPropertyRelative("IsMatched");
            }
        }
    }
}