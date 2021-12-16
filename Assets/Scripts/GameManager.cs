using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	[SerializeField] private TMPro.TextMeshPro _textMeshPro;
	public int CurrentScore { get; private set; } = 0;
	public int BestHit { get; private set; } = 0;
	[SerializeField] private Projectile projectilePrefab;
	[SerializeField] private GameObject bowl;
	public Vector3 HandPosition { get; private set; }
	private Camera mainCamera;
	private Projectile currentlyHeld;
	private Vector3 dragStart;
	private float dragStartTime;
	[SerializeField] private float minimumDragDistance = .1f;

	// Start is called before the first frame update
	void Start() {
		this.mainCamera = Camera.main;
	}

	// Update is called once per frame
	public void Update() {
		var rayPoint = this.mainCamera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward);
		// Lerp value for zero plane.
		float lerpValue = this.mainCamera.transform.position.y / (this.mainCamera.transform.position.y - rayPoint.y);
		this.HandPosition = Vector3.LerpUnclamped(this.mainCamera.transform.position, rayPoint, lerpValue);

		if(currentlyHeld != null) {
			if(Input.GetMouseButtonDown(0)) {
				this.dragStart = HandPosition;
				dragStartTime = Time.time;
			}
			if(Input.GetMouseButtonUp(0) && this.dragStart != Vector3.zero) {
				var dragEnd = HandPosition;
				Vector3 dragDelta = dragEnd - dragStart;
				if(dragDelta.magnitude >= this.minimumDragDistance) {
					// Release after drag!
					// Time to throw.
					float dragTimeDelta = Time.time - dragStartTime;
					float upComponent = 100 / dragDelta.magnitude;
					currentlyHeld.Release((dragDelta / dragTimeDelta) + (Vector3.up * upComponent));
					currentlyHeld = null;
				}
			}
		}


		_textMeshPro.text = $"Score: {this.CurrentScore}\nBest hit: {this.BestHit}";
	}

	internal void RegisterGrab(Projectile projectile) {
		this.currentlyHeld = projectile;
		this.dragStart = Vector3.zero;
	}

	public void Score(Target target, Projectile projectile) {
		int hit = (int)Vector3.Distance(projectile.ReleasePosition, target.transform.position);
		if(hit > this.BestHit) {
			this.BestHit = hit;
		}
		this.CurrentScore += hit;
	}

	public void ReportOutOfBounds(Projectile projectile) {
		ResetProjectile(projectile);
	}

	private void ResetProjectile(Projectile projectile) {
		if(this.currentlyHeld == projectile) { this.currentlyHeld = null; }
		projectile.ResetPosition(this.bowl.transform.position + (Vector3.up * 10));
	}
}
