using UnityEngine;

public class MoveAround2D : MonoBehaviour {
    public float radius = 1f;
    public float speed = 90f;

    private float _angle;
    
    private Vector3 _initialPosition = Vector3.zero;

    [Header("Debug")]
    public bool guiDebug;
    public int guiFontSize = 24;
    public Color guiColor = Color.white;
    private GUIStyle _defaultGuiStyle;

    public bool gizmosDebug;
    public float gizmosSize = 0.1f;
    public Color gizmosCenterColor = Color.green;
    public Color gizmosDestinationColor = Color.blue;
    public Color gizmosRadiusColor = Color.red;
    
    
    private void Awake() {
        _initialPosition = transform.position;
    }

    private void Update() {
        _angle += speed * Time.deltaTime;


        transform.position = CalculateRotationPosition();
    }

    private Vector3 CalculateRotationPosition() {
        Vector3 newPosition = transform.position;
        float radAngle = Mathf.Deg2Rad * _angle;
        newPosition.x = _initialPosition.x + radius * Mathf.Cos(radAngle);
        newPosition.y = _initialPosition.y + radius * Mathf.Sin(radAngle);
        
        return newPosition;
    }
#if UNITY_EDITOR
    private void OnGUI() {
        if (guiDebug == false) return;

        if (_defaultGuiStyle == null) {
            _defaultGuiStyle = new GUIStyle();
        }

        _defaultGuiStyle.fontSize = guiFontSize;
        _defaultGuiStyle.normal.textColor = guiColor;
        GUILayout.BeginVertical();

        GUILayout.Label("Radius = " + radius, _defaultGuiStyle);
        GUILayout.Label("Speed = " + speed, _defaultGuiStyle);
        GUILayout.Label("Angle = " + _angle, _defaultGuiStyle);
        
        GUILayout.EndVertical();
    }

    private void OnDrawGizmos() {
        if (!gizmosDebug) return;
        
        Vector3 position = transform.position;

        //Draw Center
        Gizmos.color = gizmosCenterColor;
        Gizmos.DrawWireSphere(_initialPosition, gizmosSize);
        
        //Draw destination
        Gizmos.color = gizmosDestinationColor;
        Gizmos.DrawWireSphere(position, gizmosSize);
        
        //Draw radius
        Gizmos.color = gizmosRadiusColor;
        Gizmos.DrawLine(_initialPosition, position);
    }
#endif
}
