using System.Text.Json;
using RPS.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;

namespace RPS.Data
{
    public class PtInMemoryContext
    {
        string resourceNameItems = "RPS.Data.GenData.fs-items.json";
        string resourceNameUsers = "RPS.Data.GenData.fs-users.json";

        List<PtItem> items;
        List<PtUser> users;

        public List<PtItem> PtItems { get { return items; } }
        public List<PtUser> PtUsers { get { return users; } }

        public PtInMemoryContext()
        {
            var assembly = Assembly.GetExecutingAssembly();

            string contentsItems = "[]";

            using (Stream stream = assembly.GetManifestResourceStream(resourceNameItems))
            using (StreamReader file = new StreamReader(stream))
            {
                contentsItems = file.ReadToEnd();
            }

            var serializationOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };

            items = JsonSerializer.Deserialize<List<PtItem>>(contentsItems, serializationOptions);
            ModifyItemDates(items);

            string contentsUsers = "[]";

            using (Stream stream = assembly.GetManifestResourceStream(resourceNameUsers))
            using (StreamReader file = new StreamReader(stream))
            {
                contentsUsers = file.ReadToEnd();
            }

            users = JsonSerializer.Deserialize<List<PtUser>>(contentsUsers, serializationOptions);
        }

        private void ModifyItemDates(List<PtItem> items)
        {
            var startDate = DateTime.Now.AddYears(-1);
            var endDate = DateTime.Now.AddDays(-1);
            var randomTest = new Random();

            TimeSpan timeSpan = endDate - startDate;


            items.ForEach(i => 
            {
                TimeSpan newSpan = new TimeSpan(0, randomTest.Next(0, (int)timeSpan.TotalMinutes), 0);
                DateTime newDate = startDate + newSpan;

                i.DateCreated = newDate;
                i.DateModified = newDate;
            });
        }
    }
}