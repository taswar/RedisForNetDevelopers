using System;
using System.Collections.Generic;
using OttawaOpenDataExtractor;
using RedisLGeo;
using StackExchange.Redis;

namespace RediusGeo
{
    class Program
    {
        static void Main(string[] args)
        {
            var redis = RedisStore.RedisCache;

            var list = new List<GeoEntry>();

            //BasetBallExtractor is just a class that calls Ottawa OpenData
            foreach (var item in BasketBallExtractor.Extract())
            {
                var longtitue = Convert.ToDouble(item.Item1);
                var latitude = Convert.ToDouble(item.Item2);
                var name = item.Item3;
                list.Add(new GeoEntry(longtitue, latitude, name));
            }
            
            var key = "locations";

            //clear out the data
            redis.KeyDelete(key, CommandFlags.FireAndForget);

            //add all the geoentry
            redis.GeoAdd(key, list.ToArray());

            //find the distance between parks
            var val = redis.GeoDistance(key, "Gesner Court Park", "Walter Baker Park", GeoUnit.Kilometers);
            
            Console.WriteLine($"Distance from Gesner Court to Walter Baker Park is {val} km");
            //ouput - Distance from Gesner Court to Walter Baker Park is 0.4377 km
            
            val = redis.GeoDistance(key, "Brewer Park", "Walter Baker Park", GeoUnit.Kilometers);

            Console.WriteLine($"Distance from Brewer Park to Walter Baker Park is {val} km");
            //output - Distance from Brewer Park to Walter Baker Park is 19.6528 km

            //get the position of Walter Baker Park
            var pos = redis.GeoPosition(key, "Walter Baker Park");

            Console.WriteLine($"Walter Baker Park is located at: Lat {pos.Value.Latitude}, Long {pos.Value.Longitude} ");
            //Walter Baker Park is located at: Lat 45.2947688017077, Long - 75.902011692524            

            //assume we are at walter baker and the place is closed
            //we want to find out what is close by within 5 km for us to play basketball and find us the top 10 closes place
            var results = redis.GeoRadius(key, "Walter Baker Park", 5, GeoUnit.Kilometers, 10, Order.Ascending, GeoRadiusOptions.WithDistance);

            foreach (var geoRadiusResult in results)
            {
                Console.WriteLine($"- {geoRadiusResult.Member},  Distance: {geoRadiusResult.Distance}");
            }

            /*
             * - Walter Baker Park,  Distance: 0
             * - Gesner Court Park,  Distance: 0.4377
             * - Stonegate Park,  Distance: 0.9454
             * - Fringewood Park and Community Centre,  Distance: 1.8288
             * - Clarence Maheral Park,  Distance: 1.9128
             * - Cattail Creek Park,  Distance: 2.0176
             * - Amberway Park,  Distance: 2.4079
             * - Ed Hollyer Park,  Distance: 2.5855
             * - Bryanston Gate Park,  Distance: 2.7199
             * - Jim Malone Park,  Distance: 2.9452
             */

            //lets say we are a Ottawa Parliment Hill and want to play basketball where can we go
            var parliamentHillLatitude = 45.424807;
            var parliamentHillLongtitude = -75.699234;
            
            //find me all the locations nearby
            results = redis.GeoRadius(key, parliamentHillLongtitude, parliamentHillLatitude, 3, GeoUnit.Kilometers, -1, Order.Ascending, GeoRadiusOptions.WithCoordinates);
            foreach (var geoRadiusResult in results)
            {
                Console.WriteLine($"- {geoRadiusResult.Member}, Position {geoRadiusResult.Position}");
            }

            /*
             * - Cathcart Park, Position -75.6929334998131 45.4346575277704
             * - St. Luke's Park, Position -75.6867161393166 45.4152390289687
             * - Bordeleau Park, Position -75.6928798556328 45.4373215197089
             * - New Edinburgh Park, Position -75.6871077418327 45.4377904431234
             * - Dalhousie Community Centre, Position -75.7093647122383 45.4104889615161
             * - Sandy Hill Park and Community Centre, Position -75.6765183806419 45.4219332275505
             * - Chaudière Park, Position -75.7137742638588 45.409835003457
             * - Plouffe Park, Position -75.7150831818581 45.4074777127788
             * - Dutchie's Hole Park, Position -75.6693086028099 45.4209015960387
             * - Riverain Park, Position -75.6697806715965 45.4313877374749
             * - Springhurst Park, Position -75.6728920340538 45.4127347244633
             * - Sylvia Holden Park, Position -75.6818452477455 45.4029101452497
             * - Lindenlea Park, Position -75.6763252615929 45.4449763776101
             * - Ev Tremblay Park, Position -75.7115051150322 45.3998811534643
             * - Laroche Park, Position -75.7291594147682 45.408689309493
             */
               
            //we can get back the hash of the geo location
            //The command returns 11 characters Geohash strings, so no precision is loss compared to the Redis internal 52 bit representation
            var hash = redis.GeoHash(key, "Walter Baker Park");

            //we can now use http://geohash.org/<geohash-string>
            Console.WriteLine($"Walter Baker Park geo hash is {hash}");
            //output Walter Baker Park geo hash is f2418v9v4u0
            //this will also work http://geohash.org/f2418v9v4u0

            Console.ReadKey();
        }
    }
}
