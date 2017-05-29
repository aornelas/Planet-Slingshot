using UnityEngine;
using System.Collections;

public class OrbiterController : MonoBehaviour {

	void OnTriggerEnter(Collider other) 
	{		
		other.GetComponent<AudioSource>().Play();
		if (other.tag == "blackhole")
		{
			other.GetComponent<ParticleSystem>().Emit(1);
			this.gameObject.SetActive(false);
		}
	}

	void OnCollisionEnter(Collision col)
    {
    	col.gameObject.GetComponent<AudioSource>().Play();
    }
}
