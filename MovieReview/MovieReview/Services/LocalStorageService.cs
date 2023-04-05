using LiteDB;
using MovieReview.Model;
using MovieReview.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace MovieReview.Services
{
    [ExcludeFromCodeCoverage]
    public class LocalStorageService : ILocalStorageService
    {
        private readonly string _databaseName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "moviereview.db");
        private LiteDatabase _database;

        public LocalStorageService()
        {
            BsonMapper.Global.Entity<Content>().Id(a => a.ImdbID);
            BsonMapper.Global.Entity<FullContent>().Id(i => i.ImdbID);
            _database = new LiteDatabase(_databaseName);
        }
        public int InsertList<T>(List<T> items)
        {
            var collection = _database.GetCollection<T>();
            if (items != null && items.Count > 0)
            {
                collection.Upsert(items);
                return 1;
            }
            else
            {
                throw new Exception("No items to Insert");
            }
        }
        public int InsertItem<T>(T item)
        {
            var collection = _database.GetCollection<T>();
            if (item != null)
            {
                collection.Upsert(item);
                return 1;
            }
            else
            {
                throw new Exception("No items to Insert");
            }
        }

        public List<T> GetItemList<T>()
        {
            var itemCollection = _database.GetCollection<T>().FindAll();
            if (itemCollection != null)
            {
                List<T> contents = new List<T>();
                contents.AddRange(itemCollection);
                return contents;
            }
            else
            {
                throw new Exception("No Data found");
            }
        }
        public FullContent GetItem(string id)
        {
            var item = _database.GetCollection<FullContent>().FindById(id);
            if (item != null)
                return item;
            else
            {
                throw new Exception("Please make sure your device is connected to the Internet!");
            }
        }
    }
}
