using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Asegure-se de que este namespace está sendo usado para EventSystem
using EntidadesJogo; // Verifique se este é o namespace correto
using UnityEngine.SceneManagement;


public class GameUI : MonoBehaviour
{
    public GameObject homeUI, inGameUI, finishUI, gameOverUI;
    public GameObject TodosButoes;

    private bool buttons;

    [Header("PreGame")]
    public Button soundButton;
    public Sprite soundOnS, soundOffS;

    [Header("InGame")]
    public Image levelSlider;
    public Image currentLevelImg;
    public Image nextLevelImg;
    public Text currentLevelText, nextLevelText;

    [Header("Finish")]
    public Text finishLevelText;

    [Header("GameOver")]
    public Text gameOverScoreText;
    public Text gameOverBestText;

    private Material ballMat;
    private Ball ball;

    void Awake()
    {
        Debug.Log("AWAKE");
        ball = FindObjectOfType<Ball>();
        Debug.Log("Bola: " + ball.transform.GetChild(0).name + "");


        ballMat = FindObjectOfType<Ball>().transform.GetChild(0).GetComponent<MeshRenderer>().material;


        levelSlider.transform.parent.GetComponent<Image>().color = ballMat.color + Color.gray;
        levelSlider.color = ballMat.color;
        currentLevelImg.color = ballMat.color;
        nextLevelImg.color = ballMat.color;

        soundButton.onClick.AddListener(() => GerirSom.instance.SoundOnOff());
    }

    private void Start()
    {
        currentLevelText.text = FindObjectOfType<GeradorNiveis>().level.ToString();
        nextLevelText.text = FindObjectOfType<GeradorNiveis>().level + 1 + "";
    }

    void Update()
    {
        if (ball.ballState == Ball.BallState.Prepare)
        {
            if (GerirSom.instance.sound && soundButton.GetComponent<Image>().sprite != soundOnS)
                soundButton.GetComponent<Image>().sprite = soundOnS;
            else if (!GerirSom.instance.sound && soundButton.GetComponent<Image>().sprite != soundOffS)
                soundButton.GetComponent<Image>().sprite = soundOffS;
        }

        if (Input.GetMouseButtonDown(0) && !IgnoreUI() && ball.ballState == Ball.BallState.Prepare)
        {
            ball.ballState = Ball.BallState.Playing;
            homeUI.SetActive(false);
            inGameUI.SetActive(true);
            finishUI.SetActive(false);
            gameOverUI.SetActive(false);
        }

        if (ball.ballState == Ball.BallState.Finish)
        {
            homeUI.SetActive(false);
            inGameUI.SetActive(false);
            finishUI.SetActive(true);
            gameOverUI.SetActive(false);

            finishLevelText.text = "Level " + FindObjectOfType<GeradorNiveis>().level;
        }

        if (ball.ballState == Ball.BallState.Died)
        {
            homeUI.SetActive(false);
            inGameUI.SetActive(false);
            finishUI.SetActive(false);
            gameOverUI.SetActive(true);

            gameOverScoreText.text = GerirPontuacao.instance.score.ToString();
            gameOverBestText.text = PlayerPrefs.GetInt("HighScore").ToString();

            if (Input.GetMouseButtonDown(0))
            {
                GerirPontuacao.instance.ResetScore();
                SceneManager.LoadScene(0);
            }
        }
    }

    private bool IgnoreUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);
        for (int i = 0; i < raycastResultList.Count; i++)
        {
            if (raycastResultList[i].gameObject.GetComponent<Ignore>() != null)
            {
                raycastResultList.RemoveAt(i);
                i--;
            }
        }

        return raycastResultList.Count > 0;
    }

    public void LevelSliderFill(float fillAmount)
    {
        levelSlider.fillAmount = fillAmount;
    }

    public void Settings()
    {
        buttons = !buttons;
        TodosButoes.SetActive(buttons);
    }
}




