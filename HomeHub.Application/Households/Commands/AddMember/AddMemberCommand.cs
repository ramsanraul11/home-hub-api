namespace HomeHub.Application.Households.Commands.AddMember
{
    public sealed record AddMemberCommand(string Email, HouseholdRole Role);

    public sealed record MemberDto(Guid MemberId, Guid UserId, string Email, HouseholdRole Role);
}
