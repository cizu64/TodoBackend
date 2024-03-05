public record Response
{
    public object detail{get;set;}
    public int statusCode{get;set;} = StatusCodes.Status200OK;
    public object meta{get;set;}
}