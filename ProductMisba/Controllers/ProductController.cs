using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Security.Claims;
using System.Runtime.ConstrainedExecution;

namespace ProductMisba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public string str = "";
        SqlConnection con;
        IConfiguration IConFig;
        public ProductController(IConfiguration c)
        {
            IConFig = c;
            str = c.GetConnectionString("ConStr");
            con = new SqlConnection();
            con.ConnectionString = str;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Product_MISBA>> GetProduct()
        {
            
            SqlCommand cmd = new SqlCommand("Select * from Product_MISBA", con);
            SqlDataReader dr;
            con.Open();
            List<Product_MISBA> lst = new List<Product_MISBA>();
            Product_MISBA pdt1 = null;
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                while (dr.Read())
                {
                    pdt1 = new Product_MISBA();
                    pdt1.ProductId = System.Convert.ToInt32(dr[0]);
                    pdt1.Name = dr[1].ToString();
                    pdt1.Description = dr[2].ToString();
                    pdt1.Price = System.Convert.ToSingle(dr[3]);
                    pdt1.Category = dr[4].ToString();
                    lst.Add(pdt1);
                   
                }
            }
            else
            {
                return NotFound("Table Does not contain the any data");
            }
            return Ok(lst);
        }

        [HttpGet("get2")]
        public ActionResult<Product_MISBA> GetProductByID(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Select * from Product_MISBA where ProductId=@p1";
            cmd.Parameters.Add(new SqlParameter("p1", SqlDbType.Int));
            cmd.Parameters["p1"].Value = id;
            cmd.Connection.Open();
            SqlDataReader dr;
            Product_MISBA pdt1 = null;
            dr = cmd.ExecuteReader();
               
            if (dr.Read())
            {
                 pdt1 = new Product_MISBA();
                 pdt1.ProductId = System.Convert.ToInt32(dr[0]);
                 pdt1.Name = dr[1].ToString();
                 pdt1.Description = dr[2].ToString();
                 pdt1.Price = (float)(dr[3]);
                 pdt1.Category = dr[4].ToString();
                 return Ok(pdt1);
            }
            else
            {
                return NotFound("Table dose not have Data For " + pdt1.ProductId.ToString());
            }   
        }
       [HttpPost]
        public ActionResult<string> CreateProduct(int Id, string nm, string des, int p, string c)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Connection.Open();
            cmd.CommandText = "Insert into Product_MISBA values (@p1,@p2,@p3,@p4,@p5) ";
            cmd.Parameters.Add(new SqlParameter("p1", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("p2", SqlDbType.VarChar, 250));
            cmd.Parameters.Add(new SqlParameter("p3", SqlDbType.VarChar,250));
            cmd.Parameters.Add(new SqlParameter("p4", SqlDbType.Float));
            cmd.Parameters.Add(new SqlParameter("p5", SqlDbType.VarChar,15));
            cmd.Parameters["p1"].Value = Id;
            cmd.Parameters["p2"].Value = nm;
            cmd.Parameters["p3"].Value = des;
            cmd.Parameters["p4"].Value = p;
            cmd.Parameters["p5"].Value = c;
            int ans= cmd.ExecuteNonQuery();
            if(ans==0)
            {
                return BadRequest("The Product detais for this ProductID is already exist enter another ProductId");
            }
            else
                return Ok(ans.ToString() + " Record Created");

        }

        [HttpDelete]
        public ActionResult<string> DeleteProduct(int Id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Connection.Open();
            cmd.CommandText = "Delete from Product_MISBA where ProductId=@p1";
            cmd.Parameters.Add(new SqlParameter("p1", SqlDbType.Int));
            cmd.Parameters["p1"].Value = Id;
            int ans = cmd.ExecuteNonQuery();
            if (ans == 0)
            {
                return NotFound("The Product detais for this ProductID is not exist enter another ProductId");
            }
            else
                return Ok(ans.ToString() + " Record Deleted");

        }

        [HttpPut]
        public ActionResult<string> UpdateProduct(int Id, string nm, string des, int p, string c)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Connection.Open();
            cmd.CommandText = "Update Product_MISBA set Name=@p2,Description=@p3,Price=@p4,Category=@p5 where ProductID=@p1";
            cmd.Parameters.Add(new SqlParameter("p1", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("p2", SqlDbType.VarChar, 250));
            cmd.Parameters.Add(new SqlParameter("p3", SqlDbType.VarChar, 250));
            cmd.Parameters.Add(new SqlParameter("p4", SqlDbType.Float));
            cmd.Parameters.Add(new SqlParameter("p5", SqlDbType.VarChar, 15));
            cmd.Parameters["p1"].Value = Id;
            cmd.Parameters["p2"].Value = nm;
            cmd.Parameters["p3"].Value = des;
            cmd.Parameters["p4"].Value = p;
            cmd.Parameters["p5"].Value = c;
            int ans = cmd.ExecuteNonQuery();
            if (ans == 0)
            {
                return BadRequest("The ProductID for this detais is not exist enter the existing ProductId");
            }
            else
                return Ok(ans.ToString() + " Record Updated");

        }
    }
}
