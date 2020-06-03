﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextContent
{
    //Contient toutes les données nécessaires pour un texte simple

    [Serializable]
    public class TextData
    {
        public string title;
        public string paratext;
        public string content;
        public string author;
        public string scene;
        public string manuscritPath;
        public string manuscritSource;
        public int minimumReadTime;

        public TextData(string title, string content, string paratext, string author, string scene, string manuscritPath = "", int minimumReadTime = 0, string manuscritSource = "")
        {
            this.paratext = paratext;
            this.title = title;
            this.content = content;
            this.author = author;
            this.scene = scene;
            this.manuscritSource = manuscritSource;
            this.minimumReadTime = minimumReadTime;

            if (manuscritPath.Contains(".png") || manuscritPath.Contains(".jpg") || manuscritPath.Contains(".jpeg"))
                this.manuscritPath = manuscritPath;
            else
                this.manuscritPath = null;
        }
    }

    public class GlossaryData
    {
        public string word;
        public string definition;
        public string imagePath;

        public GlossaryData(string word, string definition, string imagePath = null)
        {
            this.word = word;
            this.definition = definition;
            this.imagePath = imagePath;
        }
    }

    public class ImageData
    {
        public string imagePath;
        public string paratext;
        public int minimumReadTime;

        public ImageData(string imagePath, string paratext, int minReadTime)
        {
            this.imagePath = imagePath;
            this.paratext = paratext;
            this.minimumReadTime = minReadTime;
        }
    }
}
