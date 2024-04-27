namespace Main;

public class User
{
    public int NationalId { get; set; }
    public string? Password { get; set; }
    public double Balance { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public User(string? firstName, string? lastName, int nationalId, string? password)
    {
        NationalId = nationalId;
        Password = password;
        Balance = 0.0;
        FirstName = firstName;
        LastName = lastName;
    }

    public User()
    {
    }
    
    public override string ToString()
    {
        return "\n FirstName: " + FirstName + "\n  LastName: " + LastName + "\nNationalID: " + NationalId +
               "\n   Balance: " + Balance + "$\n  Password: " + Password + "\n--------\n";
    }
}