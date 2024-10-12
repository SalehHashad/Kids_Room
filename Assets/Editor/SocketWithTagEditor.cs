using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.XR.Interaction.Toolkit;
using UnityEngine;
[CustomEditor(typeof(SocketWithTagCheck))]
public class SocketWithTagEditor : XRSocketInteractorEditor
{
    SerializedProperty targetTag = null;
    SerializedProperty IsPlayAudio;
    SerializedProperty _audioClip = null;
    SerializedProperty partDescription = null;
    SerializedProperty IsCorrect;
    protected override void OnEnable()
    {
        base.OnEnable();
        targetTag = serializedObject.FindPropertyOrFail("targetTag");
        IsPlayAudio = serializedObject.FindPropertyOrFail("IsPlayAudio");
        _audioClip = serializedObject.FindPropertyOrFail("_audioClip");
        partDescription = serializedObject.FindPropertyOrFail("partDescription");
        IsCorrect = serializedObject.FindPropertyOrFail("IsAssignedScore");
    }
    protected override void DrawProperties()
    {
        base.DrawProperties();
        EditorGUILayout.PropertyField(targetTag);
        EditorGUILayout.PropertyField(IsPlayAudio);
        EditorGUILayout.PropertyField(_audioClip);
        EditorGUILayout.PropertyField(partDescription);
        EditorGUILayout.PropertyField(IsCorrect);
    }
}
