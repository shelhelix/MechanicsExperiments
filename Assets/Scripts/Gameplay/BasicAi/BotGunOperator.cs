using System.Collections.Generic;
using GameComponentAttributes.Attributes;
using GameJamEntry.Gameplay.WeaponsMechanic;
using UnityEngine;

namespace GameJamEntry.Gameplay.BasicAi {
	public class BotGunOperator : MonoBehaviour {
		const float TestTime = 3f;
		
		[NotNullReference, SerializeField] List<BaseGun> GunsToTest;
		[SerializeField]                   Transform     GunHolder;

		BaseGun                     _currentGun;
		BaseBotWeaponOperationLogic _operationLogic;

		bool _isFiring;

		float _leftTime;

		void Update() {
			if ( _leftTime > 0 ) {
				_operationLogic?.Tick();
				_leftTime -= Time.deltaTime;
			} else {
				ChangeWeapon();
				_leftTime = TestTime;
			}
		}

		void ChangeWeapon() {
			_operationLogic?.Deinit();
			var newGun = TrySelectWeaponToTest();
			DropOldGun(newGun.transform.position);
			PickUpGun(newGun);
			_operationLogic = CreateOperationLogic(newGun);
			_operationLogic?.Init(_currentGun);
		}
		
		void PickUpGun(BaseGun gun) {
			_currentGun = gun;
			_currentGun.transform.SetParent(GunHolder);
			_currentGun.transform.position = GunHolder.position;
			_currentGun.PickUp();
		}

		void DropOldGun(Vector3 pos) {
			// drop old gun if exists
			if ( !_currentGun ) {
				return;
			}
			_currentGun.Drop();
			_currentGun.transform.SetParent(null);
			_currentGun.transform.position = pos;
		}

		BaseGun TrySelectWeaponToTest() {
			if ( GunsToTest.Count == 0 ) {
				return null;
			}
			var gunIndex = GunsToTest.IndexOf(_currentGun);
			gunIndex = (gunIndex + 1) % GunsToTest.Count;
			return GunsToTest[gunIndex];
		}

		BaseBotWeaponOperationLogic CreateOperationLogic(BaseGun gun) {
			switch ( gun ) {
				case Pistol:
					return new BotOneShotWeaponOperationLogic();
				case MachineGun:
				case Laser:
					return new BotContinuousWeaponOperationLogic();
				case ChargeableGun:
					return new BotChargeWeaponOperationLogic();
				default:
					return null;
			}
		}
	}
}