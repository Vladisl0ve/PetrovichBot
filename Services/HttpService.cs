using HtmlAgilityPack;
using PetrovichBot.Database;
using PetrovichBot.Extensions;
using PetrovichBot.Properties;
using Serilog;
using System.Web;

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
        public async Task<string?> GetRandomBezdna()
        {
            string apiUrl = $"https://bezdna.su/random.php";
            string? result = "";
            try
            {
                HtmlWeb web = new HtmlWeb();
                var htmlDoc = await web.LoadFromWebAsync(apiUrl);

                if (htmlDoc == null)
                {
                    Log.Warning($"Error on downloading random bezdna");
                    return default;
                }
                result = GetFromBezdnaRandJokeResponse(htmlDoc);
                //Log.Information($"Success Random Joke: {result}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"[{nameof(GetRandomJoke)}]");
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
            string jokeText = docWithJoke.DocumentNode.SelectSingleNode("//body").SelectSingleNode("//p").InnerHtml.Replace("<br>", $"{Environment.NewLine}");
            string titleText = docWithJoke.DocumentNode.SelectSingleNode("//body").SelectSingleNode("//h2").InnerHtml.Replace("<br>", $"{Environment.NewLine}");

            string result = $"<b>{titleText}</b>{Environment.NewLine}{Environment.NewLine}" +
                            $"{jokeText}";

            return result;
        }
        private static string? GetFromBezdnaRandJokeResponse(HtmlDocument docWithJoke)
        {
            var quotesCollection = docWithJoke.DocumentNode.SelectNodes("//div[@class='q']"); //[contains(@style,'border-bottom: none')]
            if (quotesCollection.Count == 0)
                return default;

            (int maxRating, int indexBestQuote, int bestQuoteID) = (0, 0, 0);
            for (int i = 0; i < quotesCollection.Count; i++)
            {
                var tmp = quotesCollection[i]
                    .SelectSingleNode("//div[@class='vote']")
                    .SelectSingleNode("//div[@style='float: left; border: none; padding: 0px; background: none;']")
                    .InnerText; //InnerText = "[ +\n82\n- ]\n\n Комментировать цитату №42757\n"

                string? quoteRatingString = tmp.GetStringBetweenTwoStrings("[ +\n", "\n- ]");
                if (int.TryParse(quoteRatingString, out int quoteRating) && quoteRating > maxRating
                    && int.TryParse(tmp.GetStringBetweenTwoStrings("№", "\n"), out int quoteID))
                {
                    maxRating = quoteRating;
                    indexBestQuote = i;
                    bestQuoteID = quoteID;
                }
            }

            var quoteText = HttpUtility.HtmlDecode(quotesCollection[indexBestQuote]
                    .SelectSingleNode("//div[@style='border-bottom: none;']")
                    .InnerHtml)
                .Replace("<br>", $"{Environment.NewLine}");

            var quoteHeader = string.Format(nameof(Resources.RandomBezdnaHeader).UseCulture("ru"), bestQuoteID) + Environment.NewLine + Environment.NewLine;
            
            var result = quoteHeader + quoteText;
            return result;
        }

        private static string GetJokeFromBaneksTopJokeAux(HtmlNode jokeNode, int randedCounter) =>
                $"{string.Format(nameof(Resources.TopJokeHeader).UseCulture("ru"), randedCounter)}" + $"{Environment.NewLine}{Environment.NewLine}" +
                            $"{jokeNode.InnerHtml}";
    }
}
