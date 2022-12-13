using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HelperClasses;

public interface IItem
{
    int Cost {
        get;
        set;
    }

    string Description {
        get;
    }

    int ResaleValue {
        get;
        set;
    }

    ItemType Type {
        get;
        set;
    }

}
