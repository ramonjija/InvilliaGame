using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IPasswordService
    {
        string HashPassword(string password);

        bool IsPasswordValid(string password, string hashedPassword);
    }
}
