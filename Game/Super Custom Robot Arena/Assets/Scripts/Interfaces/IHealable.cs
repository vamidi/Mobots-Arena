using UnityEngine;
using System.Collections;

public interface IHealable<T> {
	void Heal(T health);
	void ArmorHeal(T health);
	void IncreaseDamage(T multiplier);
}
