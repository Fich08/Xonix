using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;


public class BoardManager : MonoBehaviour {

	public GameObject water;
	public GameObject[] enemysWater;
	public GameObject[] enemysSand;
	public int offsetOfSand;
	public int zCor;
	private static int matrix = 30;

	public int[,] board = new int[30,30]; //1 sand,2 water;
	public GameObject [,] boardObj = new GameObject [30,30];

	const int CONST_SAND = 0;
	const int CONST_WATER = 1;
	const int CONST_COURSE = 2;
	const int CONST_ENEMY = 3;


	public void setupScene(int lvl){
		boardSetup ();
		enemySetup (lvl);
	}

	public void boardSetup(){
		
		GameObject boards = new GameObject("Board");
		GameObject waterParent = new GameObject("Water");



		for(int i=0; i<matrix;i++){
			for(int j=0;j<matrix;j++){
				board [i,j] = CONST_SAND; 
			
			}
		}


		for(int i=0; i<(matrix - (offsetOfSand*2));i++){
			for(int j=0;j<(matrix - (offsetOfSand*2));j++){
				board[j+offsetOfSand,i+offsetOfSand]=CONST_WATER;

				Vector3 spawnPosition = new Vector3(j+offsetOfSand,i+offsetOfSand,water.transform.position.z);

				Quaternion spawnRotation = Quaternion.identity;
				GameObject waterInst = Instantiate(water,spawnPosition,spawnRotation) as GameObject;
				boardObj [j+offsetOfSand, i+offsetOfSand] = waterInst;
				waterInst.transform.SetParent (waterParent.transform);
			}
		}
		waterParent.transform.SetParent (boards.transform);
	}

	private void enemySetup (int lvl){
		GameObject enemyList = new GameObject ("Enemys");
		int enemyWater = (int)Mathf.Log (lvl,2f);
		enemyWater++;
		int enemySand = 1;
		enemySand += (int) Mathf.Log(lvl,3f);

		for (int i = 0; i < enemyWater; i++) {
			GameObject enemy = enemysWater [Random.Range (0, enemysWater.Length)];
			Vector3 spawnPostion = randomPositionWaterEnemy ();
			Quaternion spawnRotation = Quaternion.identity;
			GameObject enemyInGame = Instantiate (enemy,spawnPostion,spawnRotation) as GameObject;
			enemyInGame.transform.SetParent (enemyList.transform);

		}

		for (int i = 0; i < enemySand; i++) {
			GameObject enemy = enemysSand [Random.Range (0, enemysSand.Length)];
			Vector3 spawnPostion = new Vector3(enemy.transform.position.x+i,enemy.transform.position.y,enemy.transform.position.z);
			Quaternion spawnRotation = Quaternion.identity;
			GameObject enemyInGame = Instantiate (enemy,spawnPostion,spawnRotation) as GameObject;
			enemyInGame.transform.SetParent (enemyList.transform);

		}


	}

	private Vector3 randomPositionWaterEnemy(){
		
		bool isTrue = true;
		while (isTrue) {
			int xCor = Random.Range (0, matrix-1);
			int yCor = Random.Range (0, matrix-1);
			if (board [xCor, yCor] == CONST_WATER) {
				return new Vector3 (xCor, yCor, zCor);
			}
		}
		return new Vector3(0f,0f,0f);
	}

}
