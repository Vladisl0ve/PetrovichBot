using HtmlAgilityPack;
using PetrovichBot.Database;
using PetrovichBot.Extensions;
using PetrovichBot.Properties;
using Serilog;

namespace PetrovichBot.Services
{
    public class HttpService
    {
        public HttpService(IEnvsSettings envs)
        {
            _envs = envs;
        }

        private IEnvsSettings _envs { get; }

        public async Task<string?> GetTopJoke()
        {
            string apiUrl = $"https://baneks.ru/top";
            string? result = "";
            try
            {
                HtmlWeb web = new HtmlWeb();
                var htmlDoc = await web.LoadFromWebAsync(apiUrl);

                if (htmlDoc == null)
                {
                    Log.Warning($"Error on downloading random joke");
                    return default;
                }
                result = GetJokeFromBaneksTopJokeResponse(htmlDoc);
                //Log.Information($"Success Random Joke: {result}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"[{nameof(GetTopJoke)}]");
                result = default;
            }

            return result;
        }
        public async Task<string?> GetRandomJoke()
        {
            string apiUrl = $"https://baneks.ru/random";
            string? result = "";
            try
            {
                HtmlWeb web = new HtmlWeb();
                var htmlDoc = await web.LoadFromWebAsync(apiUrl);

                if (htmlDoc == null)
                {
                    Log.Warning($"Error on downloading random joke");
                    return default;
                }
                result = GetJokeFromBaneksRandJokeResponse(htmlDoc);
                //Log.Information($"Success Random Joke: {result}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"[{nameof(GetRandomJoke)}]");
                result = default;
            }

            return result;
        }

        private string GetJokeFromBaneksTopJokeResponse(HtmlDocument docWithJoke)
        {
            var nodesWithJokes = docWithJoke.DocumentNode.SelectSingleNode("//body").SelectNodes("//a[@class='golden-anek']");
            var randomCount = new Random().Next(0, nodesWithJokes.Count);
            var nodeWithResultJoke = nodesWithJokes[randomCount];

            string jokeText = GetJokeFromBaneksTopJokeAux(nodeWithResultJoke, randomCount);
            return jokeText;
        }
        private static string GetJokeFromBaneksRandJokeResponse(HtmlDocument docWithJoke)
        {
            string jokeText = docWithJoke.DocumentNode.SelectSingleNode("//body").SelectSingleNode("//p").InnerHtml.Replace("<br>", $"");
            string titleText = docWithJoke.DocumentNode.SelectSingleNode("//body").SelectSingleNode("//h2").InnerHtml.Replace("<br>", $"");

            string result = $"<b>{titleText}</b>{Environment.NewLine}{Environment.NewLine}" +
                            $"{jokeText}";

            return result;
        }

        private static string GetJokeFromBaneksTopJokeAux(HtmlNode jokeNode, int randedCounter) =>
                $"{string.Format(nameof(Resources.TopJokeHeader).UseCulture("ru"), randedCounter)}" + $"{Environment.NewLine}{Environment.NewLine}" +
                            $"{jokeNode.InnerHtml}";
    }
}
