using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public AudioSource Card;
    public AudioSource Teleport;
    public AudioSource Win;
    public AudioSource BackGround;

    public void PlayCard()
    {
        Card.Play();
    }
    
    public void PlayTeleport()
    {
        Teleport.Play();
    }
    
    public void PlayWin(){
        Win.Play();
    }
    
    public void PlayBackGround()
    {
        BackGround.Play();
    }
    
    public void StopBackGround()
    {
        BackGround.Stop();
    }
    
        
    public void PlayGoal()
    {
        BackGround.Play();
    }

}
