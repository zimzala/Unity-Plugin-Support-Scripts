(C) 2013 Zimzala Studios.
//A free script - you may not sell! See the README
//For use with Prime31's Game Centre Plug-in

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Prime31;
 
public class GCManager : MonoBehaviour {
     
    #if UNITY_IPHONE
    public static string playerName;
    public static long playerHighScoreGC;
    public long _score;
    public static bool idFound;
    public static List<GameCenterAchievement> _achievements;
     
//  Initialise Game Centre
    void Awake () 
    {
        GameCenterBinding.isGameCenterAvailable();  
    }
     
    void Start()
    {
        if (GameCenterBinding.isGameCenterAvailable() )
        {           
            GameCenterBinding.authenticateLocalPlayer(); //Must Have
            GameCenterBinding.showCompletionBannerForAchievements(); //Display banners on Achievement Unlocks...
            GameCenterBinding.loadReceivedChallenges();
        }   
         
        //Listener
        GameCenterManager.achievementsLoaded += achievementStatus =>
        {
            _achievements = achievementStatus;
        };  
    }
     
     
//  Update the GC Leaderboard. The leaderboard will determine if the score is higher than what's already been previously posted and will only post a new high score automatically, thereby stopping the achievement banner from reappearing if subsequent repeats of achievement parameters are fulfilled.
     
    public static void UpdateLeaderWithScore (long myScore)
    {
        GameCenterBinding.reportScore(myScore, "YOURLEADERBOARDID");    
        Debug.Log ("New Score reported to GC");
    }
     
     
//  Update the GameCentre with a single parameter new achievement
    public static void ReportAchievement(string id)
        {       
        if (_achievements !=null)
        {
            foreach (GameCenterAchievement i in _achievements)
            {       
                idFound = false; 
                 
                //search for achievement id in the list
                if (!idFound) 
                {
                 
                         if ( i.identifier == id)
                         {
                    Debug.Log ("Achievement already unlocked "+i.identifier);
                    idFound = true;
                    break;
                     }
                }
            }
        }
         
        //First achievemeent ever to be unlocked or achievement id not in the list of unlocked achievements 
        if (_achievements == null || !idFound) 
        {
             Debug.Log("Unlocked Achievement: "+id);
                 GameCenterBinding.reportAchievement( id, 100.0f );
        }
    }   
     
     
    // Called for achievements where progress to completion occurs over time (e.g. collection of x items)
    public static void ReportAchievementProgressive(string id, float progress)
        {       
        if (_achievements !=null)
        {
                 foreach (GameCenterAchievement i in _achievements)
                 {      
                idFound = false; 
                //search for achievement id in the list
                if (!idFound) 
                {
                 
                         if ( i.identifier == id && i.percentComplete == 100.0f)
                         {
                        Debug.Log ("Achievement already unlocked "+i.identifier);
                        idFound = true;
                        break;
                     }
                     
                     else if (i.identifier == id && i.percentComplete < 100.0f)
                     {
                        float idProgress = i.percentComplete;
                        float updatedProgress = 0.0f;
                        idFound = true;
                         
                        if (idProgress+progress > 100)
                        {
                            updatedProgress = 100.0f;
                            GameCenterBinding.reportAchievement(id, updatedProgress);
                            Debug.Log ("Progressive Achievement is now Complete! Check: "+(updatedProgress)+" percent");
                            break;
                        }
                        else
                        {
                            updatedProgress = idProgress + progress;
                            GameCenterBinding.reportAchievement(id, updatedProgress);
                            Debug.Log ("Progressive Achievement is now: "+(updatedProgress)+" percent complete!");
                            break;
                        }                   
                    }
                }
            }
        }
             
        if (_achievements == null || !idFound) //First achievemeent ever to be unlocked or achievement id not in the list of unlocked achievements
        {
             Debug.Log("Unlocked Achievement: "+id);
                 GameCenterBinding.reportAchievement( id, progress );
        }
    }
     
    //Reset this profile's Achievements. Throw a button in your scene if you want your player's to have the ability to reset all of their Achievements.
        //Also handy to have for testing purposes.
 
    public static void ResetAchievements()
    {       
        GameCenterBinding.resetAchievements();
    }
    #endif
}
