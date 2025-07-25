using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

namespace GolemKinGames.Vacumn
{
    [CustomEditor(typeof(VacuumSystem))]
    public class VacuumSystemEditor : Editor
    {
        private SerializedProperty vacuumDimension;
        private SerializedProperty affectedLayers;
        private SerializedProperty affectedTags;
        private SerializedProperty vacuumShape;
        private SerializedProperty vacuumMode;
        private SerializedProperty forceDirectionMode;
        private SerializedProperty vacuumEffect;
        private SerializedProperty shrinkCurve;
        private SerializedProperty endAction;
        private SerializedProperty forceCurve;
        private SerializedProperty maxForce;
        private SerializedProperty minForce;
        private SerializedProperty maxRange;
        private SerializedProperty pulseSpeed;
        private SerializedProperty pulseIntensity;
        private SerializedProperty vacuumPoint;
        private SerializedProperty collectionDistance;
        private SerializedProperty triggerOnStart;
        private SerializedProperty obstacleLayers;
        private SerializedProperty velocityAffectsForce;
        private SerializedProperty drawGizmos;
        private SerializedProperty vacuumParticles;
        private SerializedProperty vacuumSound;
        private SerializedProperty layerForceModifiers;

        private ReorderableList tagList;
        private ReorderableList layerModifierList;

        private void OnEnable()
        {
            // Load properties
            vacuumDimension = serializedObject.FindProperty("vacuumDimension");  // 2D/3D mode selector
            affectedLayers = serializedObject.FindProperty("affectedLayers");
            affectedTags = serializedObject.FindProperty("affectedTags");
            vacuumShape = serializedObject.FindProperty("vacuumShape");
            vacuumMode = serializedObject.FindProperty("vacuumMode");
            forceDirectionMode = serializedObject.FindProperty("forceDirectionMode");
            vacuumEffect = serializedObject.FindProperty("vacuumEffect");
            shrinkCurve = serializedObject.FindProperty("shrinkCurve");
            endAction = serializedObject.FindProperty("endAction");
            forceCurve = serializedObject.FindProperty("forceCurve");
            maxForce = serializedObject.FindProperty("maxForce");
            minForce = serializedObject.FindProperty("minForce");
            maxRange = serializedObject.FindProperty("maxRange");
            pulseSpeed = serializedObject.FindProperty("pulseSpeed");
            pulseIntensity = serializedObject.FindProperty("pulseIntensity");
            vacuumPoint = serializedObject.FindProperty("vacuumPoint");
            collectionDistance = serializedObject.FindProperty("collectionDistance");
            triggerOnStart = serializedObject.FindProperty("triggerOnStart");
            obstacleLayers = serializedObject.FindProperty("obstacleLayers");
            velocityAffectsForce = serializedObject.FindProperty("velocityAffectsForce");
            drawGizmos = serializedObject.FindProperty("drawGizmos");
            vacuumParticles = serializedObject.FindProperty("vacuumParticles");
            vacuumSound = serializedObject.FindProperty("vacuumSound");

            // Setup reorderable tag list
            tagList = new ReorderableList(serializedObject, affectedTags, true, true, true, true)
            {
                drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "Affected Tags");
                },
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = tagList.serializedProperty.GetArrayElementAtIndex(index);
                    rect.y += 2;
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                }
            };

            // Setup reorderable layer modifier list
            layerModifierList = new ReorderableList(serializedObject, layerForceModifiers, true, true, true, true)
            {
                drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "Layer Force Modifiers");
                },
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = layerModifierList.serializedProperty.GetArrayElementAtIndex(index);
                    rect.y += 2;

                    var layer = element.FindPropertyRelative("layer");
                    var forceMultiplier = element.FindPropertyRelative("forceMultiplier");

                    var width = rect.width / 2;
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, width - 5, EditorGUIUtility.singleLineHeight), layer, GUIContent.none);
                    EditorGUI.PropertyField(new Rect(rect.x + width + 5, rect.y, width - 5, EditorGUIUtility.singleLineHeight), forceMultiplier, GUIContent.none);
                }
            };
        }

        public override void OnInspectorGUI()
{
    DrawDefaultInspector();

    serializedObject.Update();

    // Fancy header
    EditorGUILayout.Space();
    EditorGUILayout.LabelField("Vacuum System", EditorStyles.boldLabel);
    EditorGUILayout.HelpBox("Configure the settings for the Vacuum System.", MessageType.Info);
    
    EditorGUILayout.Space();
    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);  // Divider line
    
    // Section: Vacuum Settings
    EditorGUILayout.LabelField("Vacuum Settings", EditorStyles.boldLabel);
    EditorGUILayout.PropertyField(vacuumDimension, new GUIContent("Dimension Mode"));  // 2D/3D mode
    EditorGUILayout.PropertyField(vacuumShape, new GUIContent("Vacuum Shape"));
    EditorGUILayout.PropertyField(vacuumMode, new GUIContent("Vacuum Mode"));
    EditorGUILayout.PropertyField(forceDirectionMode, new GUIContent("Force Direction Mode"));
    EditorGUILayout.PropertyField(triggerOnStart, new GUIContent("Trigger on Start"));

    EditorGUILayout.Space();
    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);  // Divider line

    // Section: Vacuum Effect
    EditorGUILayout.LabelField("Vacuum Effect", EditorStyles.boldLabel);
    EditorGUILayout.PropertyField(vacuumEffect, new GUIContent("Vacuum Effect"));
    if (vacuumEffect.enumValueIndex == (int)VacuumEffect.Shrink)
    {
        EditorGUILayout.PropertyField(shrinkCurve, new GUIContent("Shrink Curve"));
    }
    EditorGUILayout.PropertyField(endAction, new GUIContent("Object End Action"));

    EditorGUILayout.Space();
    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);  // Divider line

    // Section: Forces
    EditorGUILayout.LabelField("Forces", EditorStyles.boldLabel);
    EditorGUILayout.PropertyField(maxForce, new GUIContent("Max Force"));
    EditorGUILayout.PropertyField(minForce, new GUIContent("Min Force"));
    EditorGUILayout.PropertyField(maxRange, new GUIContent("Max Range"));
    EditorGUILayout.PropertyField(forceCurve, new GUIContent("Force Curve"));

    EditorGUILayout.Space();
    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);  // Divider line

    // Section: Dynamic Intensity
    EditorGUILayout.LabelField("Dynamic Intensity", EditorStyles.boldLabel);
    EditorGUILayout.PropertyField(pulseSpeed, new GUIContent("Pulse Speed"));
    EditorGUILayout.PropertyField(pulseIntensity, new GUIContent("Pulse Intensity"));

    EditorGUILayout.Space();
    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);  // Divider line

    // Section: Vacuum Point
    EditorGUILayout.LabelField("Vacuum Point", EditorStyles.boldLabel);
    EditorGUILayout.PropertyField(vacuumPoint, new GUIContent("Vacuum Point"));
    EditorGUILayout.PropertyField(collectionDistance, new GUIContent("Collection Distance"));

    EditorGUILayout.Space();
    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);  // Divider line

    // Section: Layer and Tag Filters
    EditorGUILayout.LabelField("Layer and Tag Filters", EditorStyles.boldLabel);
    EditorGUILayout.PropertyField(affectedLayers, new GUIContent("Affected Layers"));
    tagList.DoLayoutList();

    EditorGUILayout.Space();
    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);  // Divider line

    // Section: Velocity
    EditorGUILayout.LabelField("Velocity", EditorStyles.boldLabel);
    EditorGUILayout.PropertyField(velocityAffectsForce, new GUIContent("Velocity Affects Force"));

    EditorGUILayout.Space();
    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);  // Divider line

    // Section: Obstacles
    EditorGUILayout.LabelField("Obstacles", EditorStyles.boldLabel);
    EditorGUILayout.PropertyField(obstacleLayers, new GUIContent("Obstacle Layers"));

    EditorGUILayout.Space();
    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);  // Divider line

    // Section: Visual Feedback
    EditorGUILayout.LabelField("Visual Feedback", EditorStyles.boldLabel);
    EditorGUILayout.PropertyField(vacuumParticles, new GUIContent("Vacuum Particles"));
    EditorGUILayout.PropertyField(vacuumSound, new GUIContent("Vacuum Sound"));

    EditorGUILayout.Space();
    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);  // Divider line

    EditorGUILayout.PropertyField(drawGizmos, new GUIContent("Draw Gizmos"));

    serializedObject.ApplyModifiedProperties();
}


        private void OnSceneGUI()
        {
            VacuumSystem vacuumSystem = (VacuumSystem)target;

            if (vacuumSystem.drawGizmos && vacuumSystem.vacuumPoint != null)
            {
                Handles.color = Color.cyan;

                // Apply the vacuum point's local transform matrix to the Handles for drawing
                Handles.matrix = vacuumSystem.vacuumPoint.localToWorldMatrix;

                switch (vacuumSystem.vacuumShape)
                {
                    case VacuumShape.Cone:
                        Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.forward, vacuumSystem.maxRange, vacuumSystem.maxRange);
                        Handles.DrawLine(Vector3.zero, new Vector3(0, vacuumSystem.maxRange, vacuumSystem.maxRange));
                        break;

                    case VacuumShape.Box:
                        Handles.DrawWireCube(Vector3.zero, new Vector3(vacuumSystem.maxRange, vacuumSystem.maxRange, vacuumSystem.maxRange));
                        break;
                }

                Handles.matrix = Matrix4x4.identity;
            }
        }
    }
}
