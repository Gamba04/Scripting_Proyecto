using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private LevelsScrObj levelsInfo;

    [Header("Components")]
    [SerializeField]
    private Building buildingPrefab;
    [SerializeField]
    private Transform buildingParent;

    [Header("Settings")]
    [SerializeField]
    private Vector3 playerBuildingPivot;
    [SerializeField]
    private Vector3 enemyBuildingPivot;
    [SerializeField]
    private Vector3 playerPivot;

    private Building playerBuilding;
    private Building enemyBuilding;

    #region Init

    public void Init(int level, Player player)
    {
        player.transform.position = playerBuildingPivot + playerPivot;

        // Player Building
        playerBuilding = Instantiate(buildingPrefab, buildingParent);
        playerBuilding.name = "Player Building";
        playerBuilding.transform.position = playerBuildingPivot;

        playerBuilding.Init();

        // Enemy Building
        enemyBuilding = Instantiate(buildingPrefab, buildingParent);
        enemyBuilding.name = "Enemy Building";
        enemyBuilding.transform.position = enemyBuildingPivot;

        enemyBuilding.Init(levelsInfo.GetLevel(level));
    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Other

    private void CreateBuilding(Vector3 pivot)
    {

    }

    #endregion

    // ----------------------------------------------------------------------------------------------------------------------------

    #region Editor

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerBuildingPivot, 0.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemyBuildingPivot, 0.5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(playerBuildingPivot + playerPivot, 0.5f);
    }

#endif

    #endregion

}