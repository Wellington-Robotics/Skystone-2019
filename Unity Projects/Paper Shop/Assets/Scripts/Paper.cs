using System;
using System.Collections.Generic;

namespace Application
{
    public class Paper
    {
        Dictionary<string, string> paperTypeNums = new Dictionary<string, string>
        {
            { "printerPaper", "Printer Paper" },
            { "kami", "Kami" },
            { "kraft", "Kraft" },
            { "tant", "Tant" },
            { "japaneseFoil", "Japanese Foil" },
            { "washi", "Washi" },
            { "elephantHide", "Elephant Hide" },
            { "glassine", "Glassine" },
            { "lokta", "Lokta" },
            { "ogami", "O-Gami" },
            { "origamido", "Origamido" }
        };

        string name;
        int number;
        int basePrice;
        double currentPrice;
        string image;

        void Init(string t, int n, int p, string i)
        {
            name = paperTypeNums[t];
            number = n;
            basePrice = p;
            currentPrice = basePrice;
            image = i;
        }

        void Buy(int num)
        {
            currentPrice = currentPrice * 1.05;
            number += num;
        }
    }
}
