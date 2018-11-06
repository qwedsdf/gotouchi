using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    private GameObject player;
    private Player playerScript;
    private CharacterStatus playerStatus;
    private CharacterStatus cpuStatus;
    public LogDisplay logDisp;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player>();
        playerStatus = player.GetComponent<CharacterStatus>();
        cpuStatus = GameObject.Find("human2").GetComponent<CharacterStatus>();
    }

    public void AttackButton()
    {
        playerScript.state = 1;
    }

    public void AvoidanceButton()
    {
        playerScript.state = 2;
    }

    public void AttackButton1()
    {
        logDisp.DebagLog("AttackType", 6);
        playerStatus.SetAttack(20.0f);
        playerStatus.SetDefense(10.0f);
        playerStatus.SetAgility(10.0f);
        playerStatus.SetAttackedAngle(7.0f);
        playerStatus.SetLimitAngle(30.0f);
    }

    public void AttackButton2()
    {
        logDisp.DebagLog("AttackType", 7);
        cpuStatus.SetAttack(20.0f);
        cpuStatus.SetDefense(10.0f);
        cpuStatus.SetAgility(10.0f);
        cpuStatus.SetAttackedAngle(7.0f);
        cpuStatus.SetLimitAngle(30.0f);
    }

    public void DefenseButton1()
    {
        logDisp.DebagLog("DefenseType", 6);
        playerStatus.SetAttack(15.0f);
        playerStatus.SetDefense(15.0f);
        playerStatus.SetAgility(10.0f);
        playerStatus.SetAttackedAngle(7.0f);
        playerStatus.SetLimitAngle(30.0f);
    }

    public void DefenseButton2()
    {
        logDisp.DebagLog("DefenseType", 7);
        cpuStatus.SetAttack(15.0f);
        cpuStatus.SetDefense(15.0f);
        cpuStatus.SetAgility(10.0f);
        cpuStatus.SetAttackedAngle(7.0f);
        cpuStatus.SetLimitAngle(30.0f);
    }

    public void AgilityButton1()
    {
        logDisp.DebagLog("AgilityType", 6);
        playerStatus.SetAttack(15.0f);
        playerStatus.SetDefense(10.0f);
        playerStatus.SetAgility(10.0f);
        playerStatus.SetAttackedAngle(7.0f);
        playerStatus.SetLimitAngle(30.0f);
    }

    public void AgilityButton2()
    {
        logDisp.DebagLog("AgilityType", 7);
        cpuStatus.SetAttack(15.0f);
        cpuStatus.SetDefense(10.0f);
        cpuStatus.SetAgility(10.0f);
        cpuStatus.SetAttackedAngle(7.0f);
        cpuStatus.SetLimitAngle(30.0f);
    }

    public void Enable()
    {
        Transform childTransform = gameObject.GetComponentInChildren<Transform>();

        foreach (Transform child in childTransform)
        {
            child.GetComponent<Button>().interactable = true;
        }
    }

    public void Disable()
    {
        Transform childTransform = gameObject.GetComponentInChildren<Transform>();

        foreach (Transform child in childTransform)
        {
            child.GetComponent<Button>().interactable = false;
        }
    }
}
