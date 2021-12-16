using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : Interactable
{
	public void OnCollisionEnter(Collision collision) {
		var projectile = collision.collider.GetComponent<Projectile>();
		if (projectile != null) {
			this.gameManager.Score(this, projectile);
		}
	}
}
