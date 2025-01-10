namespace E_Commerce_Backend.Model
{
    public class UserExistResultDto
    {
        public bool EmailExists { get; set; }
        public bool PhonenumberExists { get; set; }
        public bool AnyExists => EmailExists || PhonenumberExists;
    }
}
