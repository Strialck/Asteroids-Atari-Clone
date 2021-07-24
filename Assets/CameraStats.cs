using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStats
{
    Camera Camera;
    public float CamWidth { get; private set; }
    public float CamHeght { get; private set; }
    public float MaxX { get; private set; }
    public float MinX { get; private set; }
    public float MaxY { get; private set; }
    public float MinY { get; private set; }
    public float Border { get; private set; }

    private static CameraStats instance;
    public Vector3 _camPosition;

    private CameraStats()
    {
        Border = 0.1f;
        this.Camera = Camera.main;
        _camPosition = Camera.transform.position;
        CamHeght = Camera.orthographicSize;
        CamWidth = CamHeght * Camera.aspect;
        MaxX = _camPosition.x + CamWidth;
        MinX = _camPosition.x - CamWidth;
        MaxY = _camPosition.y + CamHeght;
        MinY = _camPosition.y - CamHeght;
        CamHeght *= 2;
        CamWidth *= 2;
    }
    public static Vector3 GetRandomPosOnCamBorder()
    {
        Vector3 position = new Vector3();
        if(Random.Range(0f,1f) > 0.5f) // левая или правая грань
        {
            position.x = Random.Range(0f, 1f) > 0.5f ? getInstance().MaxX - getInstance().Border: getInstance().MinX + getInstance().Border;
            position.y = Random.Range(getInstance().MinY + getInstance().Border, getInstance().MaxY - getInstance().Border);
        }
        else
        {
            position.y = Random.Range(0f, 1f) > 0.5f ? getInstance().MaxY - getInstance().Border : getInstance().MinY + getInstance().Border;
            position.x = Random.Range(getInstance().MinX + getInstance().Border, getInstance().MaxX - getInstance().Border);
        }
        return position;
    }
    public static CameraStats getInstance()
    {
        if (instance == null)
            instance = new CameraStats();
        return instance;
    }
    
}
