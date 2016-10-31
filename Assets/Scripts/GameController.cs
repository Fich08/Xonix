using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public GameObject[] enemys;
	public GameObject Player;

	public float levelStartDelay = 2f;
	public BoardManager boardScript;
	public Text HPText;
	public Text TimeText;
	public Text ProgresText;
	public Text RestartText;

	private int level = 0;
	private Text levelText;
	private GameObject levelImage;
	private GameObject ButtonPause;
	private float timeGame;
	private int startWaterBlock;
	private PlayerController playerController;


	private List<Enemy> enemies;
	private int [,] boardMas;
	private GameObject[,] boardObject;
	private int[,] boardMasVerification;
	private int matrix = 30;
	private bool startGame=false;
	private int playerHP;
	private bool game_Over;


	const int CONST_SAND = 0;
	const int CONST_WATER = 1;
	const int CONST_COURSE = 2;
	const int CONST_ENEMY = 3;
	const int CONST_WATER_V = 4;


	// Use this for initialization

	void Awake () {

		enemies = new List<Enemy>();
		playerHP = 3;
		boardScript = GetComponent<BoardManager>();
	}

	void Start () {
		game_Over = false;
		GameObject GamePlayer =	Instantiate (Player,Player.transform.position,Quaternion.identity) as GameObject;
		playerController = GamePlayer.GetComponent<PlayerController> ();

		levelImage = GameObject.Find("LevelImage");
		levelText = GameObject.Find("LevelText").GetComponent<Text>();
		ButtonPause = GameObject.Find ("ButtonPause");
	}


	void Update () {

		#if UNITY_STANDALONE

		if (game_Over) {
			if(Input.GetButtonDown("Fire1")){
				level = 0;
				InitGame ();
			}

		}

		#else


		if (game_Over) {
			if (Input.touchCount > 0) {
				Touch myTouch = Input.touches[0];
				if (myTouch.phase == TouchPhase.Ended) {
					level = 0;
					Invoke("InitGame", 1f);
				}
			}
		}

		#endif		

	}


	void InitGame()	{		

		game_Over = false;
		level++;

		playerController.toStartPos ();
		levelText.text = "Level " + level;
		controlUi();
		RestartText.text = "";
		timeGame = 60f;
		startWaterBlock = ((matrix-boardScript.offsetOfSand*2)*(matrix-boardScript.offsetOfSand*2));
		Invoke("HideLevelImage",levelStartDelay);
		playerHP = 3;
		boardScript.setupScene(level);
		boardMas = boardScript.board;
		boardObject = boardScript.boardObj;

		StartCoroutine (updateTimeText());
	}	

	private void HideLevelImage()
	{
		levelImage.SetActive(false);
		startGame = true;
		updateHPText ();
		updateProgresText ();
		ButtonPause.SetActive (true);
		setLiveObject ();

	}

	public int getTypeCordinate(int x,int y){
		
		return boardMas [x, y];
	}

	public void setTypeCordinate(int x,int y,int type){
		boardMas [x, y] = type;
	}

	public void verificatio(){
		boardMasVerification = new int[matrix,matrix];
		boardMasVerification = copyArray(boardMas);
		GameObject[] enemyss = GameObject.FindGameObjectsWithTag ("enemy");
		for (int i = 0; i < enemyss.Length; i++) {
			GameObject enemyInGame = enemyss[i];
			cellSelection ((int)enemyInGame.transform.position.x + 1,(int) enemyInGame.transform.position.y);
			cellSelection ((int)enemyInGame.transform.position.x-1,(int) enemyInGame.transform.position.y);
			cellSelection ((int)enemyInGame.transform.position.x, (int) enemyInGame.transform.position.y-1);
			cellSelection ((int) enemyInGame.transform.position.x,(int)  enemyInGame.transform.position.y+1);
		}

		for (int i = 0; i < matrix; i++) {
			for (int j = 0; j < matrix; j++) {
				if(boardMasVerification[i,j]==CONST_WATER || boardMasVerification[i,j]==CONST_COURSE){
					boardMas [i, j] = CONST_SAND;
					GameObject kletka = boardObject [i, j];
					kletka.SetActive(false);
				}
			}
		}
		updateProgresText ();
	}

	public void cellSelection(int x, int y){
		int type = boardMasVerification[x, y];
		if (type == CONST_WATER) {
			boardMasVerification [x, y] = CONST_WATER_V;
			if (x > 0 && x < matrix && y > 0 && y < matrix) {
				cellSelection (x + 1, y);
				cellSelection (x - 1, y);
				cellSelection (x, y + 1);
				cellSelection (x, y - 1);
			}
			
		}
	}

	private int [,] copyArray(int [,] arrayIn){
		int[,] arrayTo = new int [matrix, matrix];
		for (int i = 0; i < matrix; i++) {
			for(int j=0;j<matrix ;j++){
				arrayTo[i,j] = arrayIn[i,j];
			}
		}
		return arrayTo;
		}

	public void death(){
		setStartGame ();
		setLiveObject();
		deleteCourses ();
		timeGame = 60f;
		TimeText.text = "Time: " + timeGame;

		playerHP--;
		if (playerHP == 0) {
			gameOver ();
			return;
		}
		Invoke ("setLiveObject", 2f);
		Invoke ("setStartGame", 2f);

		updateHPText ();
	}

	private void setLiveObject(){
		playerController.setLive ();
		for (int i = 0; i < enemies.Count; i++) {
			enemies [i].setLive();
				
		}


	}

	private void deleteCourses(){
		for(int i=0;i<matrix;i++){
			for (int j = 0; j < matrix; j++) {
				if(boardMas[i,j]==CONST_COURSE){
					boardMas [i, j] = CONST_WATER;
				}
			}			
		}
	}

	public void gameOver(){
		game_Over = true;
		levelText.text = "Game Over!";
		controlUi ();
		RestartText.text = "Tab to Restart";
		GameObject boardGame = GameObject.Find ("Board");
		Destroy (boardGame.gameObject);
		enemies.Clear ();
		GameObject enemysDelete = GameObject.Find ("Enemys");
		Destroy (enemysDelete.gameObject);
		if (GameObject.Find("Courses")) {
			GameObject cours = GameObject.Find ("Courses");
			Destroy (cours.gameObject);
		}
		StopAllCoroutines();
	}


	private void controlUi(){
		ProgresText.text = "";
		HPText.text = "";
		TimeText.text = "";
		ButtonPause.SetActive (false);
		levelImage.SetActive(true);
	}


	public void updateHPText(){
		HPText.text = "Lives: " + playerHP;
	}

	IEnumerator updateTimeText(){
		yield return new WaitForSeconds (2f);
		bool waite = true;
		while (waite) {
			if (startGame) {
				timeGame--;
				if (timeGame < 0f) {
					gameOver ();
		
				}
				else TimeText.text = "Time: " + (int)timeGame;
			}
			yield return new WaitForSeconds (1f);
		}

	}


	public void updateProgresText(){
		int waterBlock=0;
		for (int i = 0; i < matrix; i++) {
			for (int j = 0; j < matrix; j++) {
				if (boardMas [i, j] == CONST_WATER || boardMas [i, j] == CONST_COURSE )
					waterBlock++;
			}
		}
		int procent = (int) (startWaterBlock * 0.95f);
		int raznica = startWaterBlock - procent;
		int itog = (int) ((waterBlock-raznica) * 100) / procent;
		ProgresText.text = "Progres: " + (100-itog);
		if (100-itog >= 70)
			nextLvl ();
	}

	public void setStartGame(){
		startGame=!startGame;
	}

	public void AddEnemyToList(Enemy script)
	{
		enemies.Add(script);
	}

	public void StartGame(){ // onClick startGame
		GameObject Menu = GameObject.Find ("Menu");
		Menu.SetActive (false);
		GameObject canvas = GameObject.Find ("Canvas");
		canvas.SetActive (true);
		InitGame ();
	}

	private void nextLvl(){
		setStartGame ();

		StopAllCoroutines();
		GameObject boardGame = GameObject.Find ("Board");
		Destroy (boardGame.gameObject);
		playerController.setLive();
		enemies.Clear ();
		GameObject enemysDelete = GameObject.Find ("Enemys");
		Destroy (enemysDelete.gameObject);
		InitGame ();

	}

	public void clickPause(){
		setStartGame ();
		playerController.pause ();
		for (int i = 0; i < enemies.Count; i++) {
			enemies [i].setLive ();
		}
	}

			
}
	


