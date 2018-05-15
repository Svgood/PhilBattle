using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question {
    //first answer is correct
    public string text;
    public string ans1, ans2, ans3, ans4;

    public Question(string t, string a1, string a2, string a3, string a4)
    {
        text = t;
        ans1 = a1;
        ans2 = a2;
        ans3 = a3;
        ans4 = a4;
    }

}
