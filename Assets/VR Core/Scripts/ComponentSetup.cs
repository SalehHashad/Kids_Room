using UnityEngine;
using System.Collections.Generic;
using Oculus.Interaction;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ComponentSetup : MonoBehaviour
{
#if UNITY_EDITOR
    private void AddTagToUnity(string tagName)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        // Check if tag already exists
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

        // Add new tag if it doesn't exist
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

    [ContextMenu("Add Components To Children")]
    private void AddComponentsToChildren()
    {
        foreach (Transform child in transform)
        {
            // First, add the tag to Unity's tag system


            // Set the tag on the child GameObject
            child.gameObject.tag = child.name;

            // Add Rigidbody
            Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = child.gameObject.AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.isKinematic = true;
            }

            // Add BoxCollider
            Collider collider = child.gameObject.GetComponent<Collider>();
            if (collider == null)
            {
                collider = child.gameObject.AddComponent<BoxCollider>();
                collider.isTrigger = true;
            }

            // Create and setup snap child
            GameObject snapChild = new GameObject(child.name);
            snapChild.transform.SetParent(child);
            snapChild.transform.localPosition = Vector3.zero;
            snapChild.transform.localRotation = Quaternion.identity;
            snapChild.transform.localScale = Vector3.one;
#if UNITY_EDITOR
            AddTagToUnity(snapChild.name);
#endif
            snapChild.tag = snapChild.name;
            // Add and setup CustomSnapPoint
            CustomSnapPoint snapPoint = snapChild.AddComponent<CustomSnapPoint>();
            snapPoint.expectedObjectTag = child.name;
        }

        Debug.Log("Components and tags added to all children!");
    }
}