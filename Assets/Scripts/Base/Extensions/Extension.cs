using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public static class Extension
{
    public static string Implode<T>(this IEnumerable<T> someList, string etc = "and", string divisor = ",")
    {
        string ret = string.Empty;
        int listLength = someList.Count(); 

        for (int i = 0; i < listLength; i++)
        {
            ret += someList.ElementAt(i);

            if (i + 2 == listLength)
                ret += $" {etc} ";

            if (i + 2 < listLength)
                ret += $"{divisor} ";
        }

        return ret;
    }
}
