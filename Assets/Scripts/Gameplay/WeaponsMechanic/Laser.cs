using GameComponentAttributes.Attributes;
using UnityEngine;

namespace GameJamEntry.Gameplay.WeaponsMechanic {
	public class Laser : BaseGun {
		[NotNullReference, SerializeField] LaserBeamView LaserBeamView;

		[SerializeField] float FireMaxDuration = 2f;
		[SerializeField] float ReloadTime = 1f;

		public int AmmoTotal = 3;
		public int AmmoLeft;
		
		public float OverheatingProgressNormalized => _fireTimer.ProgressNormalized;

		GunStatus _status = GunStatus.Idle;

		Timer _fireTimer   = new();
		Timer _reloadTimer = new();

		bool _isFiring;
		
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
			_fireTimer.Deinit();
			_reloadTimer.Deinit();
			_isFiring = false;
		}

		void Update() {
			_fireTimer.Tick(Time.deltaTime);
			_reloadTimer.Tick(Time.deltaTime);
			TryFire();
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
			_fireTimer.Init(GoToReload, FireMaxDuration, false);
		}

		#region ReloadBlock
		
		void GoToReload() {
			_status = GunStatus.ReloadMagazine;
			_reloadTimer.Init(ExitReloadState, ReloadTime, false);
		}
		
		void ExitReloadState() {
			AmmoLeft--;
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