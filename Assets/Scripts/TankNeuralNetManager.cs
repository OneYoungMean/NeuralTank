using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EvolutionaryPerceptron.MendelMachine;
using UnityEngine.UI;

public class TankNeuralNetManager : MendelMachine
{
    //OYM:̹�˻�
    public NeuralTank[] tankList;
    public Boob[] BoobList;
    public Boob boobPrefab;
    public CameraControl m_CameraControl;
    public Bounds AreaBounds;
    public float lifetime;
    private int tankCount;
    public int BoobCount = 20;//OYM:��ʼը������
    public Text m_MessageText;
    private ParticleSystem m_ExplosionParticles;
    protected override void Start()
    {
        BoobList = new Boob[BoobCount];//OYM: ���е�boob����
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
            population = Mendelization(); //OYM:����ӽ�.ͻ��,ɸѡblabla
            generation++; //OYM:����һ��

            StartCoroutine(InstantiateBotCoroutine());
        }
    }
    private IEnumerator InstantiateBotCoroutine() //OYM:��ʼ����һ����
    {
        yield return null;//OYM:�ȴ�һ֡

        tankCount = individualsPerGeneration;
        SpawnAlBoobs();
        SpawnAllTanks();
        SetCameraTargets();
        m_MessageText.text = "��" + generation + "��";
    }
    void SpawnAllTanks()
    {
        tankList = new NeuralTank[tankCount];
        for (int i = 0; i < tankCount; i++)
        {
            Vector3 spwanPoint = new Vector3(Random.Range(AreaBounds.max.x, AreaBounds.min.x), 0, Random.Range(AreaBounds.max.x, AreaBounds.min.x));
            Quaternion spwanRotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);

            var brain = InstantiateBot(population[i], lifetime, spwanPoint, spwanRotation, i); //OYM:����һ��С̹��ʾ��
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
            BoobList[i].IsDetected = false; //OYM:����
            //OYM:�������λ�ú������ת
            Vector3 spwanPoint = new Vector3(Random.Range(AreaBounds.max.x, AreaBounds.min.x), 0, Random.Range(AreaBounds.max.x, AreaBounds.min.x));
            Quaternion spwanRotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);

            BoobList[i].transform.position = spwanPoint;
            BoobList[i].transform.rotation = spwanRotation;
        }
    }
}

