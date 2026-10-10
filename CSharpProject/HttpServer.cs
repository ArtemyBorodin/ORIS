using System.Net;
using System.Text;

class HttpServer
{
    private readonly HttpListener server = new();

    private readonly string[] prefixes;

    public HttpServer(string[] prefixes)
    {
        this.prefixes = prefixes;
    }
    public async Task StartServer()
    {
        foreach (var prefix in prefixes)
            server.Prefixes.Add(prefix);
        server.Start();

        while (true)
        {
            HttpListenerContext context;
            // возвращает данные запроса и инструмент для ответа 
            try
            {
                context = await server.GetContextAsync();
            }
            catch (HttpListenerException)
            {
                break;
            }
            catch (ObjectDisposedException)
            {
                break;
            }

            // кладём в response инструмент для ответа
            var response = context.Response;

            string responseText =
            @"<!DOCTYPE html>
            <html>
                <head>
                    <meta charset='utf8'>
                    <title>METANIT.com</title>    
                </head>
                <body>
                    <h2>Hello METANIT.COM</h2>
                <body>
            </html>";
            byte[] buffer = Encoding.UTF8.GetBytes(responseText);

            response.ContentLength64 = buffer.Length;

            using Stream output = response.OutputStream;

            await output.WriteAsync(buffer);
            await output.FlushAsync();
        }
    }

    public void StopServer()
    {
        server.Stop();
    }
}

