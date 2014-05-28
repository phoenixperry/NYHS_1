using UnityEngine;
using System.Collections;

public interface NodeInterface {
	//keep a queue of all hero elements outside the node. 
	//kick a timer off
	//as the timer hits, pull a hero element. when we are done with it, pop it off the queue and fade it out 

	//have the non-hero elements in an array list all doing their own thing with out any bother to anyone. 

	void PositionNode(); 
	//load image to sprite 
	//positon sprite
	//fade up alpha of node 
	//start timer 
	//if hero add to fade in queue 

	//if not hero just fade in and out 

	void FadeNodeIn();
	//run the lerp on the the postion 
	//run the lerp on the color 

	void FadeNodeOut(); 
	//run lerp on position 
	//run lerp on color 

	void AnimateNodeIn(); 
	void OpenNode(); 

	void GetData(); 
	//load from xml 
	//apply image to Sprite  
	//read text into GUI.text 
	//fade up Sprite 
	//fade up GUI.text 
	//kick timer 
	//fade out 
	//call close 

	void CloseNode(); 
	//

	void AnimateNodeOut(); 


}
