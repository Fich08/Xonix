using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public Vector2 x;
	public Vector2 y;
	public float waiteAfter;
	public GameObject course;
	public float curentSpead;

	private int cord; // регистр пути, 1- право, 2 лево, 3 верх, 4 низ
	private GameObject[] waters;
	private GameObject Courses;
	private bool isWater;

	private GameController gameControll;
	private Vector3 startPosition;
	private bool live;
	private Vector2 touchOrigin = -Vector2.one;


	const int CONST_SAND = 0;
	const int CONST_WATER = 1;
	const int CONST_COURSE = 2;
	const int CONST_ENEMY = 3;



	// Use this for initialization
	void Start () {

		live = false;

		startPosition = transform.position;
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController2");
		if (gameControllerObject != null)
			gameControll = gameControllerObject.GetComponent<GameController> ();
//		hod = new List<Vector2>();
		StartCoroutine (choiceWalk());
	}
	
	// Update is called once per frame
	void Update () {
		 
		int horizontal = 0;
		int vertical = 0;

			


			#if UNITY_STANDALONE || UNITY_WEBPLAYER
		if(live){
			horizontal = (int)Input.GetAxisRaw ("Horizontal");
			vertical = (int)Input.GetAxisRaw ("Vertical");
			if (horizontal != 0) vertical = 0;
		}
			#else
		if(live){	

			if ( Input.touchCount>0){
				Touch myTouch = Input.touches[0];
			if(myTouch.phase == TouchPhase.Began){
					touchOrigin = myTouch.position;
				}
				else if(myTouch.phase == TouchPhase.Ended ){
					Vector2 touchEnd = myTouch.position;
					float x = touchEnd.x - touchOrigin.x;
					float y = touchEnd.y - touchOrigin.y;
					//touchOrigin.x= -1;
					if(Mathf.Abs(x)> Mathf.Abs(y))
						horizontal = x>0? 1:-1;
					else
						vertical = y>0 ?1:-1;
				} else {
					
				}
			
			} 
		}

			#endif

				if (horizontal != 0 ) {
					vertical = 0;

					cord = horizontal > 0 ? 1 : 2;				
				} else {
					if (vertical != 0)
						cord = vertical > 0 ? 3 : 4;
				}


	
	}


	void goTo(){	
		Quaternion spawnRotation = Quaternion.identity;
		GameObject courseNow = null;
		switch (cord) {

		case 1:
			if (transform.position.x +1 <= x.y) {
				if (isWater) {
					gameControll.setTypeCordinate ((int)transform.position.x, (int)transform.position.y, CONST_COURSE);
					spawnRotation.eulerAngles = new Vector3 (0f, 0f, 90f);
					courseNow = Instantiate (course, transform.position, spawnRotation) as GameObject;
					courseNow.transform.SetParent (Courses.transform);
				}
				transform.position += Vector3.right;
				}
				break;
		case 2:
			if (transform.position.x -1 >= x.x) {
				if (isWater) {
					gameControll.setTypeCordinate ((int)transform.position.x, (int)transform.position.y, CONST_COURSE);
					spawnRotation.eulerAngles = new Vector3 (0f, 0f, 90f);
					courseNow = Instantiate (course, transform.position, spawnRotation) as GameObject;
					courseNow.transform.SetParent (Courses.transform);
				}

				transform.position += -Vector3.right;

			}
			break;
		case 3:
			if (transform.position.y + 1 <= y.y) {
				if (isWater) {
					gameControll.setTypeCordinate ((int)transform.position.x,(int) transform.position.y, CONST_COURSE);
					spawnRotation = Quaternion.identity;
					courseNow = Instantiate (course, transform.position, spawnRotation) as GameObject;
					courseNow.transform.SetParent (Courses.transform);
				}		
			transform.position += Vector3.up;
			}
			break;
		case 4:
			if (transform.position.y -1 >= y.x) {
				if (isWater) {
					gameControll.setTypeCordinate ((int)transform.position.x, (int)transform.position.y, CONST_COURSE);
					spawnRotation = Quaternion.identity;
					courseNow = Instantiate (course, transform.position, spawnRotation) as GameObject;
					courseNow.transform.SetParent (Courses.transform);
				}
				transform.position += -Vector3.up;
			}		
			break;
		}
	}

	public void toStartPos(){
		transform.position = startPosition;
	}

	public void setLive(){
		if (!live) {
			if(Courses!=null)
				Destroy (Courses.gameObject);			
			toStartPos ();
		}
		live = !live;

	}

	public void pause(){		
		live = !live;
		enabled =!enabled;
	}


	IEnumerator choiceWalk(){
		while (true) {
			if (live) {
				int positionType = gameControll.getTypeCordinate ((int)transform.position.x, (int)transform.position.y);
				if (positionType == CONST_WATER) {
					isWater = true;
					if (Courses == null) {
						Courses = new GameObject ("Courses");
					}
				} else if (positionType == CONST_SAND) {
					if (isWater) {
						Destroy (Courses.gameObject);
						cord = 0;
						isWater = false;
						gameControll.verificatio ();

					}
				} else if (positionType == CONST_COURSE) {
					
					isWater = false;
					gameControll.death ();
					cord = 0;

				}
				goTo ();
			} 				
			yield return new WaitForSeconds (waiteAfter);

		}
	}


}
