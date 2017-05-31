using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BlackHoleEffect : MonoBehaviour {

	// public settings
	public Shader shader;
	public Transform blackhole;
	public float ratio; // aspect ratio of the screen
	public float radius; // size of black hole in scene

	// private settings
	Camera cam;
	Material _material; // procedurally generated

	Material material {
		get {
			if (_material == null) {
				_material = new Material(shader);
				_material.hideFlags = HideFlags.HideAndDontSave; // ?
			}
			return _material;
		}
	}

	void OnEnable ()
	{
		cam = GetComponent<Camera>();
		ratio = 1f / cam.aspect; // need to invert ratio for some reason?
	}

	void OnDisable ()
	{
		if (_material) {
			DestroyImmediate(_material);
		}
	}

	Vector3 wtsp;
	Vector2 pos;

	// processes camera rendered frame (source) and output as destination
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		if (shader && material && blackhole) {
			wtsp = cam.WorldToScreenPoint(blackhole.position);

			// is the black hole in front of the camera
			if (wtsp.z > 0) {
				// normalize pixel position into 0..1 for shader input
				pos = new Vector2(wtsp.x / cam.pixelWidth, wtsp.y / cam.pixelHeight);
				// apply shader parameters
				_material.SetVector("_Position", pos);
				_material.SetFloat("_Ratio", ratio);
				_material.SetFloat("_Rad", radius);
				_material.SetFloat("_Distance", Vector3.Distance(blackhole.position, transform.position));

				// apply shader to image
				Graphics.Blit(source, destination, material);
			}
		}
	}
	
}
