using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolutionaryDungeon
{
    public class EvolutionProcess
    {
        public static Individual[] InitializePopulation(int mu = 10, int lambda = 10)
        {
            Individual[] population = new Individual[mu + lambda];
            for (int i = 0; i < mu + lambda; i++)
            {
                population[i] = new Individual();
            }
            return population;
        }

        public static int[] EvaluationProcess(Individual[] population)
        {
            int[] scores = new int[population.Length];
            for (int i = 0; i < population.Length; i++)
            {
                scores[i] = EvaluationFunction(population[i]);
            }
            return scores;
        }

        private static int EvaluationFunction(Individual individual)
        {
            // Start Score at 0
            int score = 0;

            // Score is higher if number of treasures and monsters is the same
            score -= Mathf.Abs(individual.MonsterCount - individual.TreasureCount);

            // Score is lower if there are more than five treasures
            if (individual.TreasureCount > 5)
            { score -= individual.TreasureCount; }

            // Score is lower if there is more than one start or no starts
            if (individual.StartCount == 1) { score += 40; }
            else { score -= 40; }

            // Score is lower if there is more than one exit or no exits
            if (individual.ExitCount == 1) { score += 40; }
            else { score -= 40; }

            // Score is higher if the distance between the start and the exit is longer
            // Run A Star protocol
            if (individual.ExitCount == 1 && individual.StartCount == 1)
            {
                int distanceScore = StartExitDistance(individual);
                // Add an extra score for connecting the start and end else remove score
                if (distanceScore > 3) { score += 100; }
                else { score -= 40; }
                score += distanceScore * 2;
            }

            return score;
        }

        private static int StartExitDistance(Individual individual)
        {
            int[][] map = Utility.ToJaggedArray(individual.ElementGrid);

            (Vector2Int start, Vector2Int end) = individual.FindStartExit();
            bool[][] boolMap = Astar.ConvertToBoolArray(map);
            List<Vector2Int> result = new Astar(boolMap, start, end).Result;
            
            return result.Count;
        }



        public static (Individual[], int[]) SortIndividuals(Individual[] population, int[] scores)
        {
            // Sort the scores and population in ascending order of the scores
            Array.Sort(scores, population);
            // Reverse the scores to be in descending order
            Array.Reverse(scores);
            Array.Reverse(population);

            return (population, scores);
        }

        public static Individual[] CreateOffspring(Individual[] population, int mu, int lambda)
        {
            Individual[] clonePopulation = (Individual[])population.Clone();

            for (int i = 0; i < mu; i++)
            {
                // Remove the worst half of individuals
                // Replace the worst half with copies of the best half
                // Using a copy constructor
                clonePopulation[i + mu] = new Individual(clonePopulation[i]);
            }
            for (int j = mu; j < mu + lambda; j++)
            {
                // Mutate the copies
                clonePopulation[j].Mutate(clonePopulation[j]);
            }
            return clonePopulation;
        }

        public static int HighestSortedScore(int[] scores)
        {
            return scores[0];
        }



    }
}