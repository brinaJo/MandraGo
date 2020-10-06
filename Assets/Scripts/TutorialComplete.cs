using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialComplete : MonoBehaviour
{
    public Tutorial tutorial_1;
    public Tutorial tutorial_2;
    

	void Update ()
    {
		if(tutorial_1.canTutorialComplete && tutorial_2.canTutorialComplete)
        {
            Application.LoadLevel("Title");
        }
	}
}
