using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    [SerializeField] GameObject p_camera;
    [SerializeField] float paralaxEffect;

    private float _startPos, _length;
    void Start()
    {
        _startPos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = p_camera.transform.position.x * (1-paralaxEffect);
        float dist = p_camera.transform.position.x * paralaxEffect;

        transform.position = new Vector3(_startPos + dist, transform.position.y, transform.position.z);

        if (temp > _startPos + _length)
            _startPos += _length;
        else if (temp < _startPos - _length)
            _startPos -= _length;
    }
}
