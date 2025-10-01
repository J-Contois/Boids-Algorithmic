using System.Collections.Generic;
using UnityEngine;

// ! Feathers movement still needs some perfection (right wings feathers move backward)
public class FlyAnimation : MonoBehaviour {
    [Header("Flying animation")]
    [Tooltip("If false, the object will move within a flock. All arguments below in this section become useless.")]
    [SerializeField] private bool autonomous = true;
    [SerializeField] private bool clockwise = true;                             // Rotation of flight path
    [SerializeField] private float radius = 15f;                                // Size of circle to fly around
    [SerializeField] private float speed = 1f;                                  // Fly speed
    [Tooltip("Offset in degrees if the model's 'front' does not correspond to +Z (ex: 180 to invert)")]
    [SerializeField] private float yawOffsetDegrees = 0f;
    [Tooltip("Optional target to orbit around (if null, orbits around starting position")]
    [SerializeField] private GameObject target;

    [Header("Vertical movement (optional)")]
    [SerializeField] private float verticalAmplitude = 2f;                      // Oscillation height
    [SerializeField] private float verticalSpeed = 0.5f;                        // Oscillation frequency

    [Header("Wings flapping")]
    [SerializeField] private Transform leftWing;
    [SerializeField] private Transform rightWing;
    [SerializeField] private float flapAmplitude = 30f;                         // Flap angle
    [SerializeField] private float flapSpeed = 0.5f;                            // Flap frequency

    [Header("Feathers movement")]
    [SerializeField] private float featherAmplitude = 5f;                       // Max angle ?
    [SerializeField] private float featherSmooth = 5f;                          // Speed of interpolation

    private Vector3 _startPos;                                                  // Starting position (center of circle)
    private Vector3 _lastPos;
    private Vector3 _direction;                                                 // Current movement direction

    private List<Transform> _Feathers = new List<Transform>();
    private float _currentFeatherAngle = 0f;                                    // Smooth angle

    void Start() {
        _startPos = target != null ? target.transform.position : transform.position;
        _lastPos = transform.position;
        _direction = _startPos - _lastPos;

        if (leftWing == null) {                                                 // Try to find wings by name if not assigned
            GameObject child = transform.Find("Wings")?.gameObject;
            GameObject wing = child.transform.Find("LeftWing")?.gameObject;
            if (wing != null) leftWing = wing.transform;
        }

        if (rightWing == null) {
            GameObject child = transform.Find("Wings")?.gameObject;
            GameObject wing = child.transform.Find("RightWing")?.gameObject;
            if (wing != null) rightWing = wing.transform;
        }

        foreach (Transform child in GetComponentsInChildren<Transform>())       // Find feathers by name (should be 5)
            if (child.name.Contains("Feathers")) _Feathers.Add(child);
    }

    void Update() {
        // Fly animation
        if (autonomous) flyAnimation();

        // Wings animation
        if (leftWing != null && rightWing != null) {
            float flapAngle = Mathf.Sin(Time.time * flapSpeed * Mathf.PI * 2f) * flapAmplitude;

            leftWing.localRotation = Quaternion.Euler(0f, 0f, flapAngle);       // Apply local rotation
            rightWing.localRotation = Quaternion.Euler(0f, 0f, -flapAngle+180f);// Reverse for symmetry
        }

        // Feathers animation
        float verticalSpeed = _direction.y / Time.deltaTime;
        float targetAngle = Mathf.Clamp(-verticalSpeed * featherAmplitude, -featherAmplitude, featherAmplitude);
        _currentFeatherAngle = Mathf.Lerp(_currentFeatherAngle, targetAngle, Time.deltaTime * featherSmooth);

        foreach (var feather in _Feathers) {
            bool inverted = Mathf.Abs(Mathf.Round(feather.parent.localEulerAngles.z) - 180f) < 1f;
            Debug.Log($"{feather.parent.parent} : {inverted}");                 // !!!
            float appliedAngle = inverted ? -_currentFeatherAngle : _currentFeatherAngle;
            feather.localRotation = Quaternion.Euler(appliedAngle, 0f, 0f);
        }
    }

    private void flyAnimation() {
        float angle = Time.time * speed * (clockwise ? -1f : 1f);

        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;
        float y = Mathf.Sin(Time.time * this.verticalSpeed * Mathf.PI * 2f) * -verticalAmplitude;

        Vector3 newPos = _startPos + new Vector3(x, y, z);
        transform.position = newPos;

        _direction = newPos - _lastPos;                                         // Movement direction

        if (_direction.sqrMagnitude > 1e-6f) {
            Quaternion targetRot = Quaternion.LookRotation(_direction.normalized, Vector3.up);   // Look at where it's going

            // Apply yaw offset
            if (Mathf.Abs(yawOffsetDegrees) > 0.001f) targetRot *= Quaternion.Euler(0f, yawOffsetDegrees, 0f);

            if (speed <= 0f) transform.rotation = targetRot;                    // Apply rotation (smoothly if moving)
            else transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, speed * Time.deltaTime);
        }

        _lastPos = newPos;
    }
}
