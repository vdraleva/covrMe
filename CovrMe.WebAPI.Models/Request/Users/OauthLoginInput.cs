namespace CovrMe.WebAPI.Models.Request.Users
{
    public class OauthLoginInput : RegisterUserInput
    {
        public string NameIdentifier { get; set; }
    }
}
