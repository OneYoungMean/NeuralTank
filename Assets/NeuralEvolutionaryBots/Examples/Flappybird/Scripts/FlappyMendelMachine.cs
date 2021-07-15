﻿using System.Collections;
using UnityEngine;

namespace EvolutionaryPerceptron.Examples.FlappyBird {
    using EvolutionaryPerceptron.MendelMachine;

    public class FlappyMendelMachine : MendelMachine
    { //OYM:愤怒小鸟机
        private int index = 15; //OYM:初始鸟鸟的数量
        Transform startPoint;
        protected override void Start () {
            base.Start ();
            StartCoroutine (InstantiateBotCoroutine ());
            
        }
        public override void NeuralBotDestroyed (Brain neuralBot) {
            base.NeuralBotDestroyed (neuralBot);

            Destroy (neuralBot.gameObject);

            index--;

            if (index <= 0) {
                Save ();
                population = Mendelization ();
                generation++;

                StartCoroutine (InstantiateBotCoroutine ());
            }
        }
        private IEnumerator InstantiateBotCoroutine () {
            yield return null;
            index = individualsPerGeneration;

            for (int i = 0; i < population.Length; i++) {
                InstantiateBot (population[i], 999999, startPoint, i);
            }
        }
    }
}