using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationSimulationWindow : EditorWindow {
    private Animator[] _animators;
    private Animator _selectedAnimator;
    
    private AnimationClip[] _animations;
    private AnimationClip _selectedAnimationClip;

    private int _tabIndex;
    private bool _isPlaying;
    private float _editorLastTime;
    private float _animationTime;
    private bool _inPlayMode;

    // Animation settings

    private SearchField _searchField;
    private string _textFilter;
    private float _sampleTime;
    private bool _isSampling;
    private float _speedModifier = 1f;
    private float _delayBetweenLoop;
    
    private static readonly string[] Tabs = {
        "Animators",
        "Animations"
    };
    
    [MenuItem("Toolbox/Animation Simulation Window")]
    private static void ShowWindow() {
        EditorWindow window = GetWindow<AnimationSimulationWindow>();
        window.titleContent = new GUIContent("Animation Simulation Window");
        window.Show();
    }

    private void OnEnable() {
        ResetWindow();
    }

    private void OnGUI() {
        EditorApplication.playModeStateChanged += PlayModeChange;
        EditorSceneManager.sceneOpened += OnSceneOpen;
        
        if (_animators == null) {
            _animators = _SelectAllAnimators();
        }

        if (_inPlayMode) {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label(
                $"/!\\ CANNOT PLAY WITH ANIMATIONS WHILE IN PLAY MODE /!\\",
                new GUIStyle {fontStyle = FontStyle.Bold, normal = {textColor = Color.red}});
            GUILayout.EndVertical();
            return;
        }
        
        _tabIndex = GUILayout.Toolbar(_tabIndex, Tabs);

        GUILayout.Space(10f);
        
        switch (_tabIndex) {
            case 0: _GUITabAnimators(); break;
            case 1: _GUITabAnimations(); break;
        }
    }

    private void OnSceneOpen(Scene scene, OpenSceneMode mode) {
        ResetWindow();
    }

    private void PlayModeChange(PlayModeStateChange state) {
        switch (state) {
            case PlayModeStateChange.EnteredPlayMode:
                _inPlayMode = true;
                StopAnim();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                _inPlayMode = false;
                break;
        }
    }

    private void ResetWindow() {
        StopAnim();
        _selectedAnimator = null;
        _selectedAnimationClip = null;
        _animators = null;
        _searchField = new SearchField();
    }
    
    private void OnHierarchyChange() {
        Animator lastAnimator = _selectedAnimator;
        ResetWindow();
        _animators = _SelectAllAnimators();
        if (lastAnimator != null)
            _selectedAnimator = _animators.Contains(lastAnimator) ? lastAnimator : null;
    }

    #region Animators

    private void _GUITabAnimators() {
        GUILayout.BeginVertical(GUI.skin.box);
        DrawResearchField();
        GUILayout.Label(
            $"Animator Selected : {(_selectedAnimator == null ? "NONE" : _selectedAnimator.name)}",
            new GUIStyle {fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter});
        
        foreach (Animator animator in _animators) {
            _DisplayAnimatorButton(animator);
        }
        GUILayout.EndVertical();
    }

    private void DrawResearchField() {
        OnInputChange(_searchField.OnGUI(_textFilter));
    }

    private void OnInputChange(string text) {
        if (text == _textFilter) return;

        _textFilter = text;
        
        _animators = _textFilter == ""
            ? _SelectAllAnimators()
            : _animators.Where(value => value.name.ToUpper().Contains(_textFilter.ToUpper())).ToArray();
    }
    
    private Animator[] _SelectAllAnimators() {
        List<Animator> resultList = new List<Animator>();

        foreach (GameObject rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects()) {
            resultList.AddRange(rootGameObject.GetComponentsInChildren<Animator>());
        }

        return resultList.ToArray();
    }

    private void _SelectAnimator(Animator animator) {
        StopAnim();
        _selectedAnimationClip = null;
        
        Selection.activeGameObject = animator.gameObject;
        EditorGUIUtility.PingObject(animator);
        _selectedAnimator = animator;
        _animations = _GetAnimClips(animator);
    }

    private void _DisplayAnimatorButton(Animator animator) {
        if (GUILayout.Button(animator.gameObject.name))
            _SelectAnimator(animator);
    }
    
    #endregion

    #region Animations

    private void _GUITabAnimations() {
        if (_selectedAnimator == null) {
            ShowNotification(new GUIContent("Select an animator first !"));
        } else {
            // Animation Clips
            foreach (AnimationClip animationClip in _animations) {
                if (GUILayout.Button(animationClip.name))
                    _DisplayAnimationClip(animationClip);
            }

            if (_selectedAnimationClip == null) return;
            
            // Animation informations
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label(_selectedAnimationClip.name, new GUIStyle {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter});

            // Play & Stop buttons
            DisplayAnimationPlayer();
            if (_isPlaying) {
                DisplayAnimationSlider();
                DisplayAnimationSpeed();
                DisplayAnimationInfos();
            }
            DisplayAnimationWait();

            GUILayout.EndVertical();
        }
    }

    private void DisplayAnimationPlayer() {
        GUILayout.BeginHorizontal();
        if (!_isPlaying) {
            if (GUILayout.Button("Play"))
                PlayAnim();
        } else {
            if (GUILayout.Button("Stop"))
                StopAnim();
        }

        GUILayout.EndHorizontal();
    }
    
    private void DisplayAnimationSpeed() {
        GUILayout.BeginHorizontal();
        GUILayout.Label($"Speed x{_speedModifier:0.00}", GUILayout.Width(90f));
        _speedModifier = GUILayout.HorizontalSlider(_speedModifier, 0.01f, 10f);
        GUILayout.EndHorizontal();
    }

    private void DisplayAnimationSlider() {
        GUILayout.BeginHorizontal();
        _isSampling = GUILayout.Toggle(_isSampling, "Sample", GUILayout.Width(60f));
        if (_isSampling) {
            GUILayout.Label($" at {_sampleTime:0.00} s", GUILayout.Width(60f));
        }
        _sampleTime = GUILayout.HorizontalSlider(_sampleTime, 0f, _selectedAnimationClip.length);
        GUILayout.EndHorizontal();
    }

    private void DisplayAnimationInfos() {
        if (!_isPlaying) return;
        GUILayout.BeginVertical(GUI.skin.box);
        
        GUILayout.Label($"Time : {_animationTime:0.000} / {_selectedAnimationClip.length:0.000}");
        GUILayout.Label($"Animation is {(_selectedAnimationClip.isLooping ? "" : " not")} looping");
        
        GUILayout.EndVertical();
    }

    private void DisplayAnimationWait() {
        GUILayout.BeginHorizontal();
        GUILayout.Label($"Delay btw loop : {_delayBetweenLoop:0.00}", GUILayout.Width(140f));
        _delayBetweenLoop = GUILayout.HorizontalSlider(_delayBetweenLoop, 0f, 3f);
        GUILayout.EndHorizontal();
    }
    
    private AnimationClip[] _GetAnimClips(Animator animator) {
        List<AnimationClip> resultList = new List<AnimationClip>();

        AnimatorController editorController = animator.runtimeAnimatorController as AnimatorController;

        if (editorController == null) return resultList.ToArray();

        resultList.AddRange((
                from controllerLayer in editorController.layers
                from childState in controllerLayer.stateMachine.states
                select childState.state.motion)
            .OfType<AnimationClip>());

        return resultList.ToArray();
    }

    private void _DisplayAnimationClip(AnimationClip animationClip) {
        _selectedAnimationClip = animationClip;
        StopAnim();
    }
    
    private void PlayAnim() {
        if (_isPlaying) return;

        _editorLastTime = Time.realtimeSinceStartup;
        EditorApplication.update += _OnEditorUpdate;
        AnimationMode.StartAnimationMode();
        _isPlaying = true;
    }

    private void _OnEditorUpdate() {
        _animationTime = 0;

        if (!_isSampling) {
            _animationTime = Time.realtimeSinceStartup - _editorLastTime;
            _animationTime *= _speedModifier;
            _animationTime %= _selectedAnimationClip.length + _delayBetweenLoop;
        } else {
            _animationTime = _sampleTime;
        }

        AnimationMode.SampleAnimationClip(_selectedAnimator.gameObject, _selectedAnimationClip, _animationTime);
        Repaint();
    }

    private void StopAnim() {
        if (!_isPlaying) return;
        EditorApplication.update -= _OnEditorUpdate;
        AnimationMode.StopAnimationMode();
        _isPlaying = false;
    }
    
    #endregion
}