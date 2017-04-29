using UnityEngine;
using System.Collections;

public class GravityBody : MonoBehaviour {

	public GravityAttractor attractor;

	void Update ()
	{
		if (attractor != null) {
			attractor.Attract(transform);
		}
	}
}
