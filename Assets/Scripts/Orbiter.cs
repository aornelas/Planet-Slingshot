//---------------------------------------------------
// From: https://www.udemy.com/3dmotive-learn-advanced-c-scripting-in-unity-5
//---------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
//---------------------------------------------------
public class Orbiter : MonoBehaviour 
{
	//---------------------------------------------------
	public Transform Pivot = null;
	private Transform ThisTransform = null;
	private Quaternion DestRot = Quaternion.identity;
	//Distance to maintain from pivot
	public float PivotDistance = 5f;
	public float MaxRotSpeed = 10f;
	private float RotX = 0f;
	private float RotY = 0f;
	//---------------------------------------------------
	void Awake()
	{
		ThisTransform = GetComponent<Transform>();
		ThisTransform.position = Pivot.position + ThisTransform.rotation * Vector3.forward * -PivotDistance;
	}
	//---------------------------------------------------
	void Update()
	{
		if (GvrController.IsTouching && ! GvrController.ClickButton) {
			float Horz = ControllerToAxis(GvrController.TouchPos.x);
			float Vert = ControllerToAxis(GvrController.TouchPos.y);
			float RotSpeed = Mathf.Max(Mathf.Abs(Horz), Mathf.Abs(Vert)) * MaxRotSpeed;

			RotX += Vert * Time.deltaTime * RotSpeed;
			RotY += Horz * Time.deltaTime * RotSpeed;
			
			Quaternion YRot = Quaternion.Euler(0f,RotY,0f);
			DestRot = YRot * Quaternion.Euler(RotX,0f,0f);
			
			ThisTransform.rotation = DestRot;
			
			//Adjust position
			ThisTransform.position = Pivot.position + ThisTransform.rotation * Vector3.forward * -PivotDistance;
		}

	}
	//---------------------------------------------------
	private float ControllerToAxis(float touchValue) {
		// translate a Daydream Controller touch position value (0..1) to a cross platform axis value (-1..1);
		return touchValue * -2 + 1;
	}
	//---------------------------------------------------
}
//---------------------------------------------------