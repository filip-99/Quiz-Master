using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UserData", menuName = "Custom/UserData")]
public class UserSO : ScriptableObject
{
    public UserData userData;

    public void SaveUserData(UserData newData)
    {
        userData = newData;
    }

    public void DeleteUserData()
    {
        userData = null;
    }
}
