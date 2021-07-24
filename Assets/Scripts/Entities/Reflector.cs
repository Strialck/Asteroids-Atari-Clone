using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflector : MonoBehaviour
{
    
    CameraStats cs;
    Vector3 buf;
    // Start is called before the first frame update
    void Start()
    {
        cs = CameraStats.getInstance();
        buf = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        buf = transform.position;

        if (buf.x > cs.MaxX)
            buf.x = cs.MinX + cs.Border;
        else
            if(buf.x < cs.MinX)
               buf.x = cs.MaxX - cs.Border;
        
        if (buf.y > cs.MaxY)
            buf.y = cs.MinY + cs.Border;
        else
            if(buf.y < cs.MinY)
               buf.y = cs.MaxY - cs.Border;

        transform.position = buf;

    }
}
