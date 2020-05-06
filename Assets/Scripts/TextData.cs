using System;
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
        public string content;
        public string author;
        public string scene;
        public string manuscritPath;
        public string manuscritSource;
        public int minimumReadTime;

        public TextData(string title, string content, string author, string scene, string manuscritPath = "", int minimumReadTime = 0, string manuscritSource = "")
        {
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

}

