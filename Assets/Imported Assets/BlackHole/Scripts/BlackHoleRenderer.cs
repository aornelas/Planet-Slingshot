using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class BlackHoleRenderer : MonoBehaviour {

	private Shader  shader;

	[HideInInspector]public float ratio = 0.7f;  	// The height to the length ratio of the screen to display shader properly
	[HideInInspector]public float radius = 0.5f; 	// The radius of the black hole measured in the same units as the other objects in the scene
	[HideInInspector]public bool EinsteinRadiusCompliance;
	[HideInInspector]public Transform BH;  // The object whose position is taken as the position of the black hole

	private Camera Cam;
	private Camera PCam;
	private int outOfScreen;
	private Material _material; // Material in which the shader is located
	protected Material material 
	{
		get 
		{
			if (_material == null) 
			{
				_material = new Material (shader);
				_material.hideFlags = HideFlags.HideAndDontSave;
			}
			return _material;
		} 
	}

	void Start()
	{
		shader = Resources.Load<Shader>("BlackHoleDistortion");
		material.SetInt("_EinsteinR", EinsteinRadiusCompliance == true ? 1 : 0);
		Cam = GetComponent<Camera>();
		PCam = GetComponentInParent<Camera>();
	}

	protected virtual void OnDisable() 
	{
		if( _material ) 
		{
			DestroyImmediate( _material );
		}
	}

	Vector3 wtsp;
	void OnRenderImage (RenderTexture source, RenderTexture destination) 
	{
		if (shader && material && BH != null) 
		{
			wtsp = Cam.WorldToScreenPoint(BH.position);
			// Find the position of the black hole in screen coordinates
			Vector2 pos = new Vector2(wtsp.x / Cam.pixelWidth, wtsp.y / Cam.pixelHeight);

			// Install all the required parameters for the shader
			material.SetVector("_Position", new Vector2(pos.x, pos.y));
			material.SetFloat("_Ratio", ratio);
			Vector3 ScreenPos = Camera.current.WorldToScreenPoint (BH.position);
			if((ScreenPos.x > Camera.current.pixelWidth || ScreenPos.x < 0) || (ScreenPos.y > Camera.current.pixelHeight || ScreenPos.y < 0))
			{
				material.SetFloat("_Rad", Mathf.Lerp(material.GetFloat("_Rad"), 0, Time.deltaTime * 5));
				outOfScreen = 60;
			}
			else
			{
				if(outOfScreen > 0)
				{
					material.SetFloat("_Rad", Mathf.Lerp(material.GetFloat("_Rad"), radius, Time.deltaTime * 5));
					outOfScreen--;
				}
				else
				{
					material.SetFloat("_Rad", radius);
				}
			}

			material.SetFloat("_Distance", Vector3.Distance(BH.position, this.transform.position));
			// And is applied to the resulting image.
			Graphics.Blit(source, destination, material);
		}
	}
}
