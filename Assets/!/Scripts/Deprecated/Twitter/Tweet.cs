using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;

public class Tweet
{
    public long Id;

    public string Text;

    public string CleanText;

    public string[] Words;

    public Tweet(TwitterDatabase.DBTweet dbTweet)
    {
        Id = dbTweet.id;

        Text = dbTweet.clean_text;
        CleanText = dbTweet.clean_text;

        // Words
        string stripped = Regex.Replace(CleanText, @"[^\u0000-\u007F]+", string.Empty);
        stripped = Regex.Replace(stripped, @",(\S)", @", $1");

        List<string> wordsList = new List<string>(stripped.Split(' '));
        wordsList.Add("- @" + dbTweet.username + ".");

        DateTime createdAt = DateTime.Parse(dbTweet.created_at);
        wordsList.Add(createdAt.ToString("htt, MMM d, yyyy"));
        Words = wordsList.ToArray();
    }
}
