using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaskColl : MonoBehaviour
{
    public bool isFlaskBroken;

    public GameObject FlaskBroken;
    public GameObject FlaskWater;
    public void FlaskBrokenInit(Transform pos)
    {
        FlaskBroken = (GameObject)Instantiate(Resources.Load("Prefab/MagicObj/FlaskBroken"));
        FlaskBroken.transform.parent = this.transform.parent;
        FlaskBroken.transform.position = pos.transform.position;
        FlaskWater = (GameObject)Instantiate(Resources.Load("Prefab/MagicObj/MagicWater"));
        FlaskWater.transform.parent = this.transform.parent;
        //Vector3 pos2 = new Vector3(pos.transform.position.x, pos.transform.position.y - 0.277f, pos.transform.position.z);
        FlaskWater.transform.position = pos.transform.position;
    }


}
