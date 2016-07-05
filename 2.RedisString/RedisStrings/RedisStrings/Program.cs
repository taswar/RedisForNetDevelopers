using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace RedisStrings
{
    class Program
    {
        static void Main(string[] args)
        {
            var redis = RedisStore.RedisCache;
            var key = "testKey";

            if (redis.StringSet(key, "testValue"))
            {
                var val = redis.StringGet(key);

                //output - StringGet(testKey) value is testValue
                Console.WriteLine("StringGet({0}) value is {1}", key, val);

                var v1 = redis.StringGetSet(key, "testValue2");

                //output - StringGetSet(testKey) testValue == testValue
                Console.WriteLine("StringGetSet({0}) {1} == {2}", key, val, v1);

                val = redis.StringGet(key);

                //output - StringGet(testKey) value is testValue2
                Console.WriteLine("StringGet({0}) value is {1}", key, val);

                //using SETNX 
                //code never goes into if since key already exist
                if (redis.StringSet(key, "someValue", TimeSpan.MaxValue, When.NotExists))
                {
                    val = redis.StringGet(key);
                    Console.WriteLine("StringGet({0}) value is {1}", key, val);
                }
                else
                {
                    //always goes here
                    Console.WriteLine("Value already exist");
                }

                var key2 = key + "1";
                if (redis.StringSet(key2, "someValue", TimeSpan.MaxValue, When.NotExists))
                {
                    val = redis.StringGet(key2);
                    //output - StringGet(testKey2) value is someValue", key2, val
                    Console.WriteLine("StringGet({0}) value is {1}", key2, val);
                }
                else
                {
                    //never goes here
                    Console.WriteLine("Value already exist");
                }
            }

            //mset and msetnx
            KeyValuePair<RedisKey, RedisValue>[] values =
            {
                new KeyValuePair<RedisKey, RedisValue>("a", "x"),
                new KeyValuePair<RedisKey, RedisValue>("b", "y")
            };

            if (redis.StringSet(values))
            {
                RedisKey[] myKeys = { "b", "a" };
                var allValues = redis.StringGet(myKeys);

                //output - y,x
                Console.WriteLine(string.Join(",", allValues));
            }

            //serialization of object
            var u = new User { Name = "taswar", Twitter = "@taswarbhatti" };

            var userKey = "user:taswar";
            var serializedUser = JsonConvert.SerializeObject(u);
            redis.StringSet(userKey, serializedUser);

            var deserializeObject = JsonConvert.DeserializeObject<User>(redis.StringGet(userKey));

            //output Name = taswar, Twitter = @taswarbhatti
            Console.WriteLine("Name = {0}, Twitter = {1}", deserializeObject.Name, deserializeObject.Twitter);

            Console.ReadKey();
        }
    }

    internal class User
    {
        public string Name { get; set; }
        public string Twitter { get; set; }
    }
}
