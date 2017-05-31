using UnityEngine;
using System.Collections;

public class BlackHoleCamera : MonoBehaviour
{
	public Transform BlackHoleTransform;
	public bool EinsteinRadiusCompliance;
	public float Radius;

	private Camera BHCam;
	private Camera Cam;
	private BlackHoleRenderer BHR;

	private float CamFar;

	void Awake()
	{
		Cam = GetComponent<Camera>();
		CamFar = Cam.farClipPlane;

		GameObject cam = new GameObject("BHC");

		Transform tr = cam.transform;
		tr.SetParent(transform);
		tr.localPosition = Vector3.zero;
		tr.localRotation = Quaternion.identity;
		tr.localScale = Vector3.one;

		BHCam = cam.AddComponent<Camera>();
		BHCam.CopyFrom(Cam);
		BHCam.renderingPath = RenderingPath.Forward;
		BHCam.depth = -10;
		BHCam.clearFlags = CameraClearFlags.Skybox;

		Cam.clearFlags = CameraClearFlags.Depth;
		Cam.renderingPath = RenderingPath.Forward;

		BHR = cam.AddComponent<BlackHoleRenderer>();
		BHR.BH = BlackHoleTransform;
		BHR.EinsteinRadiusCompliance = EinsteinRadiusCompliance;
		BHR.ratio = Screen.height / Screen.width;
		BHR.radius = Radius;
	}

	void Update()
	{
		BHR.radius = Radius;
		BHR.ratio = (float)(Screen.height) / (float)(Screen.width);
		BHCam.fieldOfView = Cam.fieldOfView;

		float clip = Mathf.Abs(Vector3.Distance(BlackHoleTransform.position, this.transform.position) * Mathf.Cos(Mathf.Deg2Rad * Vector3.Angle(this.transform.forward, this.transform.position - BlackHoleTransform.position)));
		BHCam.nearClipPlane = clip;
		BHCam.farClipPlane = CamFar;
		Cam.farClipPlane = clip;
	}
}
