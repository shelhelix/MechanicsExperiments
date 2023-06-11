using GameJamEntry.Gameplay.WeaponsMechanic;
using UnityEngine;

namespace GameJamEntry.Gameplay.BasicAi {
	public class BotOneShotWeaponOperationLogic : BaseBotWeaponOperationLogic {
		const float DelayBetweenShots = 1f;

		float _leftTime;

		public override void Init(BaseGun gun) {
			base.Init(gun);
			_leftTime = 0;
		}

		public override void Tick() {
			// limit fire rate with 1 shot per second for AI
			if ( _leftTime < 0 ) {
				Gun.StartFire();
				Gun.EndFire();
				_leftTime = DelayBetweenShots;
			} else {
				_leftTime -= Time.deltaTime;
			}
		}
	}
}