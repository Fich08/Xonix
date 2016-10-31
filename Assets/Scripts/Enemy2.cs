using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;


public class Enemy2 : Enemy {

	public Vector2 x;
	public Vector2 y;
  
	public float waiteAfter;
	private int courseToGo;// 1 в лев.верх , 2 прав.вер, 3 лев.низ, 4 прав.низ
	private GameController gameControll; 

	private GameObject player;

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
		int a = 0;
		int b = 0;
		int c = 0;

		bool not = false;
		switch(courseToGo){
		case 1:
			

			if ( position.x - 1 >= x.x && position.x - 1 <= x.y)//1 41
				a = gameControll.getTypeCordinate ((int)position.x - 1, (int)position.y);
			else {
				a = CONST_WATER;
					

				}
			if (position.x - 1 >= x.x && position.y + 1 <= y.y)
				b = gameControll.getTypeCordinate ((int)position.x - 1, (int)position.y + 1);
			else
				b = CONST_WATER;

			if (position.x >= x.x && position.x <= x.y && position.y + 1 <= y.y)
				c = gameControll.getTypeCordinate ((int)position.x, (int)position.y + 1);
			else
				c = CONST_WATER;
			

			if (a == CONST_WATER && b == CONST_WATER && c == CONST_WATER || a == CONST_WATER && c == CONST_WATER) {
				courseToGo = 4;

			} else if (a == CONST_WATER) {
				courseToGo = 2;

			} else if (c == CONST_WATER) {
				courseToGo = 3;
			} else if (b == CONST_WATER)
				courseToGo = 3;
			else if(not) courseToGo = 3;
			else {
				position.x -= 1f;
				position.y += 1f;
				transform.position = position;

			}
			break;
		case 2: 
			
			if (position.x + 1 <= x.y)
				a = gameControll.getTypeCordinate ((int)position.x + 1, (int)position.y);
			else 
				a = CONST_WATER;			
			if (position.x + 1 <= x.y && position.y + 1 <= y.y)
				b = gameControll.getTypeCordinate ((int)position.x + 1, (int)position.y + 1);
			else
				b = CONST_WATER;

			if (position.x <= x.y && position.x >= x.x && position.y + 1 <= y.y)
				c = gameControll.getTypeCordinate ((int)position.x, (int)position.y + 1);
			else
				c = CONST_WATER;
			


			if (a == CONST_WATER && b == CONST_WATER && c == CONST_WATER || a == CONST_WATER && c == CONST_WATER) {
				courseToGo = 3;

			} else if (a == CONST_WATER) {
				courseToGo = 1;

			} else if (c == CONST_WATER) {
				courseToGo = 4;
			} else if (b == CONST_WATER)
				courseToGo = 1;
			else if (not)
				courseToGo = 1;
			else {
				position.x += 1f;
				position.y += 1f;
				transform.position = position;


			}
			break;
		case 3:
			
			if (position.x - 1 >= x.x)
				a = gameControll.getTypeCordinate ((int)position.x - 1, (int)position.y);
			else
				a = CONST_WATER;

			if (position.x - 1 >= x.x && position.y - 1 >= y.x)
				b = gameControll.getTypeCordinate ((int)position.x - 1, (int)position.y - 1);
			else
				b = CONST_WATER;

			if (position.x >= x.x && position.x <= x.y && position.y - 1 >= y.x)
				c = gameControll.getTypeCordinate ((int)position.x, (int)position.y - 1);
			else
				c = CONST_WATER;
			


			if (a == CONST_WATER && b == CONST_WATER && c == CONST_WATER || a == CONST_WATER && c == CONST_WATER) {
				courseToGo = 2;

			} else if (a == CONST_WATER) {
				courseToGo = 4;

			} else if (c == CONST_WATER) {
				courseToGo = 1;
			} else if (b == CONST_WATER)
				courseToGo = 4;
			else if (not)
				courseToGo = 4;
			else {
				position.x -= 1f;
				position.y -= 1f;
				transform.position = position;

			}
			break;
		case 4:
			

			if (position.x + 1 <= x.y)
				a = gameControll.getTypeCordinate ((int)position.x + 1, (int)position.y);
			else
				a  = CONST_WATER;

			if (position.x + 1 <= x.y && position.y - 1 >= y.x)
				b = gameControll.getTypeCordinate ((int)position.x + 1, (int)position.y - 1);
			else
				b = CONST_WATER;

			if (position.x >= x.x && position.x <= x.y && position.y - 1 >= y.x)
				c = gameControll.getTypeCordinate ((int)position.x, (int)position.y - 1);
			else				
				c = CONST_WATER;

			if (a == CONST_WATER && b == CONST_WATER && c == CONST_WATER || a == CONST_WATER && c == CONST_WATER) {
				courseToGo = 1;

			} else if (a == CONST_WATER) {
				courseToGo = 3;

			} else if (c == CONST_WATER) {
				courseToGo = 2;
			} else if (b == CONST_WATER)
				courseToGo = 2;
			else if (not)
				courseToGo = 2;
			else {
				position.x += 1f;
				position.y -= 1f;
				transform.position = position;

			}
			break;
		}
	}

	private int randFaz(int x,int y){
		int[] faz = new int[]{x,y,x,y,x,y};
		int faza = faz [ Random.Range (0, faz.Length)];
		return faza;
	}


	IEnumerator choiceWalk(){
		while (true) {
			if (live) {
				if (transform.position == player.transform.position) {
					Debug.Log ("Встретил игрока");
					gameControll.death ();
				}
				else 
					go ();
			}
			yield return new WaitForSeconds (waiteAfter);

		}
	}
}
