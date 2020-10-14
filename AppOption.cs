namespace TestNikita
{
  public static class AppOption
  {
    public const string DB_USERS = "Users";

    public const string DB_TRANSFERS = "Transfers";

    public const string ADMIN_ROLE = "admin";

    public const string USER_ROLE = "user";

    public const string COMMON_METHOD = "admin, user";

    public static string CreateStringConnection(string db_name)
    {
      return $"mongodb+srv://nikita:nikita@cluster0.xczbr.mongodb.net/{db_name}?retryWrites=true&w=majority";
    }
  }
}