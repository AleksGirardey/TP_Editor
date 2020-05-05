using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomEditor(typeof(ActionsSequencer))]
public class ActionsSequencerInspector : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate")) {
            GenerateActions();
        }
    }

    private void GenerateActions() {
        ActionsSequencer actionsSequencer = target as ActionsSequencer;

        if (actionsSequencer == null) return;

        AnimatorController controller = actionsSequencer.controller as AnimatorController;

        if (controller == null) return;

        while (actionsSequencer.transform.childCount > 0) {
            DestroyImmediate(actionsSequencer.transform.GetChild(0).gameObject);
        }
        
        AnimatorControllerLayer mainLayer = controller.layers[0];

        AnimatorState state = mainLayer.stateMachine.defaultState;
        GameObject stateParentGameObject = new GameObject(state.name);
        stateParentGameObject.transform.parent = actionsSequencer.transform;
        while (state.transitions.Length > 0) {
            AnimatorState subState = state.transitions[0].destinationState;
            GameObject subStateGameObject = new GameObject(subState.name);
            subStateGameObject.transform.parent = stateParentGameObject.transform;
            state = subState;
            stateParentGameObject = subStateGameObject;
        }
    }
}