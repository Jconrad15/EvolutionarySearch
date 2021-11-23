using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace EvolutionaryDungeon
{
    public class MainController : MonoBehaviour
    {
        [SerializeField]
        private GameObject openSpacePrefab;

        [SerializeField]
        private GameObject startPrefab;

        [SerializeField]
        private GameObject exitPrefab;

        [SerializeField]
        private GameObject treasurePrefab;

        [SerializeField]
        private GameObject monsterPrefab;

        [SerializeField]
        private GameObject wallPrefab;

        [SerializeField]
        private GameObject backgroundPrefab;

        private List<GameObject> currentDungeons;

        private int mu = 200;
        private int lambda = 200;
        private int scoreThreshold = 280;
        private int maxGenerations = 400;
        private int displaySlice = 50;

        private int generationDisplayed = 0;

        [SerializeField]
        private TextMeshProUGUI processingText;

        [SerializeField]
        private CanvasGroup arrowImages;

        // Start is called before the first frame update
        void Start()
        {
            processingText.alpha = 0;
            arrowImages.alpha = 0;

            currentDungeons = new List<GameObject>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DeleteDungeons();

                // Create a new dungeon
                StartCoroutine(PerfromEvolutionaryProcess());
            }
        }

        private IEnumerator PerfromEvolutionaryProcess()
        {
            CreateGeneratingMessage();

            int currentGeneration = 1;
            bool isDone = false;

            Individual[] population = EvolutionProcess.InitializePopulation(mu, lambda);
            while (isDone == false)
            {
                int[] scores = EvolutionProcess.EvaluationProcess(population);

                (population, scores) = EvolutionProcess.SortIndividuals(population, scores);

                int highestScore = EvolutionProcess.HighestSortedScore(scores);
                Debug.Log("Highest Score for generation " + currentGeneration +
                          " is: " + highestScore);

                // Display the first generation
                // Display every x-th dungeon
                if (currentGeneration % displaySlice == 0 || currentGeneration == 1)
                {
                    StartCoroutine(DisplayDungeon(population[0], currentGeneration));
                    yield return null;
                }

                // End Conditions
                if (highestScore > scoreThreshold)
                {
                    Debug.Log("Score threshold met");
                    isDone = true;
                    StartCoroutine(DisplayDungeon(population[0], currentGeneration));
                }
                else
                {
                    if (currentGeneration >= maxGenerations)
                    {
                        isDone = true;
                        Debug.Log("Max Number of Generations");
                    }
                    currentGeneration += 1;

                    population = EvolutionProcess.CreateOffspring(population, mu, lambda);
                }
            }
            RemoveGeneratingMessage();
        }

        /// <summary>
        /// Draws the provided dungeon in the scene.
        /// </summary>
        /// <param name="bestDungeon"></param>
        private IEnumerator DisplayDungeon(Individual bestDungeon, int generation)
        {
            generationDisplayed += 1;


            // Create dungeon gameobject
            GameObject newDungeon = new GameObject("Dungeon " + generation);

            int generationalOffset = generationDisplayed * 13;

            // Add the background prefab
            GameObject background_go = Instantiate(
                backgroundPrefab,
                new Vector3(generationalOffset + (Individual.xSize / 2f) - 0.5f, (Individual.ySize / 2f) - 0.5f, 0),
                Quaternion.identity);

            background_go.transform.SetParent(newDungeon.transform);
            Canvas canvas = background_go.GetComponentInChildren<Canvas>();
            TextMeshProUGUI text = canvas.GetComponentInChildren<TextMeshProUGUI>();
            text.SetText("Generation - " + generation);

            for (int x = 0; x < Individual.xSize; x++)
            {
                for (int y = 0; y < Individual.ySize; y++)
                {
                    Individual.ElementType elementValue = (Individual.ElementType)bestDungeon.ElementGrid[x,y];
                    GameObject new_go;
                    switch (elementValue)
                    {
                        case Individual.ElementType.OpenSpace:
                            new_go = Instantiate(openSpacePrefab, new Vector3(x + generationalOffset, y, 0), Quaternion.identity);
                            new_go.transform.SetParent(newDungeon.transform);
                            break;

                        case Individual.ElementType.Start:
                            new_go = Instantiate(startPrefab, new Vector3(x + generationalOffset, y, 0), Quaternion.identity);
                            new_go.transform.SetParent(newDungeon.transform);
                            break;

                        case Individual.ElementType.Exit:
                            new_go = Instantiate(exitPrefab, new Vector3(x + generationalOffset, y, 0), Quaternion.identity);
                            new_go.transform.SetParent(newDungeon.transform);
                            break;

                        case Individual.ElementType.Treasure:
                            new_go = Instantiate(treasurePrefab, new Vector3(x + generationalOffset, y, 0), Quaternion.identity);
                            new_go.transform.SetParent(newDungeon.transform);
                            break;

                        case Individual.ElementType.Monster:
                            new_go = Instantiate(monsterPrefab, new Vector3(x + generationalOffset, y, 0), Quaternion.identity);
                            new_go.transform.SetParent(newDungeon.transform);
                            break;

                        case Individual.ElementType.Wall:
                            new_go = Instantiate(wallPrefab, new Vector3(x + generationalOffset, y, 0), Quaternion.identity);
                            new_go.transform.SetParent(newDungeon.transform);
                            break;

                        default:
                            Debug.LogError("Error - No elementType");
                            break;
                    }
                } // end for y
            } // end for x

            currentDungeons.Add(newDungeon);
            yield return null;
        }

        /// <summary>
        /// Destroys all dungeons in the scene
        /// </summary>
        private void DeleteDungeons()
        {
            for (int i = 0; i < currentDungeons.Count; i++)
            {
                Destroy(currentDungeons[i]);
            }
            currentDungeons = new List<GameObject>();
            generationDisplayed = 0;
        }

        private void RemoveGeneratingMessage()
        {
            processingText.alpha = 0;
            arrowImages.alpha = 1;
        }

        private void CreateGeneratingMessage()
        {
            processingText.alpha = 1;
            arrowImages.alpha = 0;
        }


    }
}