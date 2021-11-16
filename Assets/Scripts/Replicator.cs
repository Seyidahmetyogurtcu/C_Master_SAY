using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Replicator : MonoBehaviour
{
    Text text;
    public int ReplicatorValue;
    public string Sign;
    int probabilityOfSign;
    int probabilityOfNum;
    private void Start()
    {
        probabilityOfSign = UnityEngine.Random.Range(0, 100);
        probabilityOfNum = UnityEngine.Random.Range(0, 10);
    }

    public void CalculateReplicator(int probabilityOfSign, int probabilityOfNum)
    {
        //get initial random text
        if (probabilityOfSign < 20)// ~%20 probability
        {
            Sign = "x";
        }
        else
        {
            Sign = "+";
        }

        if (Sign == "x")
        {
            ReplicatorValue = 1 + (probabilityOfNum % 5);//multiplier is less then 6
        }
        else
        {
            //if Sign is Adder
            if (probabilityOfNum > 2)// ~%80 probability
            {
                ReplicatorValue = 10 + probabilityOfNum * 10;
            }
            else
            {
                ReplicatorValue = 10 + probabilityOfNum * 100;
            }
        }
        //add to text
        text = this.GetComponentInChildren<Text>();
        text.text = Sign + ReplicatorValue.ToString();
    }
}
