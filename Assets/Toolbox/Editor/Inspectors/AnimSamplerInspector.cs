using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomEditor(typeof(AnimSampler))]
public class AnimSamplerInspector : Editor {

    private Animator _animator;
    private AnimationClip[] _animationClipsArr;
    private string[] _animNamesArr;
    private bool _isInitialized;

    private int _currentAnimIndex;
    private bool _isPlaying;
    private float _editorLastTime;
    
    public override void OnInspectorGUI() {
        AnimSampler sampler = target as AnimSampler;

        if (sampler == null) return;
        
        if (!_isInitialized) {
            _animator = sampler.GetComponent<Animator>();
            _animationClipsArr = FindAnimClips(_animator);
            _animNamesArr = FindAnimsNames(_animationClipsArr);
            _isInitialized = true;
        }

        _currentAnimIndex = EditorGUILayout.Popup("Current Anim", _currentAnimIndex, _animNamesArr);

        if (_isPlaying) {
            if (GUILayout.Button("Stop")) {
                StopAnim();
            }
        } else {
            if (GUILayout.Button("Play")) {
                PlayAnim();
            }
        }
    }

    private void PlayAnim() {
        if (_isPlaying) return;

        _editorLastTime = Time.realtimeSinceStartup;
        EditorApplication.update += _OnEditorUpdate;
        AnimationMode.StartAnimationMode();
        _isPlaying = true;
    }

    private void _OnEditorUpdate() {
        float animTime = Time.realtimeSinceStartup - _editorLastTime;
        AnimationClip animationClip = _animationClipsArr[_currentAnimIndex];
        animTime %= animationClip.length;
        AnimationMode.SampleAnimationClip(_animator.gameObject, animationClip, animTime);
    }

    private void StopAnim() {
        if (!_isPlaying) return;
        EditorApplication.update -= _OnEditorUpdate;
        AnimationMode.StopAnimationMode();
        _isPlaying = false;
    }

    private string[] FindAnimsNames(AnimationClip[] animClipArr) {
        List<string> resultList = new List<string>();

        foreach (AnimationClip animationClip in animClipArr) {
            resultList.Add(animationClip.name);
        }

        return resultList.ToArray();
    }
    
    private AnimationClip[] FindAnimClips(Animator animator) {
        List<AnimationClip> resultList = new List<AnimationClip>();

        AnimatorController editorController = animator.runtimeAnimatorController as AnimatorController;

        if (editorController == null) return resultList.ToArray();

        AnimatorControllerLayer controllerLayer = editorController.layers[0];
        foreach (ChildAnimatorState childState in controllerLayer.stateMachine.states) {
            AnimationClip animationClip = childState.state.motion as AnimationClip;
            if (animationClip != null) resultList.Add(animationClip);
        }

        return resultList.ToArray();
    }
}