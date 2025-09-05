using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class DOFManager : MonoBehaviour
{
    public DepthOfFieldDeprecated dof;

    public void EnableDepthOfFiled()
    {
        dof.enabled = true;
    }
    public void DisableDepthOfFiled()
    {
        dof.enabled = false;
    }
}
