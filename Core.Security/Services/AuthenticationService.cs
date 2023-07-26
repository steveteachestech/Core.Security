namespace Core.Security.Services {
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Core.Security.Models;
    using Microsoft.IdentityModel.Tokens;

    public class AuthenticationService {
        public AuthenticationService()
        {
            fakePermissions = new Dictionary<string, bool>();
            fakePermissions.Add("UseApplication", true);
            fakePermissions.Add("IsReadOnly", false);

            fakePermissions2 = new Dictionary<string, bool>();
            fakePermissions2.Add("UseApplication", true);
            fakePermissions2.Add("IsReadOnly", true);

        }
        private readonly List<User> _users = new List<User>
        {
        new User { Id = 1, Username = "user1", Password = "password1",Permissions=fakePermissions},
        new User { Id = 2, Username = "user2", Password = "password2",Permissions=fakePermissions2}
        // Add more users as needed
        };

        private readonly string _secretKey = "A5C12F84DFF03C41EBCC70E839811492F8B824175F664E1121525B0609C466D5"; // Replace with your secret key
        private readonly string _issuer = "test"; // Replace with your issuer
        private readonly string _audience = "dept"; // Replace with your audience
        private static Dictionary<string, bool> fakePermissions;
        private static Dictionary<string, bool> fakePermissions2;

        public string Authenticate(string username, string password) {
            var user = _users.SingleOrDefault(u => u.Username == username && u.Password == password);

            // Return null if user not found
            if (user == null)
                return null;

            // Generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _issuer,
                Audience = _audience
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
