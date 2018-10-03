using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teststuff : MonoBehaviour {


	public bool killedten = false;
	public bool filled10000 = false;
	public bool lv1threestars = false;
	public bool lvl2twostars = false;


	public int finishedlevel;
	public int kills;
	public int threestars;
bool reset = true;
	// Use this for initialization
	void Start () {
		if(reset){
 PlayerPrefs.SetInt("AreaFilled", 0);
            PlayerPrefs.SetInt("FilledAreaAch", 0);
            PlayerPrefs.SetInt("EnemiesKilled", 0);
            PlayerPrefs.SetInt("EnemiesKilledAch", 0);
            PlayerPrefs.SetInt("FullStar", 0);
            PlayerPrefs.SetInt("FullStarAch", 0);
            PlayerPrefs.SetInt("FinishedLevelAch", 0);
           reset = false;
           PlayerPrefs.SetInt("FinishedLevel",0);
		   PlayerPrefs.SetInt("1",0);
		   PlayerPrefs.SetInt("2",0);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		PlayerPrefs.SetInt("FinishedLevel",finishedlevel);
		PlayerPrefs.SetInt("EnemiesKilled", kills);
		PlayerPrefs.SetInt("FullStar", threestars);
		if(lvl2twostars){
			PlayerPrefs.SetInt("2",2);

		}
		if(lv1threestars){
			PlayerPrefs.SetInt("1",3);
		}
	}
}
