﻿namespace Core.Security.Models {
    public class User {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public Dictionary<string,bool> Permissions { get; set; }
    }

}
