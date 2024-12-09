using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public GameObject rock;
    public GameObject paper;
    public GameObject scissors;

    public Vector3 rockOriginalPosition;
    public Vector3 paperOriginalPosition;
    public Vector3 scissorsOriginalPosition;
    public Vector3 rockSwapped;
    public Vector3 paperSwapped;
    public Vector3 scissorsSwapped;

    private Vector3 moveUpOffset = new Vector3(0, 2f, 0); // Amount to move up
    public GameManager gameManager;
    

    void Start()
    {
        // Store original positions
        rockOriginalPosition = rock.transform.position;
        paperOriginalPosition = paper.transform.position;
        scissorsOriginalPosition = scissors.transform.position;
    }

    void Update()
    {
        RockPress();
        PaperPress();
        ScissorsPress();
        PaperTied();
    }

    public void RockPress()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            rock.transform.position += moveUpOffset;
            GameManager.Instance.IPlayed = "rock";
            GameManager.Instance.PlayerPlayed = true;
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            rock.transform.position -= moveUpOffset;
        }
    }

    public void PaperPress()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            paper.transform.position += moveUpOffset;
            GameManager.Instance.IPlayed = "paper";
            GameManager.Instance.PlayerPlayed = true;
        }
        else if (Input.GetKeyUp(KeyCode.P))
        {
            paper.transform.position -= moveUpOffset;
        }
    }

    public void ScissorsPress()
    {
        if(Input.GetKeyDown(KeyCode.S) && gameManager.scissorsTied == true){
            scissors.transform.position = scissorsOriginalPosition;
            return;
        }
        //if the scissorsTied bool from the game manager is true then return this script
        else if (Input.GetKeyDown(KeyCode.S) && gameManager.scissorsTied == false)
        {
            scissors.transform.position += moveUpOffset;
            GameManager.Instance.IPlayed = "scissors";
            GameManager.Instance.PlayerPlayed = true;
        }
        else if (Input.GetKeyUp(KeyCode.S) && gameManager.scissorsTied == false)
        {
            scissors.transform.position -= moveUpOffset;
        }
    }
    public void PaperTied(){
        if (gameManager.paperTied == true){
            scissors.transform.position = rockOriginalPosition;
            scissorsSwapped = scissors.transform.position;
            rock.transform.position = paperOriginalPosition;
            rockSwapped = rock.transform.position;
            paper.transform.position = scissorsOriginalPosition;
            paperSwapped = paper.transform.position;
        }
        else if (gameManager.paperTied == false){
            rock.transform.position = rockOriginalPosition;
            paper.transform.position = paperOriginalPosition;
            scissors.transform.position = scissorsOriginalPosition;
        }
    }

    public void Combat()
    {
        // Perform combat logic here
    }

    public void CombatSuccess()
    {
        // Subtract enemy health
    }

    public void CombatFail()
    {
        // Subtract player health
    }

    public void CombatTied()
    {
        // Handle tie effect for each hand
    }

}