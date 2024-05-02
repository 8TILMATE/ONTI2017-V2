using ONTI2017_V2.Models;
using ONTI2017_V2.Properties;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTI2017_V2
{
    public static class DatabaseHelper
    {
        public static UserModel LoggedUser = new UserModel();
        public static List<VacanteModel> vacanteModels = new List<VacanteModel>();
        public static List<RezervareModel> rezervareModels = new List<RezervareModel>();
        public static List<UserModel> userModels = new List<UserModel>();
        public static List<RezervareModel> vacantelemele = new List<RezervareModel>();
        public static void DeleteData(string Table)
        {
            using(SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using(SqlCommand cmd = new SqlCommand("Delete From " + Table,con))
                {
                    cmd.ExecuteNonQuery();
                }
            }

        }
        public static void DeleteRezervari(string Table,int idvacanta)
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Delete From Rezervari Where (IdVacanta=@I and IdUser=@iu)", con))
                {
                    cmd.Parameters.AddWithValue("I", idvacanta);
                    cmd.Parameters.AddWithValue("iu", LoggedUser.Id);
                    cmd.ExecuteNonQuery();
                }
            }

        }
        public static void GetAllRezervariData(VacanteModel model)
        {
            using(SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Select * From Rezervari where IdVacanta=@i", con))
                {
                    cmd.Parameters.AddWithValue("i", model.Id);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            RezervareModel model1 = new RezervareModel
                            {
                                Id = rdr.GetInt32(0),
                                IdVacanta = model.Id,
                                IdUser = rdr.GetInt32(2),
                                datastart = rdr.GetDateTime(3),
                                dataend = rdr.GetDateTime(4),
                                NrPersoane = rdr.GetInt32(5),
                                PretTotal = rdr.GetInt32(6),
                            };
                            rezervareModels.Add(model1);
                            if (model1.IdUser == LoggedUser.Id)
                            {
                                vacantelemele.Add(model1);
                            }
                            

                        }
                    }
                }
            }
        }
        public static void ResetTable(string Table)
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("DBCC CHECKIDENT('" + Table+"',RESEED,0)", con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void InsertVacante(VacanteModel model)
        {
            using(SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Insert into Vacante values (@n,@d,@c,@p,@l)",con))
                {
                    cmd.Parameters.AddWithValue("n", model.NumeVacanta);
                    cmd.Parameters.AddWithValue("d", model.Descriere);
                    cmd.Parameters.AddWithValue("c", model.CaleFisier);
                    cmd.Parameters.AddWithValue("p", model.Pret);
                    cmd.Parameters.AddWithValue("l", model.NrLocuri);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void InsertRezervari(RezervareModel model)
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Insert into Rezervari values (@iv,@iu,@ds,@de,@np,@pt)", con))
                {
                    cmd.Parameters.AddWithValue("iv", model.IdVacanta);
                    cmd.Parameters.AddWithValue("iu", model.IdUser);
                    cmd.Parameters.AddWithValue("ds", model.datastart);
                    cmd.Parameters.AddWithValue("de", model.dataend);
                    cmd.Parameters.AddWithValue("np", model.NrPersoane);
                    cmd.Parameters.AddWithValue("pt", model.PretTotal);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void GetVacante()
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using(SqlCommand cmd = new SqlCommand("Select * From Vacante", con))
                {
                    using(SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while(rdr.Read())
                        {
                            VacanteModel model = new VacanteModel
                            {
                                Id = rdr.GetInt32(0),
                                NumeVacanta = rdr.GetString(1),
                                Descriere = rdr.GetString(2),
                                CaleFisier = rdr.GetString(3),
                                Pret = rdr.GetInt32(4),
                                NrLocuri = rdr.GetInt32(5)

                            };
                            vacanteModels.Add(model);
                            
                        }
                    }
                }
            }
        }
        public static void GetUsers()
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Select * From Utilizatori Where TipCont=1", con))
                {
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            UserModel model = new UserModel
                            {
                                Id = rdr.GetInt32(0),
                                Name = rdr.GetString(1),
                                LastName = rdr.GetString(2),
                               Email = rdr.GetString(3),
                                Password = rdr.GetString(4),
                                TipCont = rdr.GetInt32(5)

                            };
                            userModels.Add(model);
                        }
                    }
                }
            }
        }
        public static void UpdateUser(UserModel model)
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE UTILIZATORI SET TipCont=0 WHERE(Email=@e)", con))
                {
                    cmd.Parameters.AddWithValue("e",model.Email);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void InsertDB()
        {
            DeleteData("Vacante");
            ResetTable("Vacante");
            using(StreamReader rdr = new StreamReader(Resources.vacanteString))
            {
                while (rdr.Peek() >= 1)
                {
                    var line = rdr.ReadLine().Split('|');
                    VacanteModel model = new VacanteModel
                    {
                        NumeVacanta = line[0],
                        Descriere = line[1],
                        Pret = Int32.Parse(line[2]),
                        NrLocuri = Int32.Parse(line[3])
                    };
                    string path = String.Empty;
                    foreach(string file in Directory.GetFiles(Resources.imaginiString))
                    {
                        if (file.Contains(model.NumeVacanta))
                        {
                            path = file;
                            break;
                        }
                    }
                    if (path == string.Empty)
                    {
                        path = "C:\\Users\\rafxg\\source\\repos\\ONTI2017 V2\\ONTI2017 V2\\Resurse\\Imagini\\implicit.jpg";

                    }
                    model.CaleFisier = path;
                    DatabaseHelper.InsertVacante(model);
                }
            }
        }
        public static void InsertUser(UserModel model)
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Insert into Utilizatori values (@n,@pr,@e,@p,@t)", con))
                {
                    cmd.Parameters.AddWithValue("n", model.Name);
                    cmd.Parameters.AddWithValue("pr", model.LastName);
                    cmd.Parameters.AddWithValue("e", model.Email);
                    cmd.Parameters.AddWithValue("p", model.Password);
                    cmd.Parameters.AddWithValue("t", 1);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static bool CheckUser(string email,string password)
        {
            using(SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Select * From Utilizatori where (Email=@e and Parola = @p)", con))
                {
                    cmd.Parameters.AddWithValue("e", email);
                    cmd.Parameters.AddWithValue("p", password);
                    using(SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        try
                        {
                            rdr.Read();
                            Console.WriteLine(rdr.GetString(1));
                            LoggedUser = new UserModel
                            {
                                Id = rdr.GetInt32(0),
                                Name = rdr.GetString(1),
                                LastName = rdr.GetString(2),
                                Email = rdr.GetString(3),
                                Password = rdr.GetString(4),
                                TipCont = rdr.GetInt32(5)
                            };
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }
        }
    }
}
