using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
	public abstract class WeaponEffect: MonoBehaviour
	{
		[HideInInspector]
		public PlayerStats player;
		[HideInInspector]
		public Weapon weapon;

		public float GetDamage()
		{
			return weapon.GetDamage();
		}
	}
}
