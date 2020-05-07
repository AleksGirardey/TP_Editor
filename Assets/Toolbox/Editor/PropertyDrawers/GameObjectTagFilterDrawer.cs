using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomPropertyDrawer(typeof(GameObjectTagFilterAttribute))]
public class GameObjectTagFilterDrawer : PropertyDrawer {
    private GameObject[] _gameObjectsArr;
    private string[] _gameObjectsNamesArr;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        if (!(attribute is GameObjectTagFilterAttribute tagFilterAttribute)) return;
        
        if (_gameObjectsArr == null) {
            _gameObjectsArr = FindGameObjectsWithTagInScene(tagFilterAttribute.TagFilter);
        }

        if (_gameObjectsNamesArr == null) {
            _gameObjectsNamesArr = _gameObjectsArr.Select(gameObject => gameObject.name).ToArray();
        }
        
        GameObject currentGameObject = property.objectReferenceValue as GameObject;
        int currentIndex = Array.IndexOf(_gameObjectsArr, currentGameObject);

        if (currentIndex < 0) currentIndex = 0;
        
        currentIndex = EditorGUI.Popup(position, currentIndex, _gameObjectsNamesArr);

        property.objectReferenceValue = _gameObjectsArr[currentIndex];
    }

    private GameObject[] FindGameObjectsWithTagInScene(string tag) {
        List<GameObject> resultList = new List<GameObject>();
        
        // for (int i = 0; i < SceneManager.sceneCount; i++) {
        //     Scene scene = SceneManager.GetSceneAt(i);
        //     //ToDo: Load Scene
        // }

        Scene activeScene = SceneManager.GetActiveScene();
        foreach (GameObject gameObject in activeScene.GetRootGameObjects()) {
            foreach (Transform childTransform in gameObject.GetComponentsInChildren<Transform>()) {
                if (childTransform.gameObject.CompareTag(tag)) {
                    resultList.Add(childTransform.gameObject);
                }
            }
        }

        return resultList.ToArray();
    }
}