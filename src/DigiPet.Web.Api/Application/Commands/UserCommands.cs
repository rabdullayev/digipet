namespace DigiPet.Web.Api.Application.Commands
{
    public class UserCommands
    {
        public class Create
        {
            public string Username { get; set; }
        }
        public class Adopt
        {
            public int Code { get; set; }
        }
        public class Stroke
        {
            public int AnimalId { get; set; }
        }
        public class Feed
        {
            public int AnimalId { get; set; }
        }
    }
}
