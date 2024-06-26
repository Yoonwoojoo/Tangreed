using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatHandler : MonoBehaviour
{
    // 기본스탯과 추가 스탯을 계산해서 최종 스탯을 계산하는 로직

    [SerializeField] private PlayerStat baseStat;
    public PlayerStat CurrentStat { get; private set; } = new();
    public List<PlayerStat> statModifiers = new List<PlayerStat>();

    private readonly float MinAttackDelay = 1f;
    private readonly float MinCritical = 5f; // 공격 딜레이 최소값
    private readonly float MinAttackPower = 0.5f; //공격력 최소값
    private readonly float MinAttackSize = 0.4f; //공격 크기 최소값
    private readonly float MinAttackSpeed = 0.1f; // 공격 스피드 최소값
    private readonly float MinSpeed = 0.8f; //속도 최소값
    private readonly float MinDamage = 3;
    private readonly float MinDefense = 2;
    private readonly int MinMaxDashCount = 2;
    private readonly int MinMaxHealth = 50; // 최대 체력의 최소값

    private void Awake()
    {
        //if (baseStat.playerSO != null)
        //{
        //    baseStat.playerSO = Instantiate(baseStat.playerSO);
        //    CurrentStat.playerSO = Instantiate(baseStat.playerSO);
        //}
        UpdateCharacterStat();
    }

    private void UpdateCharacterStat()
    {
        // 베이스 스텟 먼저 적용함
        ApplyStatModifiers(baseStat);

        // 변경되는 수치들을 반영
        // 이때 statsChangeType의 순서 0 : Add, 1 : Multiple, 2 : Override를 반영하여 0, 1, 2 순으로 정렬함.
        foreach (var modifier in statModifiers.OrderBy(o => o.statsChangeType)) //작은 수 부터 큰 수 순으로 정렬
        {
            ApplyStatModifiers(modifier);
        }
    }
    public void AddStatModifier(PlayerStat statModifier) //외부에서 스탯을 받아옴
    {
        statModifiers.Add(statModifier);
        UpdateCharacterStat();
    }
    public void RemoveStatModifier(PlayerStat statModifier) //스탯을 제거함
    {
        statModifiers.Remove(statModifier);
        UpdateCharacterStat();
    }

    private void ApplyStatModifiers(PlayerStat modifier)
    {
        Func<float, float, float> operation = modifier.statsChangeType switch
        {
            StatsChangeType.Add => (current, change) => current + change,
            StatsChangeType.Multiple => (current, change) => current * change,
            _ => (current, change) => change,
        };

        UpdateBasicStats(operation, modifier);
        //UpdateAttackStats(operation, modifier);
    }

    private void UpdateBasicStats(Func<float, float, float> operation, PlayerStat modifier)
    {
        CurrentStat.Damage = Mathf.Max((int)operation(CurrentStat.Damage, modifier.Damage), MinDamage);
        CurrentStat.Defense = Mathf.Max((int)operation(CurrentStat.Defense, modifier.Defense), MinDefense);
        CurrentStat.maxHealth = Mathf.Max((int)operation(CurrentStat.maxHealth, modifier.maxHealth), MinMaxHealth); //Mathf.Max를 통해서 최소값을 결정. 5 미만으로 떨어지면 5가 적용
        CurrentStat.Critical = Mathf.Max(operation(CurrentStat.Critical, modifier.Critical), MinCritical);
        CurrentStat.speed = Mathf.Max(operation(CurrentStat.speed, modifier.speed), MinSpeed);
        CurrentStat.DashMax = Mathf.Max(operation(CurrentStat.DashMax, modifier.DashMax), MinMaxDashCount);
    }

    //private void UpdateAttackStats(Func<float, float, float> operation, PlayerStat modifier)
    //{
    //    if (CurrentStat.playerSO == null || modifier.playerSO == null) return;

    //    var currentAttack = CurrentStat.playerSO;
    //    var newAttack = modifier.playerSO;

    //     변경을 적용하되, 최소값을 적용함
    //    currentAttack.delay = Mathf.Max(operation(currentAttack.delay, newAttack.delay), MinAttackDelay);
    //    currentAttack.power = (int)Mathf.Max(operation(currentAttack.power, newAttack.power), MinAttackPower);
    //}
}