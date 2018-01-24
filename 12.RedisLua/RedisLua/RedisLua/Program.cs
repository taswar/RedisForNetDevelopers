using System;
using System.Linq;
using StackExchange.Redis;

namespace RedisLua
{
    class Program
    {
        static void Main(string[] args)
        {

            const string script = @"
local function MGETSUM(keys)
    local sum = 0
    for _,key in ipairs(keys) do
        local val = redis.call('GET', key) or 0
        sum = sum + tonumber(val)
    end
    return sum
end
return MGETSUM(KEYS)";

            var redis = RedisStore.RedisCache;            

            var server = RedisStore.Server;          

            var test1 = "test1";
            var test2 = "test2";
            var test3 = "test3";

            //delete the keys
            redis.KeyDelete(test1, CommandFlags.FireAndForget);
            redis.KeyDelete(test2, CommandFlags.FireAndForget);
            redis.KeyDelete(test3, CommandFlags.FireAndForget);

            redis.StringSet(test1, 1);
            redis.StringSet(test2, 2);
            redis.StringSet(test3, 3);

            RedisKey[] keys = { test1, test2, test3 };

            byte[] hash = server.ScriptLoad(script);

            //get the sha1 hash back
            string hexHash = string.Concat(hash.Select(x => x.ToString("X2")));

            var result = redis.ScriptEvaluate(hexHash, keys);
          
            Console.WriteLine(result); //result return should be 6

            //create another script to store 
            var script2 = @"
local name=@name
local age=@age
return name..' '..age
";
            
            var prepared = LuaScript.Prepare(script2);
            //we now need to cache this loaded script somewhere
            var loaded = prepared.Load(server);

            //such that we do not need to uplaod the script every time
            var val = loaded.Evaluate(redis, new { name="taswar", age=5});

            //Output: taswar 5
            Console.WriteLine(val);


            val = loaded.Evaluate(redis, new { name = "peter", age = 25 });

            //Output: peter 25
            Console.WriteLine(val);

            Console.ReadKey();
        }
    }
}
