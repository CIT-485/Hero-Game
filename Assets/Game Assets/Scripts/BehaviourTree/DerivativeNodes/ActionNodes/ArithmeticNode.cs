using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArithmeticNode : ActionNode
{
    public bool getBlackboardKey;
    public float enteredValue;

    public string operandOne;
    public string operandTwo;
    public string resultName;

    public int operandOneIndex;
    public int operandOneCount;
    public int operandOneInt;
    public float operandOneFloat;
    public bool operandOneUseInt;
    public List<string> operandOneList = new List<string>();

    public int operatorIndex;
    public List<string> operatorList = new List<string>();

    public int operandTwoIndex;
    public int operandTwoCount;
    public int operandTwoInt;
    public float operandTwoFloat;
    public bool operandTwoUseInt;
    public List<string> operandTwoList = new List<string>();

    public int resultIndex;
    public int resultCount;
    public float result;
    public List<string> resultList = new List<string>();

    public State finishState = State.SUCCESS;
    protected override void OnStart()
    {
        finishState = State.SUCCESS;
    }
    protected override State OnUpdate()
    {
        string[] operandOne = operandOneList[operandOneIndex].Split(new string[] { " (" }, System.StringSplitOptions.None);
        if (operandOne[1].Contains("Integer"))
        {
            operandOneInt = blackboard.integers.GetValue(operandOne[0]);
            operandOneUseInt = true;
        }
        else if (operandOne[1].Contains("Float"))
            operandOneFloat = blackboard.floats.GetValue(operandOne[0]);
        else if (operandOne[1].Contains("[X]"))
            operandOneFloat = blackboard.vector2s.GetValue(operandOne[0]).x;
        else
            operandOneFloat = blackboard.vector2s.GetValue(operandOne[0]).y;

        string[] operandTwo = operandTwoList[operandTwoIndex].Split(new string[] { " (" }, System.StringSplitOptions.None);
        if (operandTwo[1].Contains("Integer"))
        {
            operandTwoInt = blackboard.integers.GetValue(operandTwo[0]);
            operandTwoUseInt = true;
        }
        else if (operandTwo[0].Contains("Time.deltaTime"))
            operandTwoFloat = Time.deltaTime;
        else if (operandTwo[1].Contains("Float"))
            operandTwoFloat = blackboard.floats.GetValue(operandTwo[0]);
        else if (operandTwo[1].Contains("[X]"))
            operandTwoFloat = blackboard.vector2s.GetValue(operandTwo[0]).x;
        else
            operandTwoFloat = blackboard.vector2s.GetValue(operandTwo[0]).y;

        if (getBlackboardKey)
        {
            if (operandOneUseInt && operandTwoUseInt)
                GetResult(operandOneInt, operandTwoInt);
            else if (operandOneUseInt && !operandTwoUseInt)
                GetResult(operandOneInt, operandTwoFloat);
            else if (!operandOneUseInt && operandTwoUseInt)
                GetResult(operandOneFloat, operandTwoInt);
            else
                GetResult(operandOneFloat, operandTwoFloat);
        }
        else
        {
            if (operandOneUseInt)
                GetResult(operandOneInt, enteredValue);
            else
                GetResult(operandOneFloat, enteredValue);
        }

        string[] resultString = resultList[resultIndex].Split(new string[] { " (" }, System.StringSplitOptions.None);
        if (operandOne[1].Contains("Integer"))
            blackboard.integers.GetValue(resultString[0]) = (int)result;
        else if (resultString[1].Contains("Float"))
            blackboard.floats.GetValue(resultString[0]) = result;
        else if (resultString[1].Contains("[X]"))
            blackboard.vector2s.GetValue(resultString[0]).x = result;
        else
            blackboard.vector2s.GetValue(resultString[0]).y = result;

        return finishState;
    }
    public override void OnBeforeSerialize()
    {
        operandOneList.Clear();
        operatorList.Clear();
        operandTwoList.Clear();
        resultList.Clear();
        operandTwoList.Add("Time.deltaTime (secs)");
        foreach (Key<int> key in blackboard.integers.keys)
        {
            operandOneList.Add(key.name + " (Integer Key)");
            operandTwoList.Add(key.name + " (Integer Key)");
            resultList.Add(key.name + " (Integer Key)");
        }
        foreach (Key<float> key in blackboard.floats.keys)
        {
            operandOneList.Add(key.name + " (Float Key)");
            operandTwoList.Add(key.name + " (Float Key)");
            resultList.Add(key.name + " (Float Key)");
        }
        foreach (Key<Vector2> key in blackboard.vector2s.keys)
        {
            operandOneList.Add(key.name + " (Vector2 Key [X])");
            operandTwoList.Add(key.name + " (Vector2 Key [X])");
            resultList.Add(key.name + " (Vector2 Key [X])");
        }
        foreach (Key<Vector2> key in blackboard.vector2s.keys)
        {
            operandOneList.Add(key.name + " (Vector2 Key [Y])");
            operandTwoList.Add(key.name + " (Vector2 Key [Y])");
            resultList.Add(key.name + " (Vector2 Key [Y])");
        }


        if (operandOneCount != operandOneList.Count)
        {
            operandOneCount = operandOneList.Count;
            bool found = false;
            int i = 0;
            for (i = 0; i < operandOneList.Count && !found; i++)
                if (operandOneList[i] == operandOne)
                    found = true;
            if (found)
                operandOneIndex = i - 1;
            else
                operandOneIndex = 0;
        }
        if (operandTwoCount != operandTwoList.Count)
        {
            operandTwoCount = operandTwoList.Count;
            bool found = false;
            int i = 0;
            for (i = 0; i < operandTwoList.Count && !found; i++)
                if (operandTwoList[i] == operandTwo)
                    found = true;
            if (found)
                operandTwoIndex = i - 1;
            else
                operandTwoIndex = 0;
        }
        if (resultCount != resultList.Count)
        {
            resultCount = resultList.Count;
            bool found = false;
            int i = 0;
            for (i = 0; i < resultList.Count && !found; i++)
                if (resultList[i] == resultName)
                    found = true;
            if (found)
                resultIndex = i - 1;
            else
                resultIndex = 0;
        }

        if (operandOneCount > 0)
            operandOne = operandOneList[operandOneIndex];
        if (operandTwoCount > 0)
            operandTwo = operandTwoList[operandTwoIndex];
        if (resultCount > 0)
            resultName = resultList[resultIndex];
    }
    private void GetResult(float op1, float op2)
    {

        switch (operatorIndex)
        {
            case 0:
                result = op1 + op2;
                break;
            case 1:
                result = op1 - op2;
                break;
            case 2:
                result = op1 * op2;
                break;
            case 3:
                if (operandTwoInt != 0)
                    result = op1 / op2;
                else
                    finishState = State.FAILURE;
                break;
        }
    }
}
