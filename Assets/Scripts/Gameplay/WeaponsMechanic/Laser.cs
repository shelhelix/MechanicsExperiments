using GameComponentAttributes.Attributes;
using UnityEngine;

namespace GameJamEntry.Gameplay.WeaponsMechanic {
	public class Laser : BaseGun {
		[NotNullReference, SerializeField] LaserBeamView LaserBeamView;

		[SerializeField] float FireMaxDuration = 2f;
		[SerializeField] float ReloadTime = 1f;

		public int AmmoTotal = 3;
		public int AmmoLeft;
		
		GunStatus _status = GunStatus.Idle;

		Timer _reloadTimer = new();

		bool _isFiring;

		float _heatTime;

		public float OverheatNormalized => Mathf.Clamp01(_heatTime / FireMaxDuration);
		
		public override void PickUp() {
			AmmoLeft = AmmoTotal;
		}

		public override void Drop() {
			EndFire();
		}

		public override void StartFire() {
			if ( (_status != GunStatus.Idle) || (AmmoLeft <= 0) ) {
				return;
			}
			_isFiring = true;
			GoToFiring();
		}

		public override void EndFire() {
			LaserBeamView.HideBeam();
			_status = GunStatus.Idle;
			_reloadTimer.Deinit();
			_isFiring = false;
		}

		void Update() {
			_reloadTimer.Tick(Time.deltaTime);
			UpdateHeat();
			if ( _status == GunStatus.Firing ) {
				if (OverheatNormalized >= 1) {
					GoToReload();
				}
				TryFire();
			}
		}

		void UpdateHeat() {
			if ( _status == GunStatus.Firing ) {
				_heatTime = Mathf.Min(_heatTime + Time.deltaTime, FireMaxDuration);
			} else {
				_heatTime = Mathf.Max(_heatTime - Time.deltaTime * FireMaxDuration / ReloadTime, 0);
			}
		}

		Transform RayCastFire() {
			// TODO: add physics raycast
			return null;
		}

		void TryFire() {
			if ( _status != GunStatus.Firing ) {
				return;
			}
			var target = RayCastFire();
			LaserBeamView.ShowBeam(target);
		}
		
		void GoToIdle() {
			_status = GunStatus.Idle;
		}
		void GoToFiring() {
			_status = GunStatus.Firing;
		}

		#region ReloadBlock
		
		void GoToReload() {
			LaserBeamView.HideBeam();
			_status = GunStatus.ReloadMagazine;
			_reloadTimer.Init(ExitReloadState, ReloadTime, false);
		}
		
		void ExitReloadState() {
			AmmoLeft--;
			_heatTime = 0;
			if ( AmmoLeft <= 0 ) {
				EndFire();
				return;
			}
			// try to start new fire automatically 
			if ( _isFiring ) {
				GoToIdle();
				StartFire();
			}
		}

		#endregion
	}
}