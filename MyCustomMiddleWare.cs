using System.Diagnostics;
using System.Text.Json;

namespace Ass_4;
public class MyCustomMiddleWare
{
    private readonly RequestDelegate _next;

    public MyCustomMiddleWare(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        //headers
        var headers = new Dictionary<string , string>() ;
        foreach(var item in context.Request.Headers){
                headers.Add(item.Key , item.Value.ToString());
        }

        var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();

        var scheme = context.Request.Scheme;
        var host = context.Request.Host;
        var path = context.Request.Path;
        var queryString = context.Request.QueryString;


        var requestData = new {
            Scheme = context.Request.Scheme,
            Host = context.Request.Host.ToString(),
            Path = context.Request.Path.ToString(),
            QueryString = context.Request.QueryString.ToString(),
            Body = body,
            Headers = headers
        };

        
        // var writer = File.AppendText("file.txt");
        // var data = JsonSerializer.Serialize(requestData);
        // await writer.WriteAsync(data);

        
        using (StreamWriter writer = File.AppendText("file.txt")){
            var data = JsonSerializer.Serialize(requestData);
            await writer.WriteAsync(data);
        }
       
            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        
    }
}

public static class MyCustomMiddleWareExtensions
{
    public static IApplicationBuilder UseMyCustomMiddleWare(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MyCustomMiddleWare>();
    }
}
