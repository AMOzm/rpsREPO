using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class DeckHandler : MonoBehaviour
{
    private List <HandHandler> hands;
    public DeckHandler(){
        hands = new List<HandHandler>();
        string[] hand = {"Rock", "Paper", "Scissors"};
        Shuffle();
    }
    
    private void Shuffle(){
        for (int i = 0; i < hands.Count; i++){
            HandHandler temp = hands [i];
            int randomIndex = Random.Range(i, hands.Count);
            hands[i] = hands[randomIndex];
            hands[randomIndex] = temp;
        }
    }

    public HandHandler DrawCard(){
        if (hands.Count == 0) return null;
        HandHandler drawnHand = hands[0];
        hands.RemoveAt(0);
        return drawnHand;
    }
}
