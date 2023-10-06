using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public int user_id;
    public string username;
    public string email;
    public string password;
    public int max_score;
}

public class ListUserData
{
    public List<UserData> rows;
}