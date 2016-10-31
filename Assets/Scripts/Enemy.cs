using UnityEngine;
using System.Collections;

public abstract class Enemy :MonoBehaviour {
	protected bool live;

	// Use this for initialization
	public virtual void go(){
		
	}
	public void setLive(){
		live = !live;
	}


}
