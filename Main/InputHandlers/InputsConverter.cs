namespace Main;

public class InputsConverter
{
    public static List<User>? ToNonNullable(int? userId, string userPassword)
    {
        int userIdNonNullable = Convert.ToInt32(Convert.ToString(userId));
        string userPasswordNonNullable = Convert.ToString(userPassword);

        List<User>? currentUserList = new List<User>();
        currentUserList.Clear();
        User currentUser = new User(userIdNonNullable, userPasswordNonNullable);

        currentUserList.Add(currentUser);

        return currentUserList;
    }

    public static string ToLower(string line)
    {
        return line.ToLower();
    }
}