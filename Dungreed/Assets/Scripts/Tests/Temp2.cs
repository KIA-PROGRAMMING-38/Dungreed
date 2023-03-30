using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp2 : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 OriginPos;
    public Transform dest;
    Vector2 DestPos;
    public float time;
    bool isPlaying = false;
    void Start()
    {
        OriginPos = transform.position;
        DestPos = dest.position;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space
            ))
        {
            if(isPlaying == false)
                StartCoroutine(Cor());
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            transform.position = OriginPos;
        }
    }

    IEnumerator Cor()
    {
        isPlaying = true;
        float t = 0f;
        transform.position = OriginPos;
        while (t-0.1f < time) 
        { 
            t += Time.deltaTime;
            Vector2 v;
            //v = Utils.Math.Utility2D.Spring(OriginPos, DestPos, t / time);
            v.x = Utils.Math.Utility2D.EaseInOutQuart(OriginPos.x, DestPos.x, t/time);
            v.y = Utils.Math.Utility2D.Spring(transform.position.y, DestPos.y, t/time);
            transform.position = v;
            yield return null;
        }
        transform.position = DestPos;
        isPlaying = false;
    }
}
