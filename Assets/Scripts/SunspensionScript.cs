using System.Collections.Generic;
using UnityEngine;

public class SunspensionScript : MonoBehaviour
{

    /*
        пофиксить положение колеса на local    
    */

    //public

    [SerializeField] List<GameObject> wheels = new List<GameObject>();

    public float springLength;
    public float springStroke; // xод пружины
    public float springStiffness;

    public GameObject wheelText;
    public float wheelRadius;

    [SerializeField]
    private float dumperForce;




    //private

    float maxDistance;
    Rigidbody2D _rbCar;
    private float _springForce;
    private float springVelocity;
    RaycastHit2D hit;
    private float _lastLength;
    private float springCurLength;
    private float _rotation;
    private float _speed;

    void Start()
    {
        maxDistance = springLength+springStroke;
        _rbCar = gameObject.GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
        _speed = _rbCar.velocity.x;

        
        foreach (GameObject wheel in wheels)
        {
            _rotation = (_speed * Time.deltaTime)/wheelRadius * Mathf.Rad2Deg;
            wheel.transform.Rotate(0,0,-_rotation);
        }
    }

    void FixedUpdate()
    {
        LayerMask road_mask = LayerMask.GetMask("road");

        hit =  Physics2D.Raycast(transform.position,-transform.up, maxDistance,road_mask);
        if (hit)
        {
            _lastLength = springCurLength;
            springCurLength = hit.distance - wheelRadius;            
            
            Debug.DrawRay(transform.position,-(springCurLength) * Vector2.up,Color.green);

            _springForce = (springLength - springCurLength) * springStiffness ;

            springVelocity = (_lastLength - springCurLength) / Time.fixedDeltaTime;

            //поместить в Upd и ввести bool OnRoad
            wheelText.transform.position = new Vector2(transform.position.x, transform.position.y) - (springCurLength) * Vector2.up;

            _rbCar.AddForceAtPosition((_springForce + dumperForce * springVelocity)* Vector2.up ,transform.position);
        }
    }
}
