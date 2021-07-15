using EvolutionaryPerceptron;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankControll))]
public class NeuralTank : BotHandler //OYM:̹�˵�������
{
    private TankControll tank;
    public Boob[] boobList;
    private Boob nearlestBoob;
    private int inputSize;
    private double[,] lastInputs;
    private double[,] inputs;
    private double[,] output;

    protected override void Start()
    {
        base.Start();
        tank = GetComponent<TankControll>();
        inputSize = 12;
        lastInputs = new double[1, inputSize];
    }

    private void Update()
    {
        var time = Time.deltaTime;
        if (nearlestBoob == null || nearlestBoob.IsDetected)
        {
            nearlestBoob = null;
            FindNearlestBoob();
        }
        if (nearlestBoob==null)
        {
            return;
        }
        inputs = GetInputs();//OYM:��ȡ��ǰinput������
        inputs = ProcessInputs(inputs, time); //OYM:����input������
        output = nb.SetInput(inputs); //OYM:ǰ�򴫲�
        tank.leftWheel =(float) output[0,0];
        tank.rightWheel = (float)output[0,1];
        tank.Move();

        nb.AddFitness( 1/Vector3.Distance(nearlestBoob.transform.position, transform.position));
    }


    private double[,] ProcessInputs(double[,] inputs, double time)
    {
        var currentInput = new double[1, inputSize]; // Sensor info
        for (var i = 0; i < inputSize / 2; i++)
        {
            currentInput[0, i] = inputs[0, i];
        }

        for (var i = 0; i < inputSize / 2; i++)
        {
            currentInput[0, i + inputSize / 2] = (currentInput[0, i] - lastInputs[0, i]) * time;
        }

        lastInputs = (double[,])currentInput.Clone();

        return currentInput;
    }

    private double[,] GetInputs()
    {
        Vector3 nearlyBoob = nearlestBoob.transform.position - transform.position;
        Vector3 tankDirection = transform.forward;

        return new double[1, 6] { { nearlyBoob.x, nearlyBoob.z, nearlyBoob.sqrMagnitude, tankDirection.x,tankDirection.z,Vector3.SignedAngle(nearlyBoob,tankDirection,Vector3.up) } };
    }
    private void OnTriggerStay(Collider other)
    {
        var target = other.GetComponent<Boob>();
        if (target == nearlestBoob) //OYM:ע�⣬����ֻ��������Ŀ���,û�����Ĳ���
        {
            nearlestBoob.IsDetected = true;
            nb.AddFitness(100); //OYM:�����Ҷ��ʮ��
        }
    }

    private void FindNearlestBoob()
    {
        float minDis = float.MaxValue;
        for (int i = 0; i < boobList.Length; i++)
        {
            if (!boobList[i].IsDetected)
            {
                if (Vector3.Distance(transform.position, boobList[i].transform.position) < minDis)
                {
                    minDis = Vector3.Distance(transform.position, boobList[i].transform.position);
                    nearlestBoob = boobList[i]; //OYM:�ҵ������boob
                }
            }
        }
        if (nearlestBoob==null)
        {
            nb.lifeTime = 0; //OYM:kill
        }
    }
}
