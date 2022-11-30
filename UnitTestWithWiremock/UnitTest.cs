namespace UnitTestWithWiremock;

public class UnitTest
{
    private ClassUnderTest _sut;
    private WireMockServer _server;

    public UnitTest()
    {
        _server = WireMockServer.Start(new WireMockServerSettings
        {
            QueryParameterMultipleValueSupport = QueryParameterMultipleValueSupport.NoComma
        });
        _sut = new ClassUnderTest(_server.CreateClient());
    }

    [Fact]
    public async Task Test_send_query_without_comma()
    {
        // Arrange
        MockAPIRequest(httpMethod: HttpMethod.Get,
                       httpStatusCode: HttpStatusCode.OK,
                       path: "/queryResults",
                       @params: new KeyValuePair<string, string>(key: "query", value: ClassUnderTest.QUERY_WITHOUT_COMMA));
        // Act
        Func<Task> action = () => _sut.SendQueryRequest(ClassUnderTest.QUERY_WITHOUT_COMMA);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Test_send_query_with_comma()
    {
        // Arrange
        MockAPIRequest(httpMethod: HttpMethod.Get,
                       httpStatusCode: HttpStatusCode.OK,
                       path: "/queryResults",
                       @params: new KeyValuePair<string, string>(key: "query", value: ClassUnderTest.QUERY_WITH_COMMA));
        // Act
        Func<Task> action = () => _sut.SendQueryRequest(ClassUnderTest.QUERY_WITH_COMMA);

        // Assert
        await action.Should().NotThrowAsync();
    }

    private void MockAPIRequest(HttpMethod httpMethod,
                                HttpStatusCode httpStatusCode,
                                string? path = null,
                                params KeyValuePair<string, string>[] @params)
    {
        var request = Request.Create()
                             .UsingMethod(httpMethod.ToString())
                             .WithPath(path)
                             .WithParams(@params);

        var response = Response.Create()
                               .WithStatusCode(httpStatusCode);
        
        _server.Given(request).RespondWith(response);
    }
}

public static class ExtensionMethods
{
    public static IRequestBuilder WithParams(this IRequestBuilder builder,
                                             KeyValuePair<string, string>[] @params)
    {
        foreach (var param in @params)
        {
            builder.WithParam(key: param.Key, values: param.Value);
        }
        return builder;
    }
}