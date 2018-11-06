using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    private const string objName = "human006h4_face001(Clone)";
    private Animator anim;

    private void Start()
    {

    }

    public void Action()
    {
        anim = GameObject.Find(objName).GetComponent<Animator>();
        anim.SetTrigger("Attack");
    }
}
