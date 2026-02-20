namespace HelloKitty.API.Domain.Users.Entities
{
    public class UserSensitve
    {
        public Guid UserId { get; private set; }
        public string? CitizenId { get; set; }
        public string? PlaceOfIssue {  get; set; }
        public DateOnly? IssueDate { get; set; }
        public User user { get; set; } = null!;
    }
}
