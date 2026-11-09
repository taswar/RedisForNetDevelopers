# Contributing

Thanks for helping improve the Redis for .NET Developers samples. Each numbered folder goes with one post in the [blog series](https://taswar.zeytinsoft.com/category/redis/), so the main rule is: **keep each sample matching its post.**

## Good contributions

- Bug fixes, broken builds and outdated or vulnerable NuGet packages.
- Clearer comments or README notes for a sample.
- Ports of a sample to a current .NET version, added next to the original rather than replacing it, so the blog post still matches.

For a new sample or a big restructure, open an issue first so we can agree on where it fits.

## Running the samples

1. Start Redis, for example `docker run -d --name redis -p 6379:6379 redis`.
2. Set the endpoint: `redis.connection` in `App.config` (.NET Framework) or `appsettings.json` (.NET Core), or `Redis:Hosts` for the ASP.NET Core sample.
3. Open the sample's `.sln` in Visual Studio, or run `dotnet run` in a .NET Core project folder.

## Pull requests

- Keep a pull request to one sample or one fix.
- Make sure the sample you changed builds and runs against a local Redis.
- Don't commit your own connection strings, passwords or hostnames. Use `localhost:6379` in config files.
- Don't commit `bin/`, `obj/` or `packages/` folders.
- Say in the description which sample you changed and how you tested it.

## Security

To report a vulnerability, use [private vulnerability reporting](https://github.com/taswar/RedisForNetDevelopers/security/advisories/new) instead of a public issue.

## License

By contributing, you agree that your contributions are licensed under the [MIT License](LICENSE).
