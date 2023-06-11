using GameJamEntry.Gameplay.WeaponsMechanic;
using UnityEngine;

namespace GameJamEntry.Gameplay.BasicAi {
	public class BotChargeWeaponOperationLogic : BaseBotWeaponOperationLogic {
		const float ChargeTime = 1f;
		
		bool _chargeStarted;

		float _leftTime;

		public override void Init(BaseGun gun) {
			base.Init(gun);
			_chargeStarted = false;
			_leftTime      = 0f;
		}

		public override void Tick() {
			if ( !_chargeStarted ) {
				Gun.StartFire();
				_chargeStarted = true;
				_leftTime      = ChargeTime;
				return;
			}
			_leftTime -= Time.deltaTime;
			if ( _leftTime > 0 ) {
				return;
			}
			Gun.EndFire();
			_chargeStarted = false;
		}
		
	}
}