using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour
{
	public Transform Target;

	private Transform Tr;

	void Start()
	{
		Tr = transform;
	}

	void Update()
	{
		Tr.LookAt(Target.position);
	}
}
