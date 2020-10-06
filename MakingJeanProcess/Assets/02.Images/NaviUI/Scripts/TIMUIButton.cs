using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TIMUIButton : Button
{
    public void SetButtonStatus(bool isSelect)
    {
        DoStateTransition((isSelect)? SelectionState.Selected : SelectionState.Normal, true);
    }
    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
    }


}
