using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HelperClasses
{
    // CLOSEST: prioritise closest enemy. Will switch target if there is another enemy that is closest
    // STRONGEST: prioritise strongest enemy. Will switch target if another enemy that is stronger enter its detect range
    // WEAKEST: priorities weakest enemy. Will switch target if another enemy that is weaker enter its detect range
    public enum TargetPriority { CLOSEST, STRONGEST, WEAKEST }

    [System.Serializable]
    public enum ItemType {
        WEAPONS,
        SHIPS,
        CARRIERS
    }

    public enum Team {
        BLUE,
        RED
    }

}
