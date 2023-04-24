using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using Color = UnityEngine.Color;

public struct CubeNeighbourhood
{
    // to think about - what type
    //public int top;
    //public int bottom;
    public int leftSide, rightSide, frontSide, backSide;

    public CubeNeighbourhood(int l, int r, int f, int b)
    {
        leftSide = l;
        rightSide = r;
        frontSide = f;
        backSide = b;
    }
}

public class Hello : MonoBehaviour
{
    CubeNeighbourhood greenNeighbour = new CubeNeighbourhood(3, 2, 3, 2);
    CubeNeighbourhood blueNeighbour = new CubeNeighbourhood(2, 1, 2, 1);
    CubeNeighbourhood grayNeighbour = new CubeNeighbourhood(1, 1, 1, 1);

    static int[] gridSize = new int[3] {3,3,3}; // height is second!!
    Renderer[,,] rendererGrid = new Renderer[gridSize[0], gridSize[1], gridSize[2]];
    Color[] colors = { Color.green, Color.blue, Color.gray };
    List<int> colorsIDs = new List<int> { 0, 1, 2 };
    List<int>[,,] possibleOutputs = new List<int>[gridSize[0], gridSize[1], gridSize[2]];

    void Start()
    {
        Debug.Log("Starting...");

        GenerateGrid3D();
        WaveFunctionCollapse3D();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            WaveFunctionCollapse3D();
    }

    void GenerateGrid3D()
    {
        Debug.Log("Generating grid...");

        for (int i = 0; i < gridSize[0]; i++)
            for (int j = 0; j < gridSize[1]; j++)
                for (int k = 0; k < gridSize[2]; k++)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(i, j, k);

                    var cubeRenderer = cube.GetComponent<Renderer>();
                    rendererGrid[i, j, k] = cubeRenderer;
                }

        Debug.Log("Grid generation finished.");
    }

    public void WaveFunctionCollapse3D()
    {
        // creating array of possibilities
        for (int i = 0; i < gridSize[0]; i++)
            for (int j = 0; j < gridSize[1]; j++)
                for (int k = 0; k < gridSize[2]; k++)
                    possibleOutputs[i, j, k] = colorsIDs;

        System.Random random = new System.Random();

        int lowest = 0;
        while (lowest != 999)
        {
            // let's find out what is the lowest number of possibilities...
            lowest = 999;
            for (int i = 0; i < gridSize[0]; i++)
                for (int j = 0; j < gridSize[1]; j++)
                    for (int k = 0; k < gridSize[2]; k++)
                        if (lowest > possibleOutputs[i, j, k].Count() && possibleOutputs[i, j, k].Count() != 1)
                            lowest = possibleOutputs[i, j, k].Count();

            // ...and put them into a list
            var lowList = new List<(int, int, int)>();

            for (int i = 0; i < gridSize[0]; i++)
                for (int j = 0; j < gridSize[1]; j++)
                    for (int k = 0; k < gridSize[2]; k++)
                        if (lowest == possibleOutputs[i, j, k].Count())
                            lowList.Add((i, j, k));

            if (lowList.Count == 0)
                continue;
    
            // now let's pick one randomly, and choose it's value
            int randomIndex = random.Next(lowList.Count());
            (int, int, int) id = lowList[randomIndex];

            int randomColorIndex = random.Next(possibleOutputs[id.Item1, id.Item2, id.Item3].Count());
            int colorIndex = possibleOutputs[id.Item1, id.Item2, id.Item3][randomColorIndex];
            possibleOutputs[id.Item1, id.Item2, id.Item3] = new List<int> { colorIndex };

            rendererGrid[id.Item1, id.Item2, id.Item3].material.SetColor("_Color", colors[colorIndex]);

            UpdateWhileChanges();
        }
    }

    public void UpdateWhileChanges()
    {

    }
    
        //private Stack<(int, int)> GetNeighbours(int a, int b)
        //{
        //    Stack<(int, int)> up = new Stack<(int, int)>();

        //    if (a - 1 >= 0)
        //        up.Push((a - 1, b));
        //    if (a + 1 < gridSize)
        //        up.Push((a + 1, b));
        //    if (b - 1 >= 0)
        //        up.Push((a, b - 1));
        //    if (b + 1 < gridSize)
        //        up.Push((a, b + 1));



        //    return up;
        //}

    //private List<int> GetPossibleColors(List<int> values, int color)
    //{
    //    List<int> vals = new List<int>();
    //    if (values.Contains(color-1))
    //        vals.Add(color-1);
    //    if (values.Contains(color+1))
    //        vals.Add(color+1);

    //    if (vals.Count == 0)
    //        throw new NotImplementedException();

    //    return vals;
    //}
}
