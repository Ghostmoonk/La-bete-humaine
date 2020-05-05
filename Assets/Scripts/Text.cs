using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextContent
{
    public class Text
    {
        public string title;
        public string content;
        public string author;
        public string scene;
        public string source;

        public Text(string title, string content, string author, string scene, string source = "")
        {
            this.title = title;
            this.content = content;
            this.author = author;
            this.scene = scene;
            this.source = source;
        }
    }

}

