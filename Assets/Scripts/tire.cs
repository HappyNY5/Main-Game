using UnityEngine;


public class tire : MonoBehaviour{

    [SerializeField] private float groundFriction;
    [SerializeField] AnimationCurve frictionCurve;

    private float _finalFriction;
    private float _staticFriction;

    private CarMovement _carMovement;

    void Start()
    {
        _staticFriction = _carMovement._rbCar.mass * 9.8f * groundFriction; // F=m *N;
    }

    void FixedUpdate()
    {
        if (_carMovement._rbCar.velocity.x > 0)
        {
            foreach (Transform wheel in _carMovement.wheels)
                _carMovement._rbCar.AddForceAtPosition(_staticFriction * Vector2.left, wheel.position);

            Debug.Log("yes");
        }
    }
}