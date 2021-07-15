using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EvolutionaryPerceptron.MendelMachine;
using UnityEngine.UI;

public class TankNeuralNetManager : MendelMachine
{
    //OYM:坦克机
    public NeuralTank[] tankList;
    public Boob[] BoobList;
    public Boob boobPrefab;
    public CameraControl m_CameraControl;
    public Bounds AreaBounds;
    public float lifetime;
    private int tankCount;
    public int BoobCount = 20;//OYM:初始炸弹数量
    public Text m_MessageText;
    private ParticleSystem m_ExplosionParticles;
    protected override void Start()
    {
        BoobList = new Boob[BoobCount];//OYM: 所有的boob集合
        base.Start();
        StartCoroutine(InstantiateBotCoroutine());
    }
    public override void NeuralBotDestroyed(Brain neuralBot)
    {
        base.NeuralBotDestroyed(neuralBot);

        Destroy(neuralBot.gameObject);

        tankCount--;

        if (tankCount <= 0)
        {
            Save();
            population = Mendelization(); //OYM:完成杂交.突变,筛选blabla
            generation++; //OYM:增加一代

            StartCoroutine(InstantiateBotCoroutine());
        }
    }
    private IEnumerator InstantiateBotCoroutine() //OYM:初始化的一个类
    {
        yield return null;//OYM:等待一帧

        tankCount = individualsPerGeneration;
        SpawnAlBoobs();
        SpawnAllTanks();
        SetCameraTargets();
        m_MessageText.text = "第" + generation + "代";
    }
    void SpawnAllTanks()
    {
        tankList = new NeuralTank[tankCount];
        for (int i = 0; i < tankCount; i++)
        {
            Vector3 spwanPoint = new Vector3(Random.Range(AreaBounds.max.x, AreaBounds.min.x), 0, Random.Range(AreaBounds.max.x, AreaBounds.min.x));
            Quaternion spwanRotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);

            var brain = InstantiateBot(population[i], lifetime, spwanPoint, spwanRotation, i); //OYM:创建一个小坦克示例
            brain.AddFitness(-brain.Fitness * 0.5f);
            tankList[i] = brain.GetComponent<NeuralTank>();
            tankList[i].boobList = BoobList;
        }
    }

    private void SetCameraTargets()
    {
        // Create a collection of transforms the same size as the number of tanks.
        Transform[] targets = new Transform[tankList.Length];

        // For each of these transforms...
        for (int i = 0; i < targets.Length; i++)
        {
            // ... set it to the appropriate tank transform.
            targets[i] = tankList[i].transform;
        }

        // These are the targets the camera should follow.
        m_CameraControl.m_Targets = targets;
    }

    void SpawnAlBoobs()
    {
        for (int i = 0; i < BoobCount; i++)
        {
            if (BoobList[i]==null)
            {
                BoobList[i] = Instantiate(boobPrefab);
            }
            BoobList[i].IsDetected = false; //OYM:重置
            //OYM:创建随机位置和随机旋转
            Vector3 spwanPoint = new Vector3(Random.Range(AreaBounds.max.x, AreaBounds.min.x), 0, Random.Range(AreaBounds.max.x, AreaBounds.min.x));
            Quaternion spwanRotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);

            BoobList[i].transform.position = spwanPoint;
            BoobList[i].transform.rotation = spwanRotation;
        }
    }
}

