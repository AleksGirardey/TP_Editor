using System;
using UnityEditor;
using UnityEngine;

public class CameraPreviewWindow : EditorWindow {
    private Camera _camera;
    private RenderTexture _renderTexture;
    
    [MenuItem("Toolbox/CameraPreview")]
    private static void InitWindow() {
        EditorWindow editorWindow = GetWindow<CameraPreviewWindow>();
        editorWindow.autoRepaintOnSceneChange = true;
        editorWindow.Show();
        editorWindow.titleContent = new GUIContent("Camera Preview");
    }

    private void Awake() {
        _CreateRenderTexture();
    }

    private void Update() {
        if (!_camera) {
            _camera = Camera.main;
        }

        if (_camera == null) return;
        
        if (!_renderTexture) _CreateRenderTexture();

        RenderTexture tmpCameraTargetTexture = _camera.targetTexture;
        _camera.targetTexture = _renderTexture;
        _camera.Render();
        _camera.targetTexture = tmpCameraTargetTexture;
        
        if (Math.Abs(_renderTexture.width - position.width) > 0f ||
            Math.Abs(_renderTexture.height - position.height) > 0f)
            _CreateRenderTexture();
    }

    private void OnSelectionChange() {
        GameObject selectedGameObject = Selection.activeGameObject;
        if (!selectedGameObject) return;
        
        Camera cameraInsideSelection = selectedGameObject.GetComponentInChildren<Camera>();
        if (cameraInsideSelection)
            _camera = cameraInsideSelection;
    }

    private void OnGUI() {
        if (_renderTexture) {
            GUI.DrawTexture(new Rect(0, 0, position.width, position.height), _renderTexture);
        }
    }

    private void _CreateRenderTexture() {
        _renderTexture = new RenderTexture((int) position.width, (int) position.height, 24, RenderTextureFormat.ARGB32);
    }
}