using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_Deactivator : MonoBehaviour, IShotable
{
    public interactiontype t=interactiontype.deactivate;

    [SerializeField]
    GameObject objToDeactive = null;
    [SerializeField]
    GameObject ActivateThis=null;
    public string effectName = "PUFF";
    bool used = false;

    //activate/deactivate on click object
    public void interact(Vector3 hitPos)
    {
        if (used) return;
        used = true;
        switch (t)
        {
            case interactiontype.activate:
                if(ActivateThis!=null)
                ActivateThis.SetActive(true);
                if (objToDeactive == null) return;
                objToDeactive.SetActive(false);
                break;
            case interactiontype.deactivate:
                if (objToDeactive == null) return;
                objToDeactive.SetActive(false);
                break;
        }
       
        if (EffectDirector.instance)
        {
            EffectDirector.instance.EffectAndPopup(hitPos, effectName, 200);
        }
        Destroy(gameObject);
    }

    public enum interactiontype
    {
        activate,
        deactivate
    }
}
