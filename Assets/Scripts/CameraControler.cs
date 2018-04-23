using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
 
	public float minimumX = -360F;
	public float maximumX = 360F;
 
	public float minimumY = -60F;
	public float maximumY = 60F;
 
	float rotationX = 0F;
	float rotationY = 0F;
 
	public float frameCounter = 20;
 
	Quaternion originalRotation;
 
	void Update ()
	{
		rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
		rotationX += Input.GetAxis("Mouse X") * sensitivityX;

		rotationY = ClampAngle (rotationY, minimumY, maximumY);
		rotationX = ClampAngle (rotationX, minimumX, maximumX);

		Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, Vector3.left);
		Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);

		transform.localRotation = originalRotation * yQuaternion;
		GetComponentInParent<GodController>().transform.localRotation = originalRotation * xQuaternion; 
	}
 
	void Start ()
	{		
		originalRotation = transform.localRotation;
	}
 
	public static float ClampAngle (float angle, float min, float max)
	{
		angle = angle % 360;
		if ((angle >= -360F) && (angle <= 360F)) {
			if (angle < -360F) {
				angle += 360F;
			}
			if (angle > 360F) {
				angle -= 360F;
			}			
		}
		return Mathf.Clamp (angle, min, max);
	}
}
