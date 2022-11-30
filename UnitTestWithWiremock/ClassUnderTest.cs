namespace UnitTestWithWiremock;

public class ClassUnderTest
{
    private readonly HttpClient _httpClient;

    public ClassUnderTest(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SendQueryRequest(string query)
    {
        var uri      = GetUri(query);
        var response = await _httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();
    }

    private static Uri GetUri(string query)
    {
        var resource    = "queryResults";
        var querystring = $"query={query}";
        return new Uri($"{resource}?{querystring}", UriKind.Relative);
    }

    public const string QUERY_WITHOUT_COMMA 
        = "select id " +
          "from incidents " +
          "where id=10";

    public const string QUERY_WITH_COMMA
        = "select id, subject " +
          "from incidents " +
          "where id=10";

}
