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
		public PlayerStats owner;
		[HideInInspector]
		public Weapon weapon;

        public PlayerStats Owner { get { return owner; } }

        public float GetDamage()
		{
			return weapon.GetDamage();
		}
	}
}
