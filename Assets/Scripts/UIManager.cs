using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Collections;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI npcHealthText;

    public GameObject playerWon;
    public GameObject npcWon;
    public GameObject tiePanel;
    

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    public void UpdateHealthUI()
    {
        int playerHP = PlayerManager.healthValue;
        
        if (playerHealthText != null && playerHP != 0)
        {

            playerHealthText.text = playerHP.ToString();
        }

    }

    public void UpdateNPCHealthUI()
    {
        int npcHP = CPUManager.npcHealthValue;

        if (npcHealthText != null && npcHP != 0)
        {

            npcHealthText.text = npcHP.ToString();
        }

    }

    public void GetPlayerWon()
    {
        playerWon.SetActive(true);
    }

    public void TurnOffPlayerWon()
    {
        StartCoroutine(TurnoffPlayerWon());
    }

    public void GetNPCWon()
    {
        npcWon.SetActive(true);
    }

    public void TurnOffNPCWon()
    {
        StartCoroutine(TurnoffNPCWon());
    }

    public void GetTIE()
    {
        tiePanel.SetActive(true);
    }

    public void TurnOffTIE()
    {
        StartCoroutine(TurnoffTie());
    }


    private IEnumerator TurnoffPlayerWon()
    {
        yield return new WaitForSeconds(3f); 
        playerWon.SetActive(false);
    }

    private IEnumerator TurnoffNPCWon()
    {
        yield return new WaitForSeconds(3f); 
        npcWon.SetActive(false);
    }

    private IEnumerator TurnoffTie()
    {
        yield return new WaitForSeconds(3f);
        tiePanel.SetActive(false);
    }


}
