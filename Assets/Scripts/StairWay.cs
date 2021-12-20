using UnityEngine;
using Count_Master_SAY.Control;
using DG.Tweening;
using System.Collections;

namespace Count_Master_SAY.Trigger
{
    public class StairWay : MonoBehaviour
    {
        int[] cLimbOrder = new int[40];
        int maxArragementLineLength;
        int remaining;
        int maxLine;
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
            StopMoveControl();
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
            StartCoroutine(TempMethod());
        }
        IEnumerator TempMethod()
        {
            InvokeRepeating("Vibrate", 0, 0.2f);

            maxLine = maxArragementLineLength * 2 + lineOfAdditionalArragement + lineOfAdditionalMaxArragement;
            int numberofObjectPerLine = 1;
            int indexCounter = 0;
            int dubleIteration = 0;
            bool isAdded = false;
            int posY = 7;
            for (int iter = 0; iter < maxLine; iter++)
            {
                for (int i = 0; i < numberofObjectPerLine; i++)
                {
                    if (indexCounter >= playerManager.persons.Count)
                    {
                        yield break;
                    }

                    playerManager.persons[indexCounter].transform.position = new Vector3(playerManager.transform.position.x, posY, (iter - 2 * i));
                    playerManager.persons[0].transform.position = new Vector3(playerManager.transform.position.x, posY, (0));
                    playerManager.persons[indexCounter].GetComponent<Rigidbody>().isKinematic = true;
                    //playerManager.persons[indexCounter].GetComponent<Rigidbody>().velocity = new Vector3(playerManager.persons[indexCounter].GetComponent<Rigidbody>().velocity.x, 0,0);

                    //   if (indexCounter < playerManager.persons.Count - 1)
                    //  {
                    indexCounter++;
                    // }
                }
                for (int k = 0; k < indexCounter-1; k++)
                {
                    //  Sequence seq = DOTween.Sequence();
                    //   if (indexCounter == (playerManager.persons.Count - 1) && k == indexCounter - 1)
                    //   {
                    //       playerManager.persons[k].transform.position += new Vector3(0, 4, 0);
                    //       indexCounter++;
                    //  }
                    playerManager.persons[k].transform.position += new Vector3(0, 4, 0);//gives 4 unit in word point(its a courotine look why its speed is different):Answer it is working for every 35 stairs.
                                                                                        //playerManager.persons[k].transform.DOLocalMoveY(posY, 5f);
                                                                                        //playerManager.persons[k].transform.DOMoveY(playerManager.persons[k].transform.position.y + 4, 0.0f).SetDelay(0.01f);             
                                                                                        //seq.SetDelay(0.1f).Append(playerManager.persons[k].transform.DOMoveY(playerManager.persons[k].transform.position.y + 4, 0.001f));            
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

                // Vibrator.Vibrate(80);
                yield return new WaitForSeconds(0.1f);
            }
            for (int j = 0; j < lineOfAdditionalMaxArragement; j++)
            {
                playerManager.persons[indexCounter].transform.position = new Vector3(playerManager.transform.position.x, 0, maxLine - 2 * j);
                indexCounter++;
            }
            CancelInvoke("Vibrate");
        }
        void Vibrate()
        {
            Vibrator.Vibrate(80);
        }
        private void StopMoveControl()
        {
            PlayerManager.singleton.finTriggeredPlayer = true;
            PlayerManager.singleton.transform.position = new Vector3(PlayerManager.singleton.transform.position.x,
                                                                     PlayerManager.singleton.transform.position.y,
                                                                     0);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "PlayerManager")
            {
                for (int i = 0; i < maxLine + lineOfAdditionalMaxArragement; i++)
                {
                    Invoke("Vibrate", 0.3f);
                }
            }
        }
    }
}