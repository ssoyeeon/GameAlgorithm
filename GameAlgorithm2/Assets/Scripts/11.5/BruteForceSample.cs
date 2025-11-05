using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class BruteForceSample : MonoBehaviour
{
    int QUICK_COST = 2;
    int QUICK_DMG = 6;
    int HEAVY_COST = 3;
    int HEAVY_DMG = 8;
    int MULTI_COST = 5;
    int MULTI_DMG = 16;
    int TRIPLE_COST = 7;
    int TRIPLE_DMG = 24;

    int MAX_QUICK = 2;
    int MAX_HEAVY = 2;
    int MAX_MULTI = 1;
    int MAX_TRIPLE = 1;

    int maxCost = 15;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindBestCombo();
        }
    }

    public void FindBestCombo()
    {
        int bestDamage = 0;
        int bestCost = 0;
        int bestQuick = 0, bestHeavy = 0, bestMulti = 0, bestTriple = 0;
        int totalChecks = 0;

        Debug.Log("--- 콤보 탐색 시작 ---");

        for (int i_quick = 0; i_quick <= MAX_QUICK; i_quick++)
        {
            for (int j_heavy = 0; j_heavy <= MAX_HEAVY; j_heavy++)
            {
                for (int k_multi = 0; k_multi <= MAX_MULTI; k_multi++)
                {
                    for (int l_triple = 0; l_triple <= MAX_TRIPLE; l_triple++)
                    {
                        totalChecks++;
                        
                        Debug.Log($"[검사 {totalChecks}]: 퀵{i_quick}, 헤비{j_heavy}, 멀티{k_multi}, 트리플{l_triple}");

                        int currentCost = (i_quick * QUICK_COST) +
                                        (j_heavy * HEAVY_COST) +
                                        (k_multi * MULTI_COST) +
                                        (l_triple * TRIPLE_COST);

                        if (currentCost <= maxCost)
                        {
                            int currentDamage = (i_quick * QUICK_DMG) +
                                              (j_heavy * HEAVY_DMG) +
                                              (k_multi * MULTI_DMG) +
                                              (l_triple * TRIPLE_DMG);

                            if (currentDamage > bestDamage)
                            {
                                // ===== 로그 추가 2: 최고 데미지가 갱신되는 순간 =====
                                Debug.Log($"(Damage: {currentDamage} (Cost: {currentCost})");

                                bestDamage = currentDamage;
                                bestCost = currentCost;
                                bestQuick = i_quick;
                                bestHeavy = j_heavy;
                                bestMulti = k_multi;
                                bestTriple = l_triple;
                            }
                        }
                    }
                }
            }
        }
    }
}
