using Cinemachine;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private float _parallaxEffect;

    private Vector3 _originalPosition;

    private void Start()
    {
        _originalPosition = transform.position;
    }

    void FixedUpdate()
    {
      //  transform.position = new Vector3(_originalPosition.x + distX, _originalPosition.y + distY, transform.position.z);
    }
}