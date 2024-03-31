using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Npgsql;
using MODUL_PAA_2043.Helpers;


namespace MODUL_PAA_2043.Models
{
    public class LoginContext
    {
        private string __constr;
        private string __ErrorMsg;

        public LoginContext(string pConstr)
        {
            __constr = pConstr;
        }

        public List<Login> Authentifikasi(string p_username, string p_password, IConfiguration p_config)
        {
            List<Login> list1 = new List<Login>();
            string query = string.Format(@"SELECT ps.id_person, ps.nama, ps.alamat, ps.email,
                    pp.id_peran, p.nama_peran
                    FROM siswa.person ps
                    INNER JOIN siswa.peran_person pp ON ps.id_person = pp.id_peran
                    INNER JOIN siswa.peran p ON pp.id_peran = p.id_peran
                    where ps.username = '{0}' and ps.password = '{1}';", p_username, p_password);
            SqlDBHelpers db = new SqlDBHelpers(this.__constr);
            try
            {
                NpgsqlCommand cmd = db.getNpgsqlCommand(query);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list1.Add(new Login()
                    {
                        id_person = int.Parse(reader["id_person"].ToString()),
                        nama = reader["nama"].ToString(),
                        alamat = reader["alamat"].ToString(),
                        email = reader["email"].ToString(),
                        id_peran = reader["id_peran"].ToString(),
                        nama_peran = reader["nama_peran"].ToString(),
                        token = GenerateJwtToken(p_username, p_password, p_config),
                    });
                }
                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                __ErrorMsg = ex.Message;
            }
            return list1;
        }

        private string GenerateJwtToken(string namaUser, string peran, IConfiguration pConfig)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(pConfig["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, namaUser),
                new Claim(ClaimTypes.Role, peran)
            };
            var token = new JwtSecurityToken(pConfig["Jwt:Issuer"],
                pConfig["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
