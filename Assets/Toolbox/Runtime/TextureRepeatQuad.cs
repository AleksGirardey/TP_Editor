using UnityEngine;

namespace Toolbox {
    [ExecuteAlways]
    public class TextureRepeatQuad : MonoBehaviour {
        private MeshFilter _meshFilter;
        private Renderer _renderer;

        private Vector3 _currentLocalScale;
        private bool _isInitialized;

        private void Start() {
            _meshFilter = GetComponent<MeshFilter>();
            _renderer = GetComponent<Renderer>();
            _currentLocalScale = transform.localScale;
            
            _CheckInit();
            
            if (_renderer.sharedMaterial.mainTexture.wrapMode != TextureWrapMode.Repeat)
                _renderer.sharedMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;
        }

        private void Update() {
            _CheckInit();
            
            if (transform.localScale == _currentLocalScale) return;

            _currentLocalScale = transform.localScale;
            _RefreshMesh();
        }

        private void _CheckInit() {
            if (_isInitialized) return;

            _meshFilter = GetComponent<MeshFilter>();
            _renderer = GetComponent<Renderer>();
            _RefreshMesh();
            _isInitialized = true;
        }
        
        private void _RefreshMesh() {
            Mesh mesh = _GetMesh();
            if (!mesh) return;
            
            mesh.uv = SetupUvMap();
        }

        private Mesh _GetMesh() {
            if (Application.isPlaying) {
                return _meshFilter.mesh;
            }

            if (!_meshFilter) return null;
            if (!_meshFilter.sharedMesh) return null;

            string meshId = "Mesh" + GetInstanceID();
            if (_meshFilter.sharedMesh.name != meshId) {
                Mesh meshCopy = Instantiate(_meshFilter.sharedMesh);
                meshCopy.name = meshId;
                _meshFilter.mesh = meshCopy;
            }
            return _meshFilter.sharedMesh;
        }

        private Vector2[] SetupUvMap() {
            float scaleX = _currentLocalScale.x;
            float scaleY = _currentLocalScale.y;
            Vector2[] meshUVs = new Vector2[4];

            meshUVs[0] = new Vector2(0f, 0f);
            meshUVs[1] = new Vector2(scaleX, 0f);
            meshUVs[2] = new Vector2(0f, scaleY);
            meshUVs[3] = new Vector2(scaleX, scaleY);

            return meshUVs;
        }
    }
}
