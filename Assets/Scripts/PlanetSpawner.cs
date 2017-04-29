using UnityEngine;
using System.Collections;

public class PlanetSpawner : MonoBehaviour {

	public GameObject planetPrefab;
	public GameObject planetParent;
	public GravityAttractor blackHole;
	public float thrust = 10f;
	public float torque = 2500f;
	public float forceMultiplier = 2f;

	private GameObject planet;

	void Start()
	{
	
	}

	void Update()
	{
		if (GvrController.ClickButtonDown) {
			SpawnPlanet();
		}

		if (GvrController.ClickButton) {
			PullPlanet(GvrController.TouchPos.y);
		}

		if (GvrController.ClickButtonUp) {
			LaunchPlanet(GvrController.TouchPos.y);
		}
	}

	private void SpawnPlanet ()
	{
		planet = (GameObject) Instantiate(planetPrefab, transform.position, new Quaternion());
		planet.transform.localScale = transform.localScale;
		planet.transform.parent = this.transform;
	}

	private void PullPlanet (float strength)
	{
		planet.transform.position = new Vector3(planet.transform.position.x, planet.transform.position.y,
				transform.position.z - (strength * forceMultiplier));
	}

	private void LaunchPlanet (float strength)
	{
		GetComponent<AudioSource>().Play();
		planet.GetComponent<TrailRenderer>().Clear();
		planet.GetComponent<TrailRenderer>().enabled = true;
		planet.GetComponent<GravityBody>().attractor = blackHole;
		float force = strength * forceMultiplier * thrust;
		planet.GetComponent<Rigidbody>().AddForce(transform.forward * force * -1, ForceMode.VelocityChange);
		planet.GetComponent<Rigidbody>().AddTorque(transform.up * torque);
		planet.transform.parent = planetParent.transform;
	}
}
