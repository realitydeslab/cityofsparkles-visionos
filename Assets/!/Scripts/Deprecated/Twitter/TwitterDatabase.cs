using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System;
using System.IO;

public class TwitterDatabase : MonoBehaviour
{
    public static TwitterDatabase Instance { get { return s_Instance; } }

    private static TwitterDatabase s_Instance;

    public class DBTweet
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public string clean_text { get; set; }
        public string full_text { get; set; }

        public double latitude { get; set; }
        public double longitude { get; set; }

        public double sentiment_positive { get; set; }
        public double sentiment_negative { get; set; }
        public double sentiment_polarity { get; set; }

        public string tag { get; set; }
        public DateTime last_access { get; set; }

        public string username { get; set; }
        public string full_username { get; set; }
        public string created_at { get; set; }

        //public override string ToString()
        //{
        //    return string.Format("[{0:0.00}, {1:0.00}] {2}", SentimentPositive, SentimentNeagtive, CleanText);
        //}

        public override string ToString()
        {
            return $"id: {id}\n" +
                   $"clean_text: {clean_text}\n" +
                   $"full_text: {full_text}\n" +
                   $"latitude: {latitude}\n" +
                   $"longitude: {longitude}\n" +
                   $"sentiment_positive: {sentiment_positive}\n" +
                   $"sentiment_negative: {sentiment_negative}\n" +
                   $"sentiment_polarity: {sentiment_polarity}\n" +
                   $"tag: {tag}\n" +
                   $"last_access: {last_access}\n" +
                   $"username: {username}\n" +
                   $"full_username: {full_username}\n" +
                   $"created_at: {created_at}";
        }

        public bool IsDummy
        {
            get { return id < 0; }
        }

        private static int s_EmptyPlaceholderIdCount = -1;

        public static DBTweet EmptyPlaceholder()
        {
            DBTweet result = new DBTweet
            {
                id = s_EmptyPlaceholderIdCount,
                clean_text = "",
                last_access = DateTime.UtcNow,
                created_at = DateTime.UtcNow.ToLongTimeString()
            };
            s_EmptyPlaceholderIdCount--;
            return result;
        }
    }

    public string Database = "twitter_sf.db";

    private SQLiteConnection m_DbConnection;

    private void Awake()
    {
        if (s_Instance != null && s_Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            s_Instance = this;
        }
    }

    private void CheckConnection()
    {
        if (m_DbConnection == null)
        {
            Debug.Log("Connecting to database");
            string dbPath = Application.persistentDataPath + "/" + Database;
            if (!File.Exists(dbPath))
            {
                string sourcePath = Application.streamingAssetsPath + "/" + Database;
                File.Copy(sourcePath, dbPath);
            }
            m_DbConnection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create, true);
            //m_DbConnection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadOnly, true);
        }
    }

    public DBTweet QueryForRandomTweet()
    {
        string query = "SELECT username, full_username, full_text, created_at, latitude, longitude FROM tweets_random ORDER BY RANDOM() LIMIT 1";
        return m_DbConnection.Query<DBTweet>(query)[0];
    }

    public DBTweet QueryOne()
    {
        CheckConnection();
        string query = "SELECT * FROM tweets WHERE sentiment_negative > 0.5 ORDER BY RANDOM() LIMIT 1";
        List<DBTweet> results = m_DbConnection.Query<DBTweet>(query);
        RecordLastAccessTime(results);
        return results.Count == 0 ? null : results[0];
    }

    public IList<DBTweet> QueryForTags(string tag, int limit)
    {
        CheckConnection();
        string query = "SELECT * FROM tags ta INNER JOIN tweets tw ON ta.id = tw.id WHERE ta.tag = ? ORDER BY last_access LIMIT ?";
        List<DBTweet> result = m_DbConnection.Query<DBTweet>(query, tag, limit);
        RecordLastAccessTime(result);

        return result;
    }

    public void RecordLastAccessTime(string[] ids)
    {
        CheckConnection();

        string query = string.Format("UPDATE tweets SET last_access = ? WHERE id IN ({0})", string.Join(", ", ids));
        m_DbConnection.Execute(query, DateTime.UtcNow);
    }

    public void RecordLastAccessTime(IList<DBTweet> tweets)
    {
        string[] ids = new string[tweets.Count];
        for (int i = 0; i < tweets.Count; i++)
        {
            ids[i] = tweets[i].id.ToString();
        }

        RecordLastAccessTime(ids);
    }
}
