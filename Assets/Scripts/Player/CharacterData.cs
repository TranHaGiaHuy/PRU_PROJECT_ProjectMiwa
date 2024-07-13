using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Data/CharacterData")]
public class CharacterData : ScriptableObject
{
	[SerializeField]
	Sprite icon;
	public Sprite Icon { get => icon; private set => icon = value; }

	public RuntimeAnimatorController controller;

	[SerializeField]
	new string name;
	public string Name { get => name; private set => name = value; }

	[SerializeField]
	WeaponData startingWeapon;
	public WeaponData StartingWeapon { get => startingWeapon; private set => startingWeapon = value; }

	[System.Serializable]
	public struct Stats
	{
		public float maxHealth, recovery, moveSpeed, might, projectileSpeed, collectRange;

		public Stats(float maxHealth = 1000, float recovery = 0, float moveSpeed = 1f, float might = 1f, float projectileSpeed = 1f, float collectRange = 30f)
		{
			this.maxHealth = maxHealth;
			this.recovery = recovery;
			this.moveSpeed = moveSpeed;
			this.might = might;
			this.projectileSpeed = projectileSpeed;
			this.collectRange = collectRange;
		}

		public static Stats operator +(Stats s1, Stats s2)
		{
			s1.maxHealth += s2.maxHealth;
			s1.recovery += s2.recovery;
			s1.moveSpeed += s2.moveSpeed;
			s1.might += s2.might;
			s1.projectileSpeed += s2.projectileSpeed;
			s1.collectRange += s2.collectRange;
			return s1;
		}
	}
	public Stats stats = new Stats(1000);
}
