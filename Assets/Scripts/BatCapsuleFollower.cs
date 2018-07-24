using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCapsuleFollower : MonoBehaviour {

	[SerializeField] private GameObject _batFollower;
	private Rigidbody _rigidBody;
	private Vector3 _velocity;

	public float _speed;

	[SerializeField] private float _sensitivy = 100f;

	private void Awake(){
		_rigidBody = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
		Vector3 destination = _batFollower.transform.position;
		_rigidBody.transform.rotation = transform.rotation;
		_velocity = (destination - _rigidBody.transform.position) * _sensitivy;
		_rigidBody.velocity = _velocity;
		transform.rotation = _batFollower.transform.rotation;	
	}

	void OnCollisionEnter(Collision collision){
		collision.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.gameObject.transform.position * _speed * (transform.position.y * -1));
	}

}
