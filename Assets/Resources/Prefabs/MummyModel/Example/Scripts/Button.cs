using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	    private Animator anim;
	
	
	    void Start () {
		anim=GetComponent<Animator>();
		
	    }
        void OnGUI() {
       
        if (GUI.Button(new Rect(65, 30, 100, 50), "Idle")){
			anim.SetBool ("IsJumping",false);
			anim.SetBool ("IsRunning",false);
			anim.SetBool ("IsDeath",false);
			anim.SetBool ("IsBackDamage",false);
			anim.SetBool ("IsLeftDamage",false);
			anim.SetBool ("IsRightDamage",false);
			anim.SetBool ("IsHitting",false);
			anim.SetBool ("IsDamage",false);
		}
           
        
        if (GUI.Button(new Rect(65, 90, 100, 50), "Jump")){
		    anim.SetBool ("IsJumping",true);
			anim.SetBool ("IsRunning",false);
			anim.SetBool ("IsDeath",false);
			anim.SetBool ("IsBackDamage",false);
			anim.SetBool ("IsLeftDamage",false);
			anim.SetBool ("IsRightDamage",false);
			anim.SetBool ("IsHitting",false);
			anim.SetBool ("IsDamage",false);
		}
				
	    if (GUI.Button(new Rect(65, 150, 100, 50), "Run")){
		    anim.SetBool ("IsJumping",false);
			anim.SetBool ("IsRunning",true);
			anim.SetBool ("IsDeath",false);
			anim.SetBool ("IsBackDamage",false);
			anim.SetBool ("IsLeftDamage",false);
			anim.SetBool ("IsRightDamage",false);
			anim.SetBool ("IsHitting",false);
			anim.SetBool ("IsDamage",false);
		}
		
		if (GUI.Button(new Rect(65, 210, 100, 50), "Death")){
		    anim.SetBool ("IsJumping",false);
			anim.SetBool ("IsRunning",false);
			anim.SetBool ("IsDeath",true);
			anim.SetBool ("IsBackDamage",false);
			anim.SetBool ("IsLeftDamage",false);
			anim.SetBool ("IsRightDamage",false);
			anim.SetBool ("IsHitting",false);
			anim.SetBool ("IsDamage",false);
		}
		
		if (GUI.Button(new Rect(550, 30, 100, 50), "Back Damage")){
		    anim.SetBool ("IsJumping",false);
			anim.SetBool ("IsRunning",false);
			anim.SetBool ("IsDeath",false);
			anim.SetBool ("IsBackDamage",true );
			anim.SetBool ("IsLeftDamage",false);
			anim.SetBool ("IsRightDamage",false);
			anim.SetBool ("IsHitting",false);
			anim.SetBool ("IsDamage",false);
		}
		
		if (GUI.Button(new Rect(550, 90, 100, 50), "Left Damage")){
		    anim.SetBool ("IsJumping",false);
			anim.SetBool ("IsRunning",false);
			anim.SetBool ("IsDeath",false);
			anim.SetBool ("IsBackDamage",false);
			anim.SetBool ("IsLeftDamage",true);
			anim.SetBool ("IsRightDamage",false);
			anim.SetBool ("IsHitting",false);
			anim.SetBool ("IsDamage",false);
		}
		
		if (GUI.Button(new Rect(550, 150, 100, 50), "Right Damage")){
			anim.SetBool ("IsJumping",false);
			anim.SetBool ("IsRunning",false);
			anim.SetBool ("IsDeath",false);
			anim.SetBool ("IsBackDamage",false);
			anim.SetBool ("IsLeftDamage",false);
			anim.SetBool ("IsRightDamage",true );
			anim.SetBool ("IsHitting",false);
			anim.SetBool ("IsDamage",false);
		}
		
		if (GUI.Button(new Rect(550, 210, 100, 50), "Hit/Attack")){
		    anim.SetBool ("IsJumping",false);
			anim.SetBool ("IsRunning",false);
			anim.SetBool ("IsDeath",false);
			anim.SetBool ("IsBackDamage",false);
			anim.SetBool ("IsLeftDamage",false);
			anim.SetBool ("IsRightDamage",false);
			anim.SetBool ("IsHitting",true);
			anim.SetBool ("IsDamage",false);
		}
		
		if (GUI.Button(new Rect(320, 30, 100, 50), "Damage")){
		    anim.SetBool ("IsJumping",false);
			anim.SetBool ("IsRunning",false);
			anim.SetBool ("IsDeath",false);
			anim.SetBool ("IsBackDamage",false);
			anim.SetBool ("IsLeftDamage",false);
			anim.SetBool ("IsRightDamage",false);
			anim.SetBool ("IsHitting",false);
			anim.SetBool ("IsDamage",true);
		}
            
        
    }
}