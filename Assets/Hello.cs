using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using Color = UnityEngine.Color;

public class Hello : MonoBehaviour
{
    static int gridSize = 10;
    Renderer[,] rendererGrid = new Renderer[gridSize, gridSize];
    Color[] colors = { Color.red, Color.yellow, Color.green, Color.blue, Color.gray };
    List<int> colorsIDs = new List<int> { 0, 1, 2, 3, 4 };
    bool[,] finished = new bool[gridSize, gridSize];

    void Start()
    {
        Debug.Log("Starting...");
        GenerateGrid();

        //StartCoroutine(PaintGrid());

        StartCoroutine(WaveFunctionCollapse());
    }

    void Update()
    {

    }

    void GenerateGrid()
    {
        Debug.Log("Generating grid...");

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(i, 0, j);

                var cubeRenderer = cube.GetComponent<Renderer>();
                rendererGrid[i, j] = cubeRenderer;
            }
        }

        Debug.Log("Grid generation finished.");
    }

    IEnumerator PaintGrid()
    {
        Debug.Log("Starting coloring in 3s...");
        yield return new WaitForSeconds(3);

        for (int i = 0; i < gridSize; i++)
            for (int j = 0; j < gridSize; j++)
            {
                Color col = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
                rendererGrid[i,j].material.SetColor("_Color", col);

                yield return new WaitForSeconds(0.1f);
            }
    }

    IEnumerator WaveFunctionCollapse()
    {
        // creating array of possibilities
        List<int>[,] possibleOutputs = new List<int>[gridSize, gridSize];
        for (int i = 0; i < gridSize; i++)
            for (int j = 0;j < gridSize; j++)
                possibleOutputs[i,j] = colorsIDs;

        System.Random random = new System.Random();
        
        //while (true)
        for (int xd=0; xd < gridSize*10; xd++)
        {
            yield return new WaitForSeconds(0.4f);

            // let's find out what is the lowest number of possibilities...
            int lowest = possibleOutputs[0,0].Count();
            for (int i = 0; i < gridSize; i++)
                for (int j = 0; j < gridSize; j++)
                    if (lowest > possibleOutputs[i, j].Count() && !finished[i, j])
                        lowest = possibleOutputs[i, j].Count();

            // ...and put them into a list
            var lowList = new List<(int, int)>();
            for (int i = 0; i < gridSize; i++)
                for (int j = 0; j < gridSize; j++)
                    if (lowest == possibleOutputs[i, j].Count())
                        lowList.Add((i, j));

            // now let's pick one randomly, and choose it's value
            int randomIndex = random.Next(lowList.Count());
            (int,int) chosenOne = lowList[randomIndex];

            int randomColorIndex = random.Next(possibleOutputs[chosenOne.Item1, chosenOne.Item2].Count());
            int colorIndex = possibleOutputs[chosenOne.Item1, chosenOne.Item2][randomColorIndex];
            possibleOutputs[chosenOne.Item1, chosenOne.Item2] = new List<int> { colorIndex };

            rendererGrid[chosenOne.Item1, chosenOne.Item2].material.SetColor("_Color", colors[colorIndex]);
            finished[chosenOne.Item1, chosenOne.Item2] = true;

            // we have to update possibilities based on what we want - first - adding neighbours which obviously are affected
            Stack<(int, int)> toUpdate = new Stack<(int, int)>();
            toUpdate.Push((chosenOne.Item1, chosenOne.Item2));

            // right after - loop of changes - it has to be customized! right now it is only example of usage to get started with
            while (toUpdate.Count > 0)
            {
                var itemToUpdate = toUpdate.Pop();
                if (possibleOutputs[itemToUpdate.Item1, itemToUpdate.Item2].Count() == 1)
                {
                    int colorToDelete = possibleOutputs[itemToUpdate.Item1, itemToUpdate.Item2][0];
                    Stack<(int, int)> neighbours = GetNeighbours(itemToUpdate.Item1, itemToUpdate.Item2);
                    
                    while(neighbours.Count > 0)
                    {
                        (int, int) nghb = neighbours.Pop();
                        if (possibleOutputs[nghb.Item1, nghb.Item2].Contains(colorToDelete))
                        {
                            var newValues = new List<int> (possibleOutputs[nghb.Item1, nghb.Item2]);
                            possibleOutputs[nghb.Item1, nghb.Item2] = new List<int> ( GetPossibleColors(newValues, colorToDelete) );
                            toUpdate.Push(nghb);
                        }
                    }
                }
            }
        }
    }

    private Stack<(int, int)> GetNeighbours(int a, int b)
    {
        Stack<(int, int)> up = new Stack<(int, int)>();

        if (a - 1 >= 0)
            up.Push((a - 1, b));
        if (a + 1 < gridSize)
            up.Push((a + 1, b));
        if (b - 1 >= 0)
            up.Push((a, b - 1));
        if (b + 1 < gridSize)
            up.Push((a, b + 1));



        return up;
    }

    private List<int> GetPossibleColors(List<int> values, int color)
    {
        List<int> vals = new List<int>();
        if (values.Contains(color-1))
            vals.Add(color-1);
        if (values.Contains(color+1))
            vals.Add(color+1);

        if (vals.Count == 0)
            throw new NotImplementedException();

        return vals;
    }
}
