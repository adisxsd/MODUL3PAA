using Npgsql;
using MODUL_PAA_2043.Helpers;

namespace MODUL_PAA_2043.Models
{
    public class PersonContext
    {
        private string __constr;
        private string __ErrorMsg;

        public PersonContext(string pConstr)
        {
            __constr = pConstr;
        }
        public List<Person> ListPerson()
        {
            List<Person> list1 = new List<Person>();
            string query = string.Format(@"SELECT id_person, nama, alamat, email FROM users.person;");
            SqlDBHelpers db = new SqlDBHelpers(this.__constr);
            try
            {
                NpgsqlCommand cmd = db.getNpgsqlCommand(query);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list1.Add(new Person()
                    {
                        id_person = int.Parse(reader["id_person"].ToString()),
                        nama = reader["nama"].ToString(),
                        alamat = reader["alamat"].ToString(),
                        email = reader["email"].ToString()
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
        public void AddPerson(Person newPerson)
        {
            string query = @"INSERT INTO person (nama, alamat, email) VALUES (@nama, @alamat, @email)";
            SqlDBHelpers db = new SqlDBHelpers(this.__constr);

            try
            {
                NpgsqlCommand cmd = db.getNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@nama", newPerson.nama);
                cmd.Parameters.AddWithValue("@alamat", newPerson.alamat);
                cmd.Parameters.AddWithValue("@email", newPerson.email);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                __ErrorMsg = ex.Message;
            }
        }

        public void UpdatePerson(Person updatePerson)
        {
            string query = @"UPDATE person 
                 SET nama = @nama, alamat = @alamat, email = @email 
                 WHERE id_person = @id_person";
            SqlDBHelpers db = new SqlDBHelpers(this.__constr);
            try
            {
                NpgsqlCommand cmd = db.getNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_person", updatePerson.id_person);
                cmd.Parameters.AddWithValue("@nama", updatePerson.nama);
                cmd.Parameters.AddWithValue("@alamat", updatePerson.alamat);
                cmd.Parameters.AddWithValue("@email", updatePerson.email);
                cmd.ExecuteNonQuery();
                cmd.Dispose(); db.closeConnection();
            }
            catch (Exception ex)
            {
                __ErrorMsg = ex.Message;
            }
        }

        public void DeletePerson(Person deletePerson)
        {
            string query = @"delete from users.person where id_person = @id_person";
            SqlDBHelpers db = new SqlDBHelpers(this.__constr);
            try
            {
                NpgsqlCommand cmd = db.getNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_person", deletePerson.id_person);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                __ErrorMsg = ex.Message;
            }
        }
    }
}
