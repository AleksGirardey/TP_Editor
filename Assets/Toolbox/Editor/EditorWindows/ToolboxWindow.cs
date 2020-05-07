using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToolboxWindow : EditorWindow {

    private int _tabIndex;

    private Path2D[] _path2DOnScene;
    
    private static readonly string[] Tabs = {
        "General",
        "Path2D"
    };
    
    [MenuItem("Toolbox/Toolbox Window")]
    private static void InitWindow() {
        EditorWindow editorWindow = GetWindow<ToolboxWindow>();
        editorWindow.autoRepaintOnSceneChange = true;
        editorWindow.Show();
        editorWindow.titleContent = new GUIContent("Toolbox Window");
    }

    private void OnSelectionChange() {
        _path2DOnScene = _SelectAllPath2D();
    }

    private void OnGUI() {
        if (_path2DOnScene == null)
            _path2DOnScene = _SelectAllPath2D();
        
        _tabIndex = GUILayout.Toolbar(_tabIndex, Tabs);

        switch (_tabIndex) {
            case 0: _GUITabsGeneral(); break;
            case 1: _GUITabsPath2D(); break;
        }
    }

    private void OnHierarchyChange() {
        _path2DOnScene = _SelectAllPath2D();
    }

    #region GeneralTab
    
    private void _GUITabsGeneral() {
        GUILayout.Space(10f);
        if (GUILayout.Button("Select GameManager")) {
            _SelectGameManager();
        }
        if (GUILayout.Button("Select GameData")) {
            _SelectGameData();
        }
    }
    
    private void _SelectGameManager() {
        GameManager gameManager = _FindGameManagerInScene();
        
        if (!gameManager) return;
        
        Selection.activeGameObject = gameManager.gameObject;
        SceneView.lastActiveSceneView.FrameSelected();
        EditorGUIUtility.PingObject(gameManager.gameObject);
    }

    private void _SelectGameData() {
        GameManager gameManager = _FindGameManagerInScene();
        
        if (!gameManager) return;
        
        Selection.activeObject = gameManager.gameData;
        EditorGUIUtility.PingObject(gameManager.gameData);
    }
    
    private GameManager _FindGameManagerInScene() {
        return SceneManager.GetActiveScene().GetRootGameObjects()
            .Select(rootGameObject => rootGameObject.GetComponentInChildren<GameManager>())
            .FirstOrDefault(gameManager => gameManager);
    }
    #endregion

    #region Path2DTab

    private void _GUITabsPath2D() {
        GUILayout.Space(10f);

        if (_path2DOnScene == null) return;
        
        foreach (Path2D path in _path2DOnScene) {
            if (path == null) continue;
            if (GUILayout.Button(path.gameObject.name)) {
                _SelectPath2D(path);
            }
        }
    }

    private void _SelectPath2D(Path2D path2D) {
        Selection.activeGameObject = path2D.gameObject;
        EditorGUIUtility.PingObject(path2D);
    }
    
    private Path2D[] _SelectAllPath2D() {
        List<Path2D> resultList = new List<Path2D>();
        
        foreach (GameObject rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects()) {
            resultList.AddRange(rootGameObject.GetComponentsInChildren<Path2D>());
        }

        return resultList.ToArray();
    }

    #endregion
}