using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;


public class Enemy1 : Enemy { 

	public float waiteAfter;
 
	private int courseToGo;// 1 в лев.верх , 2 прав.вер, 3 лев.низ, 4 прав.низ
	private GameController gameControll; 
	private GameObject player;
	private int regGo;
	private GameObject enemys;


	const int CONST_SAND = 0;
	const int CONST_WATER = 1;
	const int CONST_COURSE = 2;
	const int CONST_ENEMY = 3;

	// Use this for initialization
	public void Start () {
		
		live = false;
		courseToGo = (int) Random.Range(1,4);
		player = GameObject.FindGameObjectWithTag ("Player");
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController2");
		if (gameControllerObject != null) {
			gameControll = gameControllerObject.GetComponent<GameController> ();
			gameControll.AddEnemyToList (this);
		}
	
		StartCoroutine (choiceWalk());
	}
	 
	public void Update(){


	}


	public override void go(){
		Vector3 position = new Vector3 (transform.position.x, transform.position.y,transform.position.z);
		int a;
		int b;
		int c;
		switch(courseToGo){

		case 1:


			a = gameControll.getTypeCordinate ((int)position.x - 1,(int)position.y);
			b = gameControll.getTypeCordinate ((int)position.x - 1,(int)position.y + 1);
			c = gameControll.getTypeCordinate ((int)position.x,(int)position.y + 1);

			if (a == CONST_SAND && b == CONST_SAND && c == CONST_SAND || a == CONST_SAND && c == CONST_SAND) {
				courseToGo = 4;

			} else if (a == CONST_SAND) {
				courseToGo = 2;

			} else if (c == CONST_SAND) {
				courseToGo = 3;
			} else if (b == CONST_SAND)
				courseToGo = 4;
			else {
				position.x -= 1f;
				position.y += 1f;
				transform.position = position;
			
			}
			break;
		case 2: 

			a = gameControll.getTypeCordinate ((int)position.x + 1,(int)position.y);
			b = gameControll.getTypeCordinate ((int)position.x + 1, (int)position.y + 1);
			c = gameControll.getTypeCordinate ((int)position.x,(int)position.y + 1);

			if (a == CONST_SAND && b == CONST_SAND && c == CONST_SAND || a == CONST_SAND && c == CONST_SAND ) {
				courseToGo = 3;

			} else if (a == CONST_SAND) {
				courseToGo = 1;

			} else if (c == CONST_SAND) {
				courseToGo = 4;
			} else if (b == CONST_SAND)
				courseToGo = 3;
			else {
				position.x += 1f;
				position.y += 1f;
				transform.position = position;

			}
			break;
		case 3:
			a = gameControll.getTypeCordinate ( (int)position.x -1,(int)position.y );
			b = gameControll.getTypeCordinate ( (int)position.x - 1, (int)position.y -1);
			c = gameControll.getTypeCordinate ( (int)position.x ,(int)position.y - 1);
			if (a == CONST_SAND && b == CONST_SAND && c == CONST_SAND || a == CONST_SAND && c == CONST_SAND ) {
				courseToGo = 2;

			} else if (a == CONST_SAND) {
				courseToGo = 4;

			} else if (c == CONST_SAND) {
				courseToGo = 1;
			} else if (b == CONST_SAND)
				courseToGo = 2;
			else {
				position.x -= 1f;
				position.y -= 1f;
				transform.position = position;

			}
			break;
		case 4:
			a = gameControll.getTypeCordinate ((int)position.x + 1, (int)position.y );
			b = gameControll.getTypeCordinate ((int)position.x + 1,(int)position.y -1);
			c = gameControll.getTypeCordinate ((int)position.x, (int)position.y - 1 );
			if (a == CONST_SAND && b == CONST_SAND && c == CONST_SAND || a == CONST_SAND && c == CONST_SAND) {
				courseToGo = 1;

			} else if (a == CONST_SAND) {
				courseToGo = 3;

			} else if (c == CONST_SAND) {
				courseToGo = 2;
			} else if (b == CONST_SAND)
				courseToGo = 1;
			else {
				position.x += 1f;
				position.y -= 1f;
				transform.position = position;

			}
			break;
		
		}

}


	IEnumerator choiceWalk(){
		while (true) {
			if (live) {
				int typePos = gameControll.getTypeCordinate ((int)transform.position.x, (int)transform.position.y);
				if (typePos == CONST_COURSE) {
					gameControll.death ();
				} else if (transform.position == player.transform.position) {
					Debug.Log ("Встретил игрока");
					gameControll.death ();
				} else
					go ();
			}
			yield return new WaitForSeconds (waiteAfter);

		}
	}
}
