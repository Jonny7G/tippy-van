using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldGenerator))]
public class WorldGeneratorEditor : Editor
{
    private WorldGenerator generator;
    private SerializedProperty _maxY;
    private SerializedProperty _directionSwitchChance;

    private SerializedProperty _direction;
    private SerializedProperty _worldSpeed;
    private SerializedProperty _gameActive;

    private SerializedProperty _OnWorldMove;
    private SerializedProperty _startArea;
    private SerializedProperty _leftRoad;
    private SerializedProperty _rightRoad;
    private SerializedProperty _leftTurnRoad;
    private SerializedProperty _rightTurnRoad;


    private SerializedProperty _specialsCooldownMin;
    private SerializedProperty _speciaCooldownMax;

    private SerializedProperty _leftWoodBridge;
    private SerializedProperty _rightWoodBridge;

    private SerializedProperty _leftLogBridge;
    private SerializedProperty _rightLogBridge;

    private SerializedProperty _leftBrokenBridge;
    private SerializedProperty _rightBrokenBridge;

    private GUIContent _toggleButtonStyleNormal = null;
    private GUIContent _toggleButtonStyleToggled = null
        ;
    private GUIStyle _toggleButtonToggledStyle;
    private GUIStyle _toggleButtonNormalStyle;

    private int tab=0;
    private bool left=true;
    private bool lastLeft;
    private void OnEnable()
    {
        generator = (WorldGenerator)target;
        lastLeft = left;
        _maxY = serializedObject.FindProperty("maxY");
        _directionSwitchChance = serializedObject.FindProperty("directionSwitchChance");
        _direction = serializedObject.FindProperty("direction");
        _worldSpeed = serializedObject.FindProperty("worldSpeed");
        _gameActive = serializedObject.FindProperty("gameActive");
        _OnWorldMove = serializedObject.FindProperty("OnWorldMove");

        _startArea = serializedObject.FindProperty("startArea");
        _leftRoad = serializedObject.FindProperty("leftRoad");
        _rightRoad = serializedObject.FindProperty("rightRoad");
        _leftTurnRoad = serializedObject.FindProperty("leftTurnRoad");
        _rightTurnRoad = serializedObject.FindProperty("rightTurnRoad");

        _specialsCooldownMin = serializedObject.FindProperty("specialsCooldownMin");
        _speciaCooldownMax = serializedObject.FindProperty("speciaCooldownMax");
        _leftWoodBridge = serializedObject.FindProperty("leftWoodBridge");
        _rightWoodBridge = serializedObject.FindProperty("rightWoodBridge");
        _leftLogBridge = serializedObject.FindProperty("leftLogBridge");
        _rightLogBridge = serializedObject.FindProperty("rightLogBridge");
        _leftBrokenBridge = serializedObject.FindProperty("leftBrokenBridge");
        _rightBrokenBridge = serializedObject.FindProperty("rightBrokenBridge");

      
    }
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        tab = GUILayout.Toolbar(tab, new string[] { "fields", "references","normal tracks","special tracks","Debugging" });

        switch (tab)
        {
            case 0:
                GUILayout.Space(10);
                EditorGUILayout.PropertyField(_maxY);
                EditorGUILayout.Slider(_directionSwitchChance, 0, 1);
                GUILayout.Space(20);
                EditorGUILayout.PropertyField(_specialsCooldownMin);
                EditorGUILayout.PropertyField(_speciaCooldownMax);
                break;
            case 1:

                EditorGUILayout.PropertyField(_direction);
                EditorGUILayout.PropertyField(_worldSpeed);
                EditorGUILayout.PropertyField(_gameActive);
                EditorGUILayout.PropertyField(_OnWorldMove);
                EditorGUILayout.PropertyField(_startArea);
                break;
            case 2:
                EditorGUILayout.PropertyField(_leftRoad);
                EditorGUILayout.PropertyField(_rightRoad);
                EditorGUILayout.PropertyField(_leftTurnRoad);
                EditorGUILayout.PropertyField(_rightTurnRoad);
                break;
            case 3:
                
                EditorGUILayout.PropertyField(_leftWoodBridge);
                EditorGUILayout.PropertyField(_rightWoodBridge);
                GUILayout.Space(20);
                EditorGUILayout.PropertyField(_leftLogBridge);
                EditorGUILayout.PropertyField(_rightLogBridge);
                GUILayout.Space(20);
                EditorGUILayout.PropertyField(_leftBrokenBridge);
                EditorGUILayout.PropertyField(_rightBrokenBridge);
                break;
            case 4:
                GUILayout.Space(10);
                
                generator.spawnNormally = GUILayout.Toggle(generator.spawnNormally, "Spawn normally", "Button");

                GUILayout.Space(20);

                left = GUILayout.Toggle(left, "isLeft", "Button");
                if (left != lastLeft)
                {
                    if (Application.IsPlaying(generator))
                    {
                        WorldDirection newDir=WorldDirection.left;

                        if (left)
                            newDir = WorldDirection.left;
                        else if (!left)
                            newDir = WorldDirection.right;

                        generator.SwitchDirection(newDir);
                        lastLeft = left;
                    }
                }

                GUILayout.Space(20);
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                if (GUILayout.Button("Spawn wood bridge"))
                {
                    if (Application.IsPlaying(generator))
                        generator.GenerateTrack(generator.GetKey(generator.leftWoodBridgeKey, generator.rightWoodBridgeKey));
                }
                GUILayout.Space(10);
                if (GUILayout.Button("Spawn log bridge"))
                {
                    if (Application.IsPlaying(generator))
                        generator.GenerateTrack(generator.GetKey(generator.leftLogBridgeKey, generator.rightLogBridgeKey));
                }
                GUILayout.Space(10);
                if (GUILayout.Button("Spawn broken bridge"))
                {
                    if (Application.IsPlaying(generator))
                        generator.GenerateTrack(generator.GetKey(generator.leftBrokenBridgeKey, generator.rightBrokenBridgeKey));
                }
                GUILayout.Space(10);
                GUILayout.EndHorizontal();
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }
}