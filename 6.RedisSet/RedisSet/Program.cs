using StackExchange.Redis;
using System;
using System.Linq;

namespace RedisSet
{
    class Program
    {
        static void Main(string[] args)
        {
            var redis = RedisStore.RedisCache;

            RedisKey key = "setKey";
            RedisKey alphaKey = "alphaKey";
            RedisKey numKey = "numberKey";
            RedisKey destinationKey = "destKey";


            redis.KeyDelete(key, CommandFlags.FireAndForget);
            redis.KeyDelete(alphaKey, CommandFlags.FireAndForget);
            redis.KeyDelete(numKey, CommandFlags.FireAndForget);
            redis.KeyDelete(destinationKey, CommandFlags.FireAndForget);


            //add 10 items to the set
            for (int i = 1; i <= 10; i++)
                redis.SetAdd(key, i);

            var members = redis.SetMembers(key);

            Console.WriteLine(string.Join(",", members)); //output 1,2,3,4,5,6,7,8,9,10
                        
            //remove 5th element
            redis.SetRemove(key, 5);

            Console.WriteLine(redis.SetContains(key, 5)); //False

            members = redis.SetMembers(key);

            Console.WriteLine(string.Join(",", members)); //output 1,2,3,4,6,7,8,9,10
            
            Console.WriteLine(redis.SetContains(key, 10)); //True

            //number of elements
            Console.WriteLine(redis.SetLength(key)); //output 9
           
            
            //add alphabets to set
            redis.SetAdd(alphaKey, "abc".Select(x => (RedisValue) x.ToString()).ToArray());
            redis.SetAdd(numKey, "123".Select(x => (RedisValue)x.ToString()).ToArray());

            var values = redis.SetCombine(SetOperation.Union, numKey, alphaKey);

            Console.WriteLine(string.Join(",", values)); //unordered list of items (e.g output can be "1,3,2,a,c,b")
            
            values = redis.SetCombine(SetOperation.Difference, key, numKey);

            Console.WriteLine(string.Join(",", values)); //4, 6, 7, 8, 9, 10

            values = redis.SetCombine(SetOperation.Intersect, key, numKey);

            Console.WriteLine(string.Join(",", values)); //1, 2, 3
         
            //move a random from source numKey to aplhaKey 
            redis.SetMove(numKey, alphaKey, 2);

            members = redis.SetMembers(alphaKey);

            Console.WriteLine(string.Join(",", members)); //output can be (b, c, a, 2)

            //Add apple to 
            redis.SetAdd(alphaKey, "apple");

            //look for item that starts with he
            var patternMatchValues = redis.SetScan(alphaKey, "ap*"); //output apple

            Console.WriteLine(string.Join(",", patternMatchValues));

            patternMatchValues = redis.SetScan(alphaKey, "a*"); //output apple, a

            Console.WriteLine(string.Join(",", patternMatchValues));
            

            //store into destinantion key the union of numKey and alphaKey
            redis.SetCombineAndStore(SetOperation.Union, destinationKey, numKey, alphaKey); 

            Console.WriteLine(string.Join(",", redis.SetMembers(destinationKey)));

            //radmon value and removes it from the set
            var randomVal = redis.SetPop(numKey);

            Console.WriteLine(randomVal); //random member in the set

            Console.ReadKey();
        }
    }
}
