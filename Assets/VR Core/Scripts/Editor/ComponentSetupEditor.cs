//#if UNITY_EDITOR
//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(ComponentSetup))]
//public class ComponentSetupEditor : Editor
//{
//    public static void AddTagToUnity(string tagName)
//    {
//        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
//        SerializedProperty tagsProp = tagManager.FindProperty("tags");
//        bool found = false;
//        for (int i = 0; i < tagsProp.arraySize; i++)
//        {
//            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
//            if (t.stringValue.Equals(tagName))
//            {
//                found = true;
//                break;
//            }
//        }
//        if (!found)
//        {
//            tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
//            SerializedProperty newTagProp = tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1);
//            newTagProp.stringValue = tagName;
//            tagManager.ApplyModifiedProperties();
//            Debug.Log($"Added new tag: {tagName}");
//        }
//    }

//    public static void ClearModelsDataList(LevelSnapManager levelSnapManager)
//    {
//        if (levelSnapManager != null)
//        {
//            SerializedObject serializedManager = new SerializedObject(levelSnapManager);
//            SerializedProperty modelsDataProp = serializedManager.FindProperty("modelsData");
//            modelsDataProp.ClearArray();
//            serializedManager.ApplyModifiedProperties();
//        }
//    }

//    public static void AddModelData(LevelSnapManager levelSnapManager, string objectTag, bool isMatched)
//    {
//        if (levelSnapManager != null)
//        {
//            SerializedObject serializedManager = new SerializedObject(levelSnapManager);
//            SerializedProperty modelsDataProp = serializedManager.FindProperty("modelsData");

//            modelsDataProp.InsertArrayElementAtIndex(modelsDataProp.arraySize);
//            SerializedProperty element = modelsDataProp.GetArrayElementAtIndex(modelsDataProp.arraySize - 1);
//            SerializedProperty tagProp = element.FindPropertyRelative("ObjetcTag");
//            SerializedProperty matchedProp = element.FindPropertyRelative("IsMatched");

//            tagProp.stringValue = objectTag;
//            matchedProp.boolValue = isMatched;

//            serializedManager.ApplyModifiedProperties();
//        }
//    }

//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();
//        ComponentSetup setup = (ComponentSetup)target;

//        if (GUILayout.Button("Print ModelsData"))
//        {
//            PrintModelsData(setup);
//        }
//    }

//    private void PrintModelsData(ComponentSetup setup)
//    {
//        var levelSnapManager = setup.GetComponent<LevelSnapManager>();
//        if (levelSnapManager != null)
//        {
//            SerializedObject serializedManager = new SerializedObject(levelSnapManager);
//            SerializedProperty modelsDataProp = serializedManager.FindProperty("modelsData");
//            for (int i = 0; i < modelsDataProp.arraySize; i++)
//            {
//                SerializedProperty element = modelsDataProp.GetArrayElementAtIndex(i);
//                SerializedProperty tagProp = element.FindPropertyRelative("ObjetcTag");
//                SerializedProperty matchedProp = element.FindPropertyRelative("IsMatched");
//                Debug.Log($"Index {i}: Tag = {tagProp.stringValue}, Matched = {matchedProp.boolValue}");
//            }
//        }
//    }
//}
//#endif