using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    #region map boundary constants
    public const float xLimit = 165f;
    public const float yLimit = 165f;
    #endregion

    #region ship movement ai constants
    public const float searchInterval = 1f;
    public const float searchDelay = 0.5f;
    public const float roamOffset = 20f;
    public const float distanceFromRoam = 2f;
    #endregion

    #region weapon constants
    public const float weaponSearchInterval = 0.5f;
    public const float idleRotateInterval = 4f;
    #endregion

    public const float patrolOffset = 2f;
}
