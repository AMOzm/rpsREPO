using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // Sprites
    public Sprite rockSprite;
    public Sprite paperSprite;
    public Sprite scissorsSprite;
    // Card in combat
    public string IPlayed;
    public string OppPlayed;
    // Health
    public int MyMaxHealth;
    public int myHealth;
    public int OppMaxHealth;
    public int oppHealth;
    public Slider MyHealthSlider; // Player health slider
    public Slider OppHealthSlider; // Opponent health slider
    public TextMeshProUGUI ResultText;

    // Rhythm System
    public bool PlayerSuccess;
    public bool PlayerFail;
    public bool PlayerTied;
    public bool isPlaying = false;
    private float bpm = 100f;
    private int beatCount = 0;
    public bool PlayOnOne;
    public bool PlayerPlayed;
    private bool myTurn;
    private bool resultsCalled;
    [SerializeField]private string sceneName;
    [SerializeField] private OppController oc;
    [SerializeField] private PlayerController pc;
    [SerializeField] private Animator faceAnimator;
    public bool scissorsTied = false;
    private int TiedScount = 0;
    public bool paperTied = false;
    private int TiedPcount = 0;
    public bool rockTied = false;
    private int TiedRcount = 0;
    [SerializeField] GameObject indicator;

    //audio
    [SerializeField] private AudioSource Metronome;
    [SerializeField] private AudioSource BGMSong;
    private int PrepPhase = 0;
    [SerializeField] private int StartAt;
    private bool PlayStarted;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //in the lines above, we check to see what scene we are in so that later (in Update) we can change scene depending on the scene we're in
        ResultText.text = "";
        myHealth = MyMaxHealth;
        oppHealth = OppMaxHealth;
        // Initialize sliders
        MyHealthSlider.maxValue = MyMaxHealth;
        MyHealthSlider.value = myHealth;
        OppHealthSlider.maxValue = OppMaxHealth;
        OppHealthSlider.value = oppHealth;

        float interval = 60f / bpm;
        InvokeRepeating("PlayBeat", interval, interval);
        faceAnimator.SetBool("ifTie", false);
        faceAnimator.SetBool("ifLose", false);
        faceAnimator.SetBool("ifWin", false);
        Metronome.Play();
        BGMSong.Play();
    }

    void PlayBeat()
    {
        isPlaying = true;
        beatCount = (beatCount % 8) + 1;
        //speed = distance/time
        //so we want the indicators speed to be the distance between its spawn point and the middle, divided by the beat length
        //speed will be calculated
        //distance = spawn point - middle, but maybe its actually a bit before the middle so that the player has some leeway
        //time = bpm/60
        //speed = (spawnpoint-middle)/(bpm/60)

        switch (beatCount)
        {
            case 1:
                //rockSprite.rotation = Quaternion.Euler(0, 0, -5); Is this were I should rotate the sprites in an animation?
                if(PlayStarted){
                    myTurn = false;
                    PlayerPlayed = false;
                    IPlayed = "none";
                    EnemyPOne();
                }
                break;
            case 2:
                if(PlayStarted){
                    myTurn = false;
                    PlayerPlayed = false;
                    IPlayed = "none";
                    EnemyPTwo();
                }
                break;
            case 3:
                if(PlayStarted){
                    if(PlayOnOne){
                        myTurn = true;
                        if(scissorsTied == true){
                            TiedScount = 1;
                        }
                        if(paperTied == true){
                            TiedPcount = 1;
                        }
                        if(rockTied == true){
                            TiedRcount = 1;
                        }

                    }
                    else{
                        myTurn = false;
                    }
                    PlayerPlayed = false;
                    IPlayed = "none";
                }
                break;
            case 4:
                if(PlayStarted){
                    if(!PlayOnOne){
                        myTurn = true;
                        if(scissorsTied == true){
                            TiedScount = 1;
                        }
                        if(paperTied == true){
                            TiedPcount = 1;
                        }
                        if(rockTied == true){
                            TiedRcount = 1;
                        }

                    }
                    else{
                        myTurn = false;
                        if(IPlayed == "none"){
                            ResultText.text = "Failed to Play.";
                            myHealth --;
                        }
                    }
                    PlayerPlayed = false;
                    IPlayed = "none";
                }
                break;
            case 5:
                if(PlayStarted){
                    if(!PlayOnOne){
                        if(IPlayed == "none"){
                            ResultText.text = "Failed to Play.";
                            myHealth --;
                        }
                    }
                    PlayerPlayed = false;
                    IPlayed = "none";
                    if(scissorsTied == true && TiedScount == 1){
                        pc.scissors.transform.position = pc.scissorsOriginalPosition;
                        scissorsTied = false;
                        TiedScount = 0;
                    }
                    if(paperTied == true && TiedPcount == 1){
                        paperTied = false;
                        TiedPcount = 0;
                    }
                    if(rockTied == true && TiedRcount == 1){
                        paperTied = false;
                        TiedPcount = 0;
                    }
                }
                break;
            case 8:
                if(!PlayStarted){
                        PrepPhase ++;
                }
                faceAnimator.SetBool("ifTie", false);
                faceAnimator.SetBool("ifLose", false);
                faceAnimator.SetBool("ifWin", false);
                break;
            default:
                if(PlayStarted){
                    IPlayed = "none";
                    OppPlayed = "none";
                    resultsCalled = false;
                    oc.ResetOppCard();
                    myTurn = false;
                    ResultText.text = "";
                    PlayerPlayed = false;
                    if(!paperTied){
                        pc.ResetHandPos();
                    }
                }
                break;
        }

        //Debug.Log("Beat: " + beatCount);
    }

    void Update()
    {
        if(PrepPhase == StartAt){
            PlayStarted = true;
        }
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        UpdateHealthText();

        // if my health is zero, restart, if enemys health is 0, go to next level
        WinStateCheck();
        LossStateCheck();
        

        // Ensure player can play only during the valid beats (3 or 4)
        if (myTurn && PlayerPlayed && !resultsCalled)
        {
            Results();
            PlayerPlayed = false;  // Reset after processing
        }
        if(!myTurn && PlayerPlayed){
            ResultText.text = "Not Good!";
        }
        if(myHealth <= 0 || oppHealth <= 0){
            SceneManager.LoadScene("sceneName");
        }
    }
    void LossStateCheck(){
        if (myHealth <= 0){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else{
            return;
        }
    }
    void WinStateCheck(){
        if (sceneName == "RockEnemy" && oppHealth <= 0){
            SceneManager.LoadScene("PaperEnemy");
            Debug.Log("got here");
            oppHealth = 3;
        }
        if (sceneName == "PaperEnemy" && oppHealth <= 0){
            oppHealth = 3;
            SceneManager.LoadScene("ScissorsEnemy");
        }
        if (sceneName == "ScissorsEnemy" && oppHealth <= 0){
            oppHealth = 5;
            SceneManager.LoadScene("FirstBoss");
        }
        else{
            return;
        }
    }

    void OnDisable()
    {
        CancelInvoke("PlayBeat");
    }

    private void EnemyPOne()
    {
        if (PlayOnOne)
        {
            oc.PlayCard();
        }
    }

    private void EnemyPTwo()
    {
        if (!PlayOnOne)
        {
            oc.PlayCard();
        }
    }

    private void Results()
    {
        resultsCalled = true;
        Debug.Log("we got here");
        if((rockTied && IPlayed != "rock")){
            myHealth --;
            rockTied = false;
            faceAnimator.SetBool("ifLose", true);
            ResultText.text = "Failed to bounce rock back!";
        }
        else if((rockTied && IPlayed == "rock")){
            oppHealth --;
            rockTied = false;
            faceAnimator.SetBool("ifWin", true);
            ResultText.text = "Rock bounced back!";
        }
        else if ((IPlayed == "rock" && OppPlayed == "scissors") || (IPlayed == "scissors" && OppPlayed == "paper") || (IPlayed == "paper" && OppPlayed == "rock"))
        {
            Debug.Log("we got further");
            oppHealth--;
            faceAnimator.SetBool("ifWin", true);
            //PlayerSuccess = true;
            ResultText.text = "Win!";
        }
        else if ((IPlayed == "scissors" && OppPlayed == "rock") || (IPlayed == "paper" && OppPlayed == "scissors") || (IPlayed == "rock" && OppPlayed == "paper"))
        {
            Debug.Log("we got further");
            myHealth--;
            faceAnimator.SetBool("ifLose", true);
            //PlayerFail = true;
            ResultText.text = "Lose!";
        }
        else if ((IPlayed == "scissors" && OppPlayed == "scissors"))
        {
            Debug.Log("we got further");
            //PlayerTied = true;
            faceAnimator.SetBool("ifTie", true);
            scissorsTied = true;

            //deactivate scissors next round. Maybe make a boolean that says
            //"tied scissors" and its true, and then make it false if its true
            ResultText.text = "Tie! No Scissors 1 round";
        }
        else if ((IPlayed == "paper" && OppPlayed == "paper"))
        {
            Debug.Log("we got further");
            //PlayerTied = true;
            //change the location of the sprites
            faceAnimator.SetBool("ifTie", true);
            paperTied = true;
            pc.PaperTied();
            ResultText.text = "Tie! Mixing Up the hands!";
        }
        else if ((IPlayed == "rock" && OppPlayed == "rock") && !rockTied)
        {
            Debug.Log("we got further");
            faceAnimator.SetBool("ifTie", true);
            rockTied = true;
            //PlayerTied = true;
            //rocks bounce back 
            ResultText.text = "Tie! Play Rock Again!";
        }
        else if (IPlayed == "none"){
            ResultText.text = "Failed.";
            faceAnimator.SetBool("ifLose", true);
            myHealth --;
        }


        // Reset states for next turn
        // PlayerSuccess = false;
        // PlayerFail = false;
        // PlayerTied = false;
    }

    public void UpdateHealthText()
    {
        MyHealthSlider.value = myHealth;
        OppHealthSlider.value = oppHealth;
    }
    //demonstration
}
