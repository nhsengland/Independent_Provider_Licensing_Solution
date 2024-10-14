﻿namespace Domain.Objects.ViewModels.YourProfile;

public record ProfileViewModel
{
    public string OktaUrl { get; set; } = default!;

    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;

    public string Email { get; init; } = default!;
}
