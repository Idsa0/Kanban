namespace IntroSE.Kanban.Backend.ServiceLayer;

public class Response
{
    public string ErrorMessage { get; set; }

    public object ReturnValue { get; set; }

    public Response(string msg, object res)
    {
        ErrorMessage = msg;
        ReturnValue = res;
    }

    public Response()
    {
        //ErrorMessage = null;
        //ReturnValue = null;
    }

    public Response(object res)
    {
        ReturnValue = res;
    }

    public Response(object res, int i)
    {
        ReturnValue = res;
    }

    public Response(string msg)
    {
        ErrorMessage = msg;
    }
}