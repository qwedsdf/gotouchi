using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    // 体力
    private float hp;
    // 攻撃力
    private float attack;
    // 防御力
    private float defense;
    // すばやさ
    private float agility;
    // 攻撃時の変更角度
    private float attackedAngle;
    // 制限角度
    private float limitAngle;


    // Setter
    public void SetHP(float hp)
    {
        this.hp = hp;
    }

    public void SetAttack(float attack)
    {
        this.attack = attack;
    }

    public void SetDefense(float defense)
    {
        this.defense = defense;
    }

    public void SetAgility(float agility)
    {
        this.agility = agility;
    }

    public void SetAttackedAngle(float attackedAngle)
    {
        this.attackedAngle = attackedAngle;
    }

    public void SetLimitAngle(float limitAngle)
    {
        this.limitAngle = limitAngle;
    }


    // Getter
    public float GetHP()
    {
        return hp;
    }

    public float GetAttack()
    {
        return attack;
    }

    public float GetDefense()
    {
        return defense;
    }

    public float GetAgility()
    {
        return agility;
    }

    public float GetAttackedAngle()
    {
        return attackedAngle;
    }

    public float GetLimitAngle()
    {
        return limitAngle;
    }
}
