using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppController : MonoBehaviour
{
    public GameObject OppCardInPlay;  // The card to change
    private List<string> CardPool = new List<string>();  // Holds rps
    private List<string> discardPool = new List<string>(); // Holds discarded rps
    [SerializeField]private int RNum; //The amount of rocks in opponent hand
    [SerializeField]private int SNum; //The amount of scissors in opponent hand
    [SerializeField]private int PNum; //The amount of papers in opponent hand
    // Start is called before the first frame update
    void Start()
    {
        OppCardInPlay.GetComponent<SpriteRenderer>().color = Color.black;
        //Fill Rocks
        for (int i = 0; i < RNum; i++)
        {
            CardPool.Add("Rock");
        }
        //Fill Scissors
        for (int i = 0; i < SNum; i++)
        {
            CardPool.Add("Scissors");
        }
        //Fill Papers
        for (int i = 0; i < PNum; i++)
        {
            CardPool.Add("Paper");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayCard(){
        // Check if rockTied is true
        if (GameManager.Instance.rockTied)
        {
            Debug.Log("Playing an extra round with Rock due to rockTied.");

            // Play a "Rock" card without removing it from the pool
            OppCardInPlay.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            OppCardInPlay.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.rockSprite;
            GameManager.Instance.OppPlayed = "rock";
            
            // Treat this round as an extra round
            return;
        }
        else{
        if (CardPool.Count == 0)
        {
            Debug.Log("Card pool is empty. Refilling from discard pool.");
            FillOppCardPool();
            discardPool.Clear();
        }
        int randomIndex = Random.Range(0, CardPool.Count);
        string randomCard = CardPool[randomIndex];

        // Remove the chosen card from CardPool and add to discardPool
        CardPool.RemoveAt(randomIndex);
        discardPool.Add(randomCard);

        // Set the sprite based on the random selection
        switch (randomCard)
        {
            case "Rock":
                OppCardInPlay.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                OppCardInPlay.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.rockSprite;
                GameManager.Instance.OppPlayed = "rock";
                break;
            case "Paper":
                OppCardInPlay.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                OppCardInPlay.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.paperSprite;
                GameManager.Instance.OppPlayed = "paper";
                break;
            case "Scissors":
                OppCardInPlay.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                OppCardInPlay.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.scissorsSprite;
                GameManager.Instance.OppPlayed = "scissors";
                break;
        }
        }
    }
    public void ResetOppCard(){
        OppCardInPlay.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
    }
    // Refills CardPool with 8 of each card type
    private void FillOppCardPool()
    {
        CardPool.Clear();
        //Fill Rocks
        for (int i = 0; i < RNum; i++)
        {
            CardPool.Add("Rock");
        }
        //Fill Scissors
        for (int i = 0; i < SNum; i++)
        {
            CardPool.Add("Scissors");
        }
        //Fill Papers
        for (int i = 0; i < PNum; i++)
        {
            CardPool.Add("Paper");
        }
        
    }
}
