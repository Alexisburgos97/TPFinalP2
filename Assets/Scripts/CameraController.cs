using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GatherInput gatherInput;
    [SerializeField] private Transform target;
    
    [SerializeField] private float rotationSpeedMouse = 5f;
    [SerializeField] private float rotationSpeedGamepad = 120f;
    
    [SerializeField] private float distance = 5f;
    
    [SerializeField] private float minYAngle = -20f;
    [SerializeField] private float maxYAngle = 45f;

    [SerializeField] private Vector2 offset;
    [SerializeField] private bool invertX, invertY;

    private float rotationY, rotationX;

    private void Start()
    {
        Cursor.visible = false;
    }
    
    void LateUpdate()
    {
        float invertValueX = invertX ? -1 : 1;
        float invertValueY = invertY ? -1 : 1;
        
        float rotationSpeed = gatherInput.usingGamePad ? rotationSpeedGamepad : rotationSpeedMouse;
     
        rotationX += gatherInput.lookInput.y * invertValueY * rotationSpeed * Time.deltaTime;
        rotationY += gatherInput.lookInput.x * invertValueX * rotationSpeed * Time.deltaTime;
        
        rotationX = Mathf.Clamp(rotationX, minYAngle, maxYAngle);
        
        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
        var focusPosition = target.position + new Vector3(offset.x, offset.y);
        
        transform.SetPositionAndRotation(focusPosition - targetRotation * new Vector3(0, 0, distance), targetRotation);
        
        // transform.position = focusPosition - targetQuaternion * new Vector3(0, 0, distance);
        // transform.rotation = targetQuaternion;
    }

    public Quaternion GetYRotation => Quaternion.Euler(0,rotationY,0);
    
}
