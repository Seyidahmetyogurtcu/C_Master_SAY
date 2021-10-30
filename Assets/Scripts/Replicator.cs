using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Replicator : MonoBehaviour
{
    Text text;
    public int ReplicatorValue { get; set; }
    public string Sign { get; set; }

    private void Start()
    {
        //get random text
        int probabilityOfSign = UnityEngine.Random.Range(0, 100);
        if (probabilityOfSign < 20)// ~%20 probability
        {
            Sign = "x";
        }
        else
        {
            Sign = "+";
        }

        int randomInt = UnityEngine.Random.Range(0, 10);
        if (Sign == "x")
        {
            ReplicatorValue = (randomInt % 6);//multiplier is less then 6
        }
        else
        {
            //if Sign is Adder
            if (randomInt > 2)// ~%80 probability
            {
                ReplicatorValue = randomInt * 10;
            }
            else
            {
                ReplicatorValue = randomInt * 100;
            }
        }
        //add to text
        text = this.GetComponentInChildren<Text>();
        text.text = Sign + ReplicatorValue.ToString();
    }
}
