﻿using Hospital.Abstracts;

namespace Hospital.Models
{
    public class Admin : User
    {
        public override string GetRoleName()
        {
            return "Administrator";
        }
    }
}
