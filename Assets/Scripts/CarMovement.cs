using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CarMovement : MonoBehaviour
{   
    
    [SerializeField] public List<Transform> wheels = new List<Transform>();
    [SerializeField] private float breakForce;
    [SerializeField] Transform centerOfMass;

    [Header("__Engine__")]
    [SerializeField] float enginePower;
    [SerializeField] public float maxRPM = 6800f;
    [SerializeField] private float minRPM = 990f;
    [SerializeField] AnimationCurve engineCurveHP;
    
    [Header("__Gearbox__")]
    [SerializeField] private float[] gearRatio = new float[8] {0f, 3.91f, 2.29f, 1.58f, 1.18f, 0.94f, 0.79f,0.62f};
    [SerializeField] private float mainGearRatio = 3.9f;

    [Header("__For UI__")]
    [SerializeField] TMP_Text curSpeed;
    [SerializeField] TMP_Text curGear;
    [SerializeField] RotateTaxometr curRPM;

    private float _horizInput;
    private int _clutch;
    [HideInInspector] public float _curRPM = 1200f;
    [HideInInspector] public float shit = 0;
    [HideInInspector] public Rigidbody2D _rbCar ;

    private float _horsePower;
    private float _maxVelocity = 50f;
    private float _curVelocity;
    private float _oldRPM;
    private int i = 1;
    private float _otherRPM;
    
    
    public float speedShifting;

    void Start()
    {
        _rbCar = gameObject.GetComponent<Rigidbody2D>();
        _rbCar.centerOfMass = centerOfMass.localPosition;

    }


    void Update()
    {
        _horizInput = Input.GetAxis("Horizontal");

        curSpeed.text = ((int)(_rbCar.velocity.x * 3.6f)).ToString();
        curGear.text = i.ToString();
        curRPM.rotateStrelka(_curRPM);

        GearShift();
        Clutch();
    }


    void FixedUpdate()
    {        

        if (_horizInput >= 0)
            Acceleration();
        else if (_rbCar.velocity.x >= 0)
            Break(1);
        else
            Break(-1);

        EngineWork();
    }


    void EngineWork()
    {
        _horsePower = engineCurveHP.Evaluate(_curRPM);
        _curVelocity =( (_curRPM * 0.13188f)/(gearRatio[i] * mainGearRatio) );
        _maxVelocity =( ( maxRPM * 0.13188f)/(gearRatio[i] * mainGearRatio) );
        // Debug.Log(_horsePower);

        if (_rbCar.velocity.x * 3.6f <= _maxVelocity  && _curRPM > minRPM && _clutch == 1)
        {
            _curRPM = _rbCar.velocity.x * 3.6f * gearRatio[i]* mainGearRatio / 0.13f;
        }
        else if (_curRPM <= minRPM && _clutch == 1)
        {
            _curRPM =  Mathf.Max(minRPM, _rbCar.velocity.x * 3.6f * gearRatio[i]* mainGearRatio / 0.13f);
        }
        else if (_clutch == 0 && _horizInput >=0 && _curRPM <= maxRPM)
        {
            _oldRPM = _curRPM;
            _curRPM = Mathf.Lerp( minRPM, _oldRPM, 0.1f);
            // _oldRPM = _rbCar.velocity.x * 3.6f * gearRatio[i]* mainGearRatio / 0.13f;
            // Mathf.MoveTowards(_curRPM, minRPM, 0.1f* Time.deltaTime);
            
        }
        
        
    }


    void Acceleration()
    {
        if (_rbCar.velocity.x * 3.6f <= _maxVelocity && _clutch == 1)
            foreach (Transform wheel in wheels)
            {
                _rbCar.AddForceAtPosition(_horsePower * _horizInput * Vector2.right * Time.fixedDeltaTime * enginePower/ wheels.Count, wheel.position - Vector3.up * 0.34f);
                Debug.DrawRay(wheel.position, _horsePower * Vector2.right * Time.fixedDeltaTime / wheels.Count , Color.green);
            }
    }


    private void Break(int _directionRight)
    {
        foreach (Transform wheel in wheels)
        {
            _rbCar.AddForceAtPosition(_directionRight * breakForce * _horizInput * Vector2.right * Time.fixedDeltaTime, wheel.position - Vector3.up * 0.34f);
            Debug.DrawRay(wheel.position - Vector3.up * 0.34f, breakForce * _horizInput * Vector2.right * Time.fixedDeltaTime, Color.red);
        }
    }

    private void GearShift()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && i < 7 && i != 0 && _clutch == 0)
        {
            _curRPM = _rbCar.velocity.x * 3.6f * gearRatio[i]* mainGearRatio / 0.13f;
            
            _oldRPM = _curRPM;
            // Debug.Log("oldRPM = " + _oldRPM);

            _curVelocity = ((_oldRPM * 0.13188f) / (gearRatio[i] * mainGearRatio));
            // Debug.Log("CurVelocity = " + _curVelocity);

            i++;

            _curRPM =_curVelocity * gearRatio[i] * mainGearRatio / 0.13f;
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl) && i > 1 && ((_curVelocity * gearRatio[i-1] * mainGearRatio / 0.13f) < maxRPM) && _clutch == 0)
        {
            _oldRPM = _curRPM;
            // Debug.Log("oldRPM = " + _oldRPM);

            _curVelocity = ((_oldRPM * 0.13188f) / (gearRatio[i] * mainGearRatio));
            // Debug.Log("CurVelocity = " + _curVelocity);

            i--;

            _curRPM = _curVelocity * gearRatio[i] * mainGearRatio / 0.13f;
        }
            
    }

    private void Clutch()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            _clutch = 0;
            // Debug.Log("Сцепа выжата");
        }
        else
        {
            _clutch = 1;
            // Debug.Log("Полная мощность на колеса");
        }
    }

}
