using UnityEngine;
using System.Collections;

public class QuadCopterMovement : MonoBehaviour {

	public Transform LB;
	public Transform RB;
	public Transform LF;
	public Transform RF;

	public float strength;
	public float LBP;
	public float RBP;
	public float LFP;
	public float RFP;
	
	private float PLB;
	private float PRB;
	private float PLF;
	private float PRF;

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {

		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		if (Input.GetKeyDown("space")) {
			if(strength < 5.0f){
				strength = 5.0f;
			} else {
				strength = 0.0f;
			}
		}

		LBP = strength;
		RBP = strength;
		LFP = strength;
		RFP = strength;
		float steerStength = 0.15f;
		if (horizontal > 0) {
			LBP *= 1+steerStength;
			RBP *= 1-steerStength;
			LFP *= 1+steerStength;
			RFP *= 1-steerStength;
		}
		if (horizontal < 0) {
			LBP *= 1-steerStength;
			RBP *= 1+steerStength;
			LFP *= 1-steerStength;
			RFP *= 1+steerStength;
		}
		if (vertical > 0) {
			LBP *= 1+steerStength;
			RBP *= 1+steerStength;
			LFP *= 1-steerStength;
			RFP *= 1-steerStength;
		}
		if (vertical < 0) {
			LBP *= 1-steerStength;
			RBP *= 1-steerStength;
			LFP *= 1+steerStength;
			RFP *= 1+steerStength;
		}
		
		PLB = propellor (true,  LBP, LB, PLB);
		PLF = propellor (true,  LFP, LF, PLF);
		PRB = propellor (false, RBP, RB, PRB);
		PRF = propellor (false, RFP, RF, PRF);

	}

	float propellor(bool left, float force, Transform prop, float prev){
		RaycastHit hit;
		Physics.Raycast (prop.position, -prop.up, out hit, 8*force);
		if (hit.distance > 0) {
			float applyForce = 2 * (force - hit.distance) + 2 * (prev - hit.distance) / Time.deltaTime;
			rb.AddForceAtPosition (applyForce*prop.up, prop.position);
			return hit.distance;
		}
		return prev;
	}
}
