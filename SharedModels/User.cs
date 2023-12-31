namespace SharedModels
{
    public class User
    {

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Cpr { get; set; }
        public string Name { get; set; }

        public double Balance { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Email: {Email}, Password: {Password}, Cpr: {Cpr}, Name: {Name}, Balance: {Balance}";
        }
    }
}
