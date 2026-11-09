# Redis for .NET Developers

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

Sample C# code for the **Redis for .NET Developers** blog series on [taswar.zeytinsoft.com](http://taswar.zeytinsoft.com). Each folder is a small, self-contained project covering one Redis feature, using the [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis) client.

## Samples

| # | Topic | Project | Blog post |
|---|-------|---------|-----------|
| 1 | Connecting to Redis (.NET Framework and .NET Core) | [1.RedisConnection](1.RedisConnection) | [Connecting with C#](http://taswar.zeytinsoft.com/2015/11/03/redis-for-net-developer-connecting-with-c/) |
| 2 | String data type | [2.RedisString](2.RedisString) | [String data type](http://taswar.zeytinsoft.com/2015/11/05/redis-for-net-developer-string-datatype/) |
| 3 | Strings as integers | [3.RedisStringsAsInt](3.RedisStringsAsInt) | [String data type, part 2](http://taswar.zeytinsoft.com/2016/02/26/redis-net-developer-redis-string-datatype-part-2/) |
| 4 | Hash data type | [4.RedisHashes](4.RedisHashes) | [Hash data type](http://taswar.zeytinsoft.com/2016/03/03/redis-hash-datatype/) |
| 5 | List data type | [5.RedisList](5.RedisList) | [List data type](http://taswar.zeytinsoft.com/2016/07/21/redis-list/) |
| 6 | Set data type | [6.RedisSet](6.RedisSet) | [Set data type](http://taswar.zeytinsoft.com/2016/12/28/redis-sets-datatype/) |
| 7 | Sorted set data type | [7.RedisSortedSets](7.RedisSortedSets) | [Sorted set data type](http://taswar.zeytinsoft.com/2016/12/29/redis-sorted-sets-datatype/) |
| 8 | HyperLogLog | [8.RedisHyperLogLog](8.RedisHyperLogLog) | [HyperLogLog](http://taswar.zeytinsoft.com/2017/01/12/redis-hyperloglog/) |
| 9 | Pub/Sub | [9.RedisPubSub](9.RedisPubSub) | [Pub/Sub](http://taswar.zeytinsoft.com/redis-pub-sub/) |
| 10 | Pipelining and batching | [10.RedisPipeline](10.RedisPipeline) | [Pipeline and batching](http://taswar.zeytinsoft.com/redis-pipeline-batching/) |
| 11 | Transactions | [11.RedisTransaction](11.RedisTransaction) | [Transactions](http://taswar.zeytinsoft.com/redis-transactions) |
| 12 | Lua scripting | [12.RedisLua](12.RedisLua) | — |
| 13 | Geospatial commands (with Ottawa open data) | [13.RedisGeo](13.RedisGeo) | — |
| 14 | ASP.NET Core Web API with Redis | [14.RedisAspDotNetCore](14.RedisAspDotNetCore) | — |

## Prerequisites

- A running Redis server. The quickest way is Docker:

  ```bash
  docker run -d --name redis -p 6379:6379 redis
  ```

- **Samples 1–13 (.NET Framework):** Visual Studio with .NET Framework 4.5.2–4.6.2. NuGet packages restore on build.
- **.NET Core samples** (`1.RedisConnection/RedisConnectionDotNetCore`, `14.RedisAspDotNetCore`): the .NET SDK. These target `netcoreapp2.2`, so you may need to change `TargetFramework` in the `.csproj` to a version you have installed.

## Configuring the Redis connection

The samples were written against Redis running in Docker on different ports, so check the endpoint before you run one:

| Project type | Where to set it | Key |
|--------------|-----------------|-----|
| .NET Framework console apps | `App.config` | `redis.connection` (for example `localhost:6379`) |
| .NET Core console app | `appsettings.json` | `redis.connection` |
| ASP.NET Core Web API | `appsettings.json` | `Redis:Hosts` |

## Running a sample

**.NET Framework:** open the `.sln` file in the sample's folder in Visual Studio, set the endpoint in `App.config`, and press F5.

**.NET Core:**

```bash
cd 1.RedisConnection/RedisConnectionDotNetCore
dotnet run
```

```bash
cd 14.RedisAspDotNetCore/RedisAspDotNetCoreWebApiSample
dotnet run
```

The Web API exposes its endpoints under `api/redis`; see [RedisController.cs](14.RedisAspDotNetCore/RedisAspDotNetCoreWebApiSample/Controllers/RedisController.cs).

## License

[MIT](LICENSE) © Taswar Bhatti
