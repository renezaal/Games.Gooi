using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Interactable {
	private Rigidbody body;
	private bool stickToMouse = false;
	private bool followMouse = false;
	public Vector3 ReleasePosition { get; private set; } = Vector3.zero;

	public override void Start() {
		base.Start();
		this.body = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	public void FixedUpdate() {
		if(this.transform.position.y < -10) {
			this.gameManager.ReportOutOfBounds(this);
		}
	}

	public void Update() {
		if(this.stickToMouse) {
			this.transform.position = this.gameManager.HandPosition;
		}
	}

	public void OnMouseUpAsButton() {
		if(this.stickToMouse == false) {
			this.body.isKinematic = true;
			this.body.useGravity = false;
			this.transform.position = this.gameManager.HandPosition;
			this.stickToMouse = true;
			this.gameManager.RegisterGrab(this);
		}
	}

	internal void ResetPosition(Vector3 newPosition) {
		this.transform.position = newPosition + (Vector3.one * 0.01f * Random.value);
		this.body.ResetInertiaTensor();
		this.body.velocity=Vector3.zero;
		this.body.angularVelocity=Vector3.zero;
	}

	internal void Release(Vector3 velocity) {
		this.ReleasePosition = this.transform.position;
		this.body.isKinematic = false;
		this.body.useGravity = true;
		this.stickToMouse = false;
		this.body.velocity = velocity;
	}
}
