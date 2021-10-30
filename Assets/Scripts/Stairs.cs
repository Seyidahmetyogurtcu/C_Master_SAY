﻿using UnityEngine;
using Count_Master_SAY.Control;
namespace Count_Master_SAY.Trigger
{
    public class Stairs : MonoBehaviour
    {
        int[] cLimbOrder = new int[40];
        int maxArragementLineLength;
        int remaining;
        int lineOfAdditionalMaxArragement;
        int lineOfAdditionalArragement;
        PlayerManager playerManager; 
        private void Start()
        {
            playerManager = PlayerManager.singleton; //Initialize
            EventManager.singleton.onFinishTriggerEnter += OnFinish;        
        }

        private void OnDestroy()
        {
            EventManager.singleton.onFinishTriggerEnter -= OnFinish;
        }

        private void OnFinish()
        {
            CalculateSeperation();
            ClimbPersonSeperatly();
            MoveOnByLeaving();
            CameraCahnge();
        }
        private void CalculateSeperation()
        {
            //triangular arrengement formula
            cLimbOrder[0] = 0;
            for (int i = 0; i < 39; i++)
            {
                cLimbOrder[i + 1] = cLimbOrder[i] + 2 * (i + 1);
            }
            //find range of person count
            int midPoint = (cLimbOrder.Length / 2) - 1;//for 40=>19
            int leftLimit = 0;
            int rightLimit = cLimbOrder.Length;


            //bilinear search algorithm
            for (int i = 0; i < playerManager.persons.Count; i++)
            {
                if (playerManager.persons.Count < cLimbOrder[midPoint])
                {
                    rightLimit = midPoint;
                    midPoint = (midPoint + leftLimit) / 2;
                    if (rightLimit - leftLimit == 1)
                    {
                        //find it
                        maxArragementLineLength = leftLimit;
                        remaining = playerManager.persons.Count - cLimbOrder[leftLimit];
                        return;
                    }
                }
                else if (playerManager.persons.Count > cLimbOrder[midPoint])
                {

                    leftLimit = midPoint;
                    midPoint = (midPoint + rightLimit) / 2;
                    if (rightLimit - leftLimit == 1)
                    {
                        //find it
                        maxArragementLineLength = leftLimit;
                        remaining = playerManager.persons.Count - cLimbOrder[leftLimit];
                        return;
                    }
                }
                else if (playerManager.persons.Count == cLimbOrder[midPoint])
                {
                    //founded!
                    maxArragementLineLength = midPoint;
                    remaining = 0;
                    return;
                }
            }

        }
        private void ClimbPersonSeperatly()
        {
            if (remaining != 0)
            {
                int lineOfAdditionalMaxArragement = remaining / maxArragementLineLength;
                if (lineOfAdditionalMaxArragement != 0)
                {
                    lineOfAdditionalArragement = remaining - lineOfAdditionalMaxArragement * maxArragementLineLength;
                }
                else
                {
                    lineOfAdditionalArragement = 0;
                }
            }
            else
            {
                lineOfAdditionalMaxArragement = 0;
                lineOfAdditionalArragement = 0;
            }
        }
        private void MoveOnByLeaving()
        {
            int maxLine = maxArragementLineLength * 2 + lineOfAdditionalArragement + lineOfAdditionalMaxArragement;
            int numberofObjectPerLine = 1;
            int indexCounter = 0;
            int dubleIteration = 0;
            bool isAdded = false;
            for (int iter = 0; iter < maxLine; iter++)
            {
                for (int i = 0; i < numberofObjectPerLine; i++)
                {
                    if (indexCounter >= playerManager.persons.Count)
                    {
                        return;
                    }
                    playerManager.persons[indexCounter].transform.position = new Vector3(playerManager.transform.position.x, 7, (iter - 2 * i));
                    playerManager.persons[indexCounter].GetComponent<Rigidbody>().isKinematic = true;
                    indexCounter++;
                }
                for (int k = 0; k < indexCounter; k++)
                {
                    playerManager.persons[k].transform.position += new Vector3(0, 4, 0);

                }
                if (lineOfAdditionalArragement == iter && !isAdded)
                {
                    numberofObjectPerLine--;
                    iter--;
                    dubleIteration--;
                    isAdded = true;
                }
                if (dubleIteration % 2 == 0 && indexCounter < playerManager.persons.Count)
                {
                    numberofObjectPerLine--;
                    iter--;
                }
                numberofObjectPerLine++;
                dubleIteration++;
            }
            for (int j = 0; j < lineOfAdditionalMaxArragement; j++)
            {
                playerManager.persons[indexCounter].transform.position = new Vector3(playerManager.transform.position.x, 0, maxLine - 2 * j);
                indexCounter++;
            }
        }
        private void CameraCahnge()
        {
            Camera.main.transform.position += new Vector3(-15, 120, -30);
            Camera.main.transform.rotation = new Quaternion(Camera.main.transform.rotation.x, Camera.main.transform.rotation.y - 27, Camera.main.transform.rotation.z, Camera.main.transform.rotation.w);
            Camera.main.transform.LookAt(playerManager.persons[0].transform, Vector3.up);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag=="Player")
            {
                other.GetComponent<Rigidbody>().isKinematic = false;
                other.transform.SetParent(this.transform);
            }
        }
    }
}

