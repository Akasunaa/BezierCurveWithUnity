using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;

public class BezierCurve : MonoBehaviour
{

    private LineRenderer lineRenderer;
    [SerializeField] private LineRenderer tangenteLine;
    [SerializeField] ControlPoint[] controlPoints;
    [SerializeField] int numberOfPoints;

    int timeAnim = 10;
    float startTime;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        startTime = Time.time;
    }

    private void Update()
    {
        lineRenderer.positionCount = numberOfPoints;
        tangenteLine.positionCount = numberOfPoints;
        for (int i = 0; i < numberOfPoints; i++)
        {
            float t = i / (float)(numberOfPoints - 1);
            lineRenderer.SetPosition(i, DrawCurveWithWeight(t));

        }

        //animation tangente
        if(Time.time - startTime < timeAnim)
        {
            for (int i = 0; i < numberOfPoints; i++)
            {
                float t = i / (float)(numberOfPoints - 1);
                tangenteLine.SetPosition(i, DrawTangenteWithWeight(t, (Time.time - startTime) / timeAnim));

            }

        }
        else
        {
            startTime = Time.time;
            print("ouf");
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
        Vector3 sum = Vector3.zero;
        float sumDown = 0;
        for (int i = 0; i < n; i++)
        {
            sum += CoefBin(i, n - 1) * Mathf.Pow((1 - t), n - 1 - i) * Mathf.Pow(t, i) * (controlPoints[i + 1].position - controlPoints[i].position) * controlPoints[i].weight;
            sumDown += CoefBin(i, n) * Mathf.Pow((1 - t), n - i) * Mathf.Pow(t, i) * controlPoints[i].weight;
        }
        if(sumDown != 0)
        {
            return n * sum / sumDown;

        }
        return n * sum;
    }
    private Vector3 DrawTangente(float t, float a)
    {
        return DrawCurve(a) + Derivative(a) * (t - a);
    }

    private Vector3 DrawTangenteWithWeight(float t, float a)
    {
        return DrawCurveWithWeight(a) + DerivativeWithWeight(a) * (t - a);
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
