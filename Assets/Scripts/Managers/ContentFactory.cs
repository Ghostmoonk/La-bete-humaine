using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Factory qui crée chaque élément et va fetch son contenu en fonction de l'énumération envoyé et de l'id dans les dicos du TextsLoader.

public static class ContentFactory
{
    public static Content CreateContent(int fetchId, ContentType type)
    {
        switch (type)
        {
            case ContentType.ClosedQuestion:
                return new ClosedQuestion(TextsLoader.Instance.questionsDico[fetchId]);
            case ContentType.OpenQuestion:
                return new OpenQuestion(TextsLoader.Instance.questionsDico[fetchId]);
            case ContentType.FillGaps:
                return new FillGaps();
            case ContentType.SimpleText:
                return new SimpleText(TextsLoader.Instance.textsDico[fetchId]);
            case ContentType.SimpleImage:
                return new SimpleImage(TextsLoader.Instance.imagesDico[fetchId]);
            default:
                return null;
        }
    }
}
