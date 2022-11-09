using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BezierCurve : MonoBehaviour
{

    private LineRenderer lineRenderer;
    [SerializeField] private LineRenderer tangenteLine;
    [SerializeField] private LineRenderer pointLine;
    [SerializeField] ControlPoint[] controlPoints;
    [SerializeField] int numberOfPoints;

    int timeAnim = 10;
    float startTime;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        startTime = Time.time;
        tangenteLine.positionCount = 3;
    }

    private void Update()
    {
        lineRenderer.positionCount = numberOfPoints;
        pointLine.positionCount = controlPoints.Length;
        for (int i = 0; i < numberOfPoints; i++)
        {
            float t = i / (float)(numberOfPoints - 1);
            lineRenderer.SetPosition(i, DrawCurveWithWeight(t));

        }

        for (int i =0; i < controlPoints.Length; i++)
        {
            pointLine.SetPosition(i, controlPoints[i].transform.position);
        }


        //animation tangente
        if(Time.time - startTime < timeAnim)
        {
            float point = (Time.time - startTime) / timeAnim;
            tangenteLine.SetPosition(0,DrawTangenteWithWeight(point - 0.5f, point));
            tangenteLine.SetPosition(1, DrawTangenteWithWeight(point, point));
            tangenteLine.SetPosition(2, DrawTangenteWithWeight(point + 0.5f, point));
        }
        else
        {
            startTime = Time.time;
        }
    }


    private Vector2 DrawThree(float t)
    {
        return (1 - t) * ((1 - t) * controlPoints[0].position + t * controlPoints[1].position) +
            t * ((1 - t) * controlPoints[1].position + t * controlPoints[2].position);
    }

     private Vector3 DrawCurve(float t)
    {
        int n = controlPoints.Length - 1;
        Vector3 sum = Vector3.zero;
        for (int i = 0; i <= n; i++)
        {
            sum += CoefBin(i, n) * Mathf.Pow((1 - t), n - i) * Mathf.Pow(t, i) * controlPoints[i].position;
        }
        return sum;
    }

    private Vector3 DrawCurveWithWeight(float t)
    {
        int n = controlPoints.Length - 1;
        Vector3 sumUp = Vector3.zero;
        float sumDown = 0;
        for (int i = 0; i <= n; i++)
        {
            sumUp += CoefBin(i, n) * Mathf.Pow((1 - t), n - i) * Mathf.Pow(t, i) * controlPoints[i].weight * controlPoints[i].position;
            sumDown += CoefBin(i, n) * Mathf.Pow((1 - t), n - i) * Mathf.Pow(t, i) * controlPoints[i].weight;
        }
        if (sumDown==0)
        {
            return sumUp;

        }
        return sumUp/sumDown;
    }

    private Vector3 Derivative(float t)
    {
        int n = controlPoints.Length - 1;
        Vector3 sum = Vector3.zero;
        for (int i = 0; i < n; i++)
        {
            sum += CoefBin(i, n-1) * Mathf.Pow((1 - t), n - 1 - i) * Mathf.Pow(t, i) * (controlPoints[i+1].position- controlPoints[i].position);
        }
        return n*sum;
    }

    private Vector3 DerivativeWithWeight(float t)
    {
        int n = controlPoints.Length - 1;
        Vector3 u = Vector3.zero;
        Vector3 uprime = Vector3.zero;
        float v = 0;
        float vprime = 0;
        for (int i = 0; i < n; i++)
        {
            u += CoefBin(i, n) * Mathf.Pow((1 - t), n - i) * Mathf.Pow(t, i) * controlPoints[i].weight * controlPoints[i].position;
            v += CoefBin(i, n) * Mathf.Pow((1 - t), n - i) * Mathf.Pow(t, i) * controlPoints[i].weight;
            uprime += CoefBin(i, n - 1) * Mathf.Pow((1 - t), n - 1 - i) * Mathf.Pow(t, i) * (controlPoints[i + 1].position * controlPoints[i + 1].weight - controlPoints[i].position * controlPoints[i].weight);
            vprime += CoefBin(i, n - 1) * Mathf.Pow((1 - t), n - 1 - i) * Mathf.Pow(t, i) * ( controlPoints[i + 1].weight -  controlPoints[i].weight);

        }
        return (uprime * v - u * vprime) / (v * v);
    }
    private float DrawTangente(float t, float a)
    {
        return (DrawCurve(a) + Derivative(a) * (t - a)).y;
    }

    private Vector3 DrawTangenteWithWeight(float t, float a)
    {
        Vector3 tan = Vector3.zero;
        tan.x = t;
        return ((DrawCurveWithWeight(a) + DerivativeWithWeight(a) * (t - a)));
    }
    private int CoefBin(int i, int n)
    {
        return Factorial(n) / (Factorial(n - i) * Factorial(i));
    }

    int Factorial(int i)
    {
        if (i <= 1)
            return 1;
        return i * Factorial(i - 1);
    }

}
