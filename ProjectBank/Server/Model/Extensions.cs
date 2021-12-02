
namespace ProjectBank.Server.Model;

public static class Extensions
{
    public static IActionResult ToActionResult(this ActionResponse response) => response switch
    {
        Updated => new NoContentResult(),
        Deleted => new NoContentResult(),
        NotFound => new NotFoundResult(),
        Conflict => new ConflictResult(),
        _ => throw new NotSupportedException($"{response} not supported")
    };

    public static ActionResult<T> ToActionResult<T>(this Option<T> option) where T : class
        => option.IsSome ? option.Value : new NotFoundResult();
}