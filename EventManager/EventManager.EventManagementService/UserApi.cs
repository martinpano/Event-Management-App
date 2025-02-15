namespace EventManager.Api
{
    public static class UserApi
    {
        //public static RouteGroupBuilder MapUserApi(this IEndpointRouteBuilder routes)
        //{
        //    var group = routes.MapGroup("/api/v1/users");

        //    group.WithTags("User Management");

        //    // Register User
        //    group.MapPost("/register", async ([FromBody] RegisterUserDto registerDto, IUserService userService) =>
        //    {
        //        var result = await userService.RegisterUserAsync(registerDto);
        //        return Results.Ok(result);
        //    })
        //    .Produces<UserDto>(StatusCodes.Status200OK)
        //    .Produces(StatusCodes.Status400BadRequest);

        //    // Login User
        //    group.MapPost("/login", async ([FromBody] LoginDto loginDto, IUserService userService) =>
        //    {
        //        var result = await userService.LoginAsync(loginDto);
        //        return Results.Ok(result);
        //    })
        //    .Produces<AuthResponseDto>(StatusCodes.Status200OK)
        //    .Produces(StatusCodes.Status401Unauthorized);

        //    // Update User (Admin Only)
        //    group.MapPut("/{id:int}", async (int id, [FromBody] UpdateUserDto updateUserDto, IUserService userService) =>
        //    {
        //        var result = await userService.UpdateUserAsync(id, updateUserDto);
        //        return Results.Ok(result);
        //    })
        //    .RequireAuthorization(policy => policy.RequireRole("Admin"))
        //    .Produces<UserDto>(StatusCodes.Status200OK)
        //    .Produces(StatusCodes.Status404NotFound);

        //    // Delete User (Admin Only)
        //    group.MapDelete("/{id:int}", async (int id, IUserService userService) =>
        //    {
        //        var success = await userService.DeleteUserAsync(id);
        //        return success ? Results.NoContent() : Results.NotFound();
        //    })
        //    .RequireAuthorization(policy => policy.RequireRole("Admin"))
        //    .Produces(StatusCodes.Status204NoContent)
        //    .Produces(StatusCodes.Status404NotFound);

        //    return group;
        //}
    }
}
