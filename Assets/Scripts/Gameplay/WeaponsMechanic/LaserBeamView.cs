using GameComponentAttributes.Attributes;
using UnityEngine;

namespace GameJamEntry.Gameplay.WeaponsMechanic {
	public class LaserBeamView : MonoBehaviour { 
		public float MaxDistance;
		
		[NotNullReference, SerializeField] LineRenderer LineRenderer;

		Vector3 MaxEndPositions => transform.position + transform.up * MaxDistance;
		
		public void ShowBeam(Transform endPoint) {
			LineRenderer.enabled = true;
			var endPosition = endPoint ? endPoint.position : MaxEndPositions;
			LineRenderer.SetPosition(0, transform.position);
			LineRenderer.SetPosition(1, endPosition);
		}
		
		public void HideBeam() {
			LineRenderer.enabled = false;
		}
	}
}