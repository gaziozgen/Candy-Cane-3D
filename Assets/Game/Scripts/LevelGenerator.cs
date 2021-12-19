using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // prefabs
    [SerializeField] private GameObject saveMachine = null;
    [SerializeField] private GameObject[] souceMachines = null;
    [SerializeField] private GameObject[] sprinkleMachines = null;
    [SerializeField] private GameObject[] obstacles = null;
    [SerializeField] private GameObject finalMachine = null;
    [SerializeField] private GameObject collectibleCandy = null;
    [SerializeField] private GameObject road = null;
    [SerializeField] private GameObject environmentUnit = null;

    // parent objects
    [SerializeField] private Transform platform = null;
    [SerializeField] private Transform collectibleCandies = null;
    [SerializeField] private Transform machines = null;
    [SerializeField] private Transform environment = null;

    // generate inputs
    [SerializeField] private int partCount = 6;
    [SerializeField] private int singlePartLength = 5;
    [SerializeField] private int maxCandySize = 1;
    [SerializeField] private int maxMachineSize = 1;

    // last created prefabs's position info
    private float currentPosition = 0;
    private float nextObstaclePosition =-2;
    private float nextUpgradeMachineScale = 1;
    private int leftCandyCount;
    private int leftMachineCount;


    public void Generate()
    {
        currentPosition += 10;

        RandomCandyGroup();
        RandomObstacle();
        SouceAndUpgrade();

        for (int i = 0; i < partCount; i++)
        {
            leftCandyCount = maxCandySize;
            leftMachineCount = maxMachineSize;

            for (int j = 0; j < singlePartLength; j++)
            {
                if (!LevelPart())
                {
                    j--;
                    continue;
                }
            }

            SouceAndUpgrade();
        }

        #region end

        // final machine
        currentPosition += 20;
        Instantiate(finalMachine, new Vector3(0, 0, currentPosition), Quaternion.identity, machines);

        // environment
        /*for (int i = 0; i < currentPosition; i += 28)
        {
            Instantiate(environmentUnit, new Vector3(0, 0, i), Quaternion.identity, environment);
        }*/

        // road
        Instantiate(road, new Vector3(0, 0, -5), Quaternion.identity, platform);
        for (int i = 0; i < currentPosition; i += 5)
        {
            Instantiate(road, new Vector3(0, 0, i), Quaternion.identity, platform);
        }

        #endregion

    }

    private bool LevelPart()
    {
        switch ((int)Random.Range(0f, 3f))
        {
            case 0:
                if (leftCandyCount > 0)
                {
                    leftCandyCount--;
                    RandomCandyGroup();
                }
                else
                {
                    return false;
                }
                break;

            case 1:
                RandomObstacle();
                break;

            case 2:
                if (leftMachineCount > 0)
                {
                    leftMachineCount--;
                    switch ((int)Random.Range(0f, 4f))
                    {
                        case 0:
                            RandomSprinkleMachine();
                            break;

                        case 1:
                            SprinkleMachineCouple();
                            break;

                        case 2:
                            RandomSouceMachine();
                            break;

                        case 3:
                            RandomSouceMachineCouple();
                            break;
                    }
                }
                else
                {
                    return false;
                }
                break;

        }
        return true;
    }

    private void ExampleLevel()
    {
        CandyGroup1();
        SouceAndUpgrade();

        Torpu();
        CandyGroup2();
        Giyotin();
        RandomSprinkleMachine();
        CandyGroup3();
        Saw1();
        Giyotin();
        RandomSprinkleMachine();
        SouceAndUpgrade();

        CandyGroup1();
        Torpu();
        Saw2();
        CandyGroup2();
        RandomUpgradeCouple();
        SouceAndUpgrade();

        CandyGroup1();
        Giyotin();
        Torpu();
        Torpu();
        RandomSouceMachineCouple();
        SprinkleMachineCouple();
        SouceAndUpgrade();

        CandyGroup1();
        RandomSouceMachine();
        Saw1();
        Saw2();
        CandyGroup1();
        SprinkleMachineCouple();
    }

    private void RandomObstacle()
    {
        int rand = (int)Random.Range(0f, 3f);
        switch (rand)
        {
            case 0:
                Giyotin();
                break;

            case 1:
                Torpu();
                break;

            case 2:
                Saw1();
                Saw2();
                break;
        }
    }

    private void Giyotin()
    {
        float distance = 10;

        Instantiate(obstacles[0], new Vector3(0, 0, currentPosition + distance), Quaternion.identity, machines);

        currentPosition += 20;
    }

    private void Torpu()
    {
        float distance = 10;

        Instantiate(obstacles[1], new Vector3(nextObstaclePosition, 0, currentPosition + distance), Quaternion.identity, machines);
        nextObstaclePosition *= -1;

        currentPosition += 20;
    }

    private void Saw1()
    {
        float distance = 10;

        Instantiate(obstacles[2], new Vector3(0, 0, currentPosition + distance), Quaternion.identity, machines);

        currentPosition += 20;
    }

    private void Saw2()
    {
        float distance = 10;

        Instantiate(obstacles[3], new Vector3(nextObstaclePosition, 0, currentPosition + distance), Quaternion.identity, machines);
        nextObstaclePosition *= -1;

        currentPosition += 20;
    }

    private void RandomCandyGroup()
    {
        int rand = (int)Random.Range(0f, 3f);
        switch (rand)
        {
            case 0:
                CandyGroup1();
                break;

            case 1:
                CandyGroup2();
                break;

            case 2:
                CandyGroup3();
                break;
        }

    }

    private void CandyGroup1()
    {
        float distance = 5;

        for (int i = 0; i < 3; i++)
        {
            for (int x = -2; x <= 2; x += 2)
            {
                Instantiate(collectibleCandy, new Vector3(x, 2, (i * 3) + currentPosition + distance), Quaternion.identity, collectibleCandies);
            }
        }

        currentPosition += 20;
    }

    private void CandyGroup2()
    {
        float distance = 5;

        for (int i = 0; i < 5; i++)
        {
            Instantiate(collectibleCandy, new Vector3(i - 2f, 2, (i * 2) + currentPosition + distance), Quaternion.identity, collectibleCandies);
        }

        currentPosition += 15;
    }

    private void CandyGroup3()
    {
        float distance = 5;

        for (int i = 0; i < 5; i++)
        {
            Instantiate(collectibleCandy, new Vector3(2f - i, 2, (i * 2) + currentPosition + distance), Quaternion.identity, collectibleCandies);
        }

        currentPosition += 15;
    }

    private void SouceAndUpgrade()
    {
        float distance = 10;

        Instantiate(saveMachine, new Vector3(2, 0, currentPosition + distance), Quaternion.identity, machines);
        Instantiate(souceMachines[(int)Random.Range(0, 3)], new Vector3(0, 0, currentPosition + distance), Quaternion.identity, machines);

        currentPosition += 20;
    }

    private void RandomUpgradeCouple()
    {
        float distance = 10;

        int souceIndex = Random.Range(0, 3);
        Instantiate(souceMachines[(int)Random.Range(0, 3)], new Vector3(0, 0, currentPosition + distance), Quaternion.identity, machines);
        GameObject machine = Instantiate(sprinkleMachines[(int)Random.Range(0, 2)], new Vector3(0, 0, currentPosition + distance), Quaternion.identity, machines);
        machine.transform.localScale = new Vector3(-1, 1, 1);

        currentPosition += 20;
    }

    private void RandomSouceMachineCouple()
    {
        float distance = 10;

        int souceIndex = Random.Range(0, 3);
        Instantiate(souceMachines[souceIndex], new Vector3(0, 0, currentPosition + distance), Quaternion.identity, machines);
        GameObject machine = Instantiate(souceMachines[(souceIndex+1)%3], new Vector3(0, 0, currentPosition + distance), Quaternion.identity, machines);
        machine.transform.localScale = new Vector3(-1, 1, 1);

        currentPosition += 20;
    }

    private void RandomSouceMachine()
    {
        float distance = 5;

        GameObject machine = Instantiate(souceMachines[(int)Random.Range(0, 3)], new Vector3(0, 0, currentPosition + distance), Quaternion.identity, machines);
        machine.transform.localScale = new Vector3(nextUpgradeMachineScale, 1, 1);
        nextUpgradeMachineScale *= -1;

        currentPosition += 10;
    }

    private void SprinkleMachineCouple()
    {
        float distance = 10;

        Instantiate(sprinkleMachines[0], new Vector3(0, 0, currentPosition + distance), Quaternion.identity, machines);
        GameObject machine = Instantiate(sprinkleMachines[1], new Vector3(0, 0, currentPosition + distance), Quaternion.identity, machines);
        machine.transform.localScale = new Vector3(-1, 1, 1);

        currentPosition += 20;
    }

    private void RandomSprinkleMachine()
    {
        float distance = 5;

        GameObject machine = Instantiate(sprinkleMachines[(int)Random.Range(0, 2)], new Vector3(0, 0, currentPosition + distance), Quaternion.identity, machines);
        machine.transform.localScale = new Vector3(nextUpgradeMachineScale, 1, 1);
        nextUpgradeMachineScale *= -1;

        currentPosition += 10;
    }

    public void Reset()
    {
        currentPosition = 0;

        for (int i = platform.childCount -1; i >= 0; i--)
        {
            DestroyImmediate(platform.GetChild(i).gameObject);
        }

        for (int i = collectibleCandies.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(collectibleCandies.GetChild(i).gameObject);
        }

        for (int i = machines.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(machines.GetChild(i).gameObject);
        }

        for (int i = environment.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(environment.GetChild(i).gameObject);
        }
    }

}
