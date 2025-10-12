using System;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using NUnit.Framework;

namespace EfficientDynamoDb.Tests;

public class ExceptionStuffTests
{
    [Test]
    public void CheckIoException()
    {
        var socketException = new SocketException(10054, "An existing connection was forcibly closed by the remote host.");
        var ioException = new IOException("Unable to read data from the transport connection: An existing connection was forcibly closed by the remote host..", innerException: socketException);
        var httpRequestException = new HttpRequestException("An error occurred while sending the request.", inner: ioException);
        
        try
        {
            throw httpRequestException;
        }
        catch (HttpRequestException e) when (e.InnerException is IOException or HttpIOException)
        {
            // This logs:
            // System.Net.Http.HttpRequestException: An error occurred while sending the request.
            //  ---> System.IO.IOException: Unable to read data from the transport connection: An existing connection was forcibly closed by the remote host..
            //  ---> System.Net.Sockets.SocketException (10054): An existing connection was forcibly closed by the remote host.
            //    --- End of inner exception stack trace ---
            //    --- End of inner exception stack trace ---
            Console.WriteLine(e);
            
            // And the reported error:
            // System.Net.Http.HttpRequestException: An error occurred while sending the request.
            //  ---> System.IO.IOException: Unable to read data from the transport connection: An existing connection was forcibly closed by the remote host..
            //  ---> System.Net.Sockets.SocketException (10054): An existing connection was forcibly closed by the remote host.
            //     --- End of inner exception stack trace ---
        }
    }
    
    [Test]
    public void CheckHttpIoException()
    {
        var httpIoException = new HttpIOException(HttpRequestError.ResponseEnded, "The response ended prematurely.");
        var httpRequestException = new HttpRequestException("An error occurred while sending the request.", inner: httpIoException);
        
        try
        {
            throw httpRequestException;
        }
        catch (HttpRequestException e) when (e.InnerException is IOException or HttpIOException)
        {
            // This logs:
            // System.Net.Http.HttpRequestException: An error occurred while sending the request.
            //  ---> System.Net.Http.HttpIOException: The response ended prematurely. (ResponseEnded)
            //    --- End of inner exception stack trace ---
            Console.WriteLine(e);
            
            // And the reported error:
           // System.Net.Http.HttpRequestException: An error occurred while sending the request.
           // ---> System.Net.Http.HttpIOException: The response ended prematurely. (ResponseEnded)
           //   at System.Net.Http.HttpConnection.SendAsync(HttpRequestMessage request, Boolean async, CancellationToken cancellationToken)
           //   --- End of inner exception stack trace ---
        }
    }
}