using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolutionaryDungeon
{
    public class Individual
    {
        public const int xSize = 10;
        public const int ySize = 10;

        public int[,] ElementGrid { get; protected set; }

        public int OpenSpaceCount { get; protected set; }
        public int StartCount { get; protected set; }
        public int ExitCount { get; protected set; }
        public int TreasureCount { get; protected set; }
        public int MonsterCount { get; protected set; }
        public int WallCount { get; protected set; }


        public enum ElementType { OpenSpace, Start, Exit, Treasure, Monster, Wall};


        /// <summary>
        /// Standard Random Constructor
        /// </summary>
        public Individual()
        {
            ElementGrid = new int[xSize, ySize];
            OpenSpaceCount = StartCount = ExitCount = TreasureCount = MonsterCount = WallCount = 0;

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    ElementType selectedElement = Utility.GetRandomEnum<ElementType>();
                    
                    // Generate only 1 start location
                    if (StartCount >= 1)
                    {                        
                        while (selectedElement == ElementType.Start)
                        {
                            // Get a new elementType
                            selectedElement = Utility.GetRandomEnum<ElementType>();
                        }
                    }

                    // Generate only 1 exit location
                    if (ExitCount >= 1)
                    {
                        while (selectedElement == ElementType.Exit)
                        {
                            // Get a new elementType
                            selectedElement = Utility.GetRandomEnum<ElementType>();
                        }
                    }

                    ElementGrid[x, y] = (int)selectedElement;
                    switch (selectedElement)
                    {
                        case ElementType.OpenSpace:
                            OpenSpaceCount += 1;
                            break;

                        case ElementType.Start:
                            StartCount += 1;
                            break;

                        case ElementType.Exit:
                            ExitCount += 1;
                            break;

                        case ElementType.Treasure:
                            TreasureCount += 1;
                            break;

                        case ElementType.Monster:
                            MonsterCount += 1;
                            break;

                        case ElementType.Wall:
                            WallCount += 1;
                            break;

                        default:
                            Debug.LogError("Error - No elementType");
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="previousIndividual"></param>
        public Individual(Individual previousIndividual)
        {
            ElementGrid = (int[,])previousIndividual.ElementGrid.Clone();
            OpenSpaceCount = previousIndividual.OpenSpaceCount;
            StartCount = previousIndividual.StartCount;
            ExitCount = previousIndividual.ExitCount;
            TreasureCount = previousIndividual.TreasureCount;
            MonsterCount = previousIndividual.MonsterCount;
            WallCount = previousIndividual.WallCount;
        }

        public (Vector2Int, Vector2Int) FindStartExit()
        {
            Vector2Int startPosition = new Vector2Int();
            Vector2Int exitPosition = new Vector2Int();

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    switch ((ElementType)ElementGrid[x, y])
                    {
                        case ElementType.Start:
                            startPosition = new Vector2Int(x, y);
                            break;

                        case ElementType.Exit:
                            exitPosition = new Vector2Int(x, y);
                            break;

                        default:
                            break;
                    }
                }
            }

            return (startPosition, exitPosition);
        }

        private void CountUpdate()
        {
            OpenSpaceCount = StartCount = ExitCount = TreasureCount = MonsterCount = WallCount = 0;

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    switch ((ElementType)ElementGrid[x, y])
                    {
                        case ElementType.OpenSpace:
                            OpenSpaceCount += 1;
                            break;

                        case ElementType.Start:
                            StartCount += 1;
                            break;

                        case ElementType.Exit:
                            ExitCount += 1;
                            break;

                        case ElementType.Treasure:
                            TreasureCount += 1;
                            break;

                        case ElementType.Monster:
                            MonsterCount += 1;
                            break;

                        case ElementType.Wall:
                            WallCount += 1;
                            break;

                        default:
                            Debug.LogError("Error - No elementType");
                            break;
                    }
                }
            }
        }

        public void Mutate(Individual indiv)
        {
            int numberToMutate;
            if (Random.value < 0.7f)
            {
                numberToMutate = 1;
            }
            else
            {
                numberToMutate = 5;
            }

            List<Vector2> elementsToMutate = GenerateRandomLocationList(numberToMutate);

            for (int i = 0; i < numberToMutate; i++)
            {
                ElementType oldType = (ElementType)indiv.ElementGrid[(int)elementsToMutate[i].x, (int)elementsToMutate[i].y];
                ElementType newType = Utility.GetRandomEnum<ElementType>();
                while (oldType == newType)
                {
                    newType = Utility.GetRandomEnum<ElementType>();
                }

                indiv.ElementGrid[(int)elementsToMutate[i].x, (int)elementsToMutate[i].y] = (int)newType;
            }

            // Also update the counts
            CountUpdate();
        }

        private List<Vector2> GenerateRandomLocationList(int numberToMutate)
        {
            List<Vector2> randomList = new List<Vector2>();

            for (int i = 0; i < numberToMutate; i++)
            {
                Vector2 vectorToAdd = new Vector2(Random.Range(0, xSize), Random.Range(0, ySize));
                while (randomList.Contains(vectorToAdd))
                {
                    vectorToAdd = new Vector2(Random.Range(0, xSize), Random.Range(0, ySize));
                }
                randomList.Add(vectorToAdd);

            }
            return randomList;
        }


    }
}