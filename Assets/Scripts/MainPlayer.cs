using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MainPlayer : MonoBehaviour
{
    public string PlayerName = "The player";

    class CompareTest
    {
        public readonly int val;

        public CompareTest(int val)
        {
            this.val = val;
        }

        public override bool Equals(object obj)
        {
            return obj is CompareTest test && this.val == test.val;
        }

        public override int GetHashCode()
        {
            return val;
        }

        public static bool operator == (CompareTest a, object b)
        {
            return Equals(a, b);
        }

        public static bool operator != (CompareTest a, object b)
        {
            return !(a == b);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CompareTest a = new CompareTest(1);
        object b = new CompareTest(1);
        CompareTest c = new CompareTest(1);
        Debug.Log("AB " + (a == b)); // t
        Debug.Log("BA " + (b == a)); // f!
        Debug.Log("AC " + (a == c)); // t
        Debug.Log("CA " + (c == a)); // t 

        Debug.Log("Hello " + PlayerName);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Hello " + PlayerName);
    }
}
