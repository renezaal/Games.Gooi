namespace Assets.Scripts {
	
	using UnityEngine;

	public class Interactable : MonoBehaviour {
		protected GameManager gameManager { get;private set; }
		protected Camera mainCamera { get; private set; }
		public virtual void Start() {
			this.gameManager = FindObjectOfType<GameManager>(false) ?? throw new System.Exception("GameManager is missing!");
			this.mainCamera = Camera.main;
			var material = this.GetComponent<MeshRenderer>()?.material;
			if(material != null) {
				material.color = Color.HSVToRGB(Random.value,1,1);
			}
		}

	}
}
