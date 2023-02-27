using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Data;
using System.Data.SqlClient; 
using EmploployeedB.Models;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using EmploployeedB.Helper;
using System.Reflection.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data.Common;

namespace EmploployeedB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class EmployeeController : ControllerBase
    {


        [HttpGet]
        [Route("SPEmployees")]
        public ActionResult<IEnumerable<Employee>> Get([FromQuery] int[] Emp_id)
        {
            var result= new List<Employee>();
           
            using (SqlConnection cn = new SqlConnection(dbHelper.connectionString))
            {
                cn.Open();
                using (SqlCommand command = new SqlCommand("GetValuess", cn))
                //Name of SP;
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    var dt = new DataTable();
                    dt.Columns.Add("Emp_id" , typeof(int));
                    dt.Columns.Add("Emp_name", typeof(string));
                    dt.Columns.Add("Emp_city", typeof(string));
                    int [] arr =new int[100];
                    int i = 0,j=0;
                    foreach (var ed in Emp_id)
                    {
                        arr[i] = ed;
                        dt.Rows.Add(ed);
                        i++;
                        
                    }
                    var parameter = command.Parameters.AddWithValue("@ListId", dt);
                    parameter.SqlDbType = SqlDbType.Structured;

                    var reader = command.ExecuteReader();
                    while(reader.Read())
                    {

                        if ( Array.IndexOf(arr, (int)reader["Emp_id"] ) !=-1  && (int)reader["Emp_id"] > 0)
                        {
                            result.Add(new Employee()
                            {
                                Emp_id = int.Parse(reader["Emp_id"].ToString()!),
                                Emp_name = reader["Emp_name"].ToString()!,
                                Emp_city = reader["Emp_city"].ToString()!,
                            });

                        }
                    }
                }
            }
            return result;

        }



        [HttpGet]
        [Route("GetAllEmployees")]
        public List<Employee> GetEmployees()
        {
            List<Employee> emplist = new List<Employee>();
            string query = "select * from employee order by Emp_name ASC";
            dbHelper.cnn = new SqlConnection(dbHelper.connectionString);
            dbHelper.cnn.Open();
            dbHelper.command = new SqlCommand(query, dbHelper.cnn);
            dbHelper.dataReader = dbHelper.command.ExecuteReader(); //dataAdapter + DataFill {table} -
            while (dbHelper.dataReader.Read())
            {
                emplist.Add(new Employee()
                {

                    Emp_id = Convert.ToInt32(dbHelper.dataReader.GetValue(0)), //() tells column
                    Emp_name = Convert.ToString(dbHelper.dataReader.GetValue(1)),
                    Emp_city = Convert.ToString(dbHelper.dataReader.GetValue(2)),
                });

            }
            dbHelper.cnn.Close();
            return emplist;

        }

        private string ValidateEmployees(List<Employee> employees)
        {
            string result="";

            if(employees==null ||  employees.Count==0)
            {
                result = "Employee List empty";
                return result;
            }

            foreach(var emp in employees)
            {
                if(emp.Emp_id<=0)
                {
                    result = "Employee Id cannot be 0 ";
                    return result;
                }
                if(string.IsNullOrEmpty(emp.Emp_name.Trim()) && string.IsNullOrEmpty(emp.Emp_city.Trim()))
                {
                    result = "Employee Id cannot be Empty";
                    return result;
                }
            }

            return result;

        }

        [HttpPost]
        [Route("AddEmployee")]
        public ActionResult Post([FromBody]List<Employee> employees) //SQlClient Liberary is AWESOMEE.....Employee data
        {
            Response response = new Response();
            try
            {
                var validate = ValidateEmployees(employees); //validate Employees list passed to this method
                if (validate == "") //Data Okay hai
                {

                    foreach (var data in employees)
                    {
                        try
                        {  
                            // convert direct query to SP
                            string query = "Insert into Employee values(' " + data.Emp_id + "','" + data.Emp_name + "','" + data.Emp_city + " ')";
                            var result= dbHelper.ExecuteQuery(query);

                            //dbNonstaticHelper _dbhelperOracleEmployee = new dbNonstaticHelper("connection string for Oracle DB");
                            //dbNonstaticHelper _dbhelperTREmployee = new dbNonstaticHelper("connection string for TR dB");

                            //_dbhelperOracleEmployee.ExecuteNonQuery(query);
                            //_dbhelperTREmployee.ExecuteNonQuery(query);

                            response.IsSuccess = true;
                            response.ResponseMessage = "Employees processed successfully.";
                            return Ok(response);
                        }
                        catch(Exception ex)
                        {
                            response.IsSuccess = false;
                            response.ResponseMessage = "Smething went wrong";
                        }

                    }
                }

                else // Data Not Okay
                {
                    response.IsSuccess = false;
                    response.ResponseMessage = validate;
                    return BadRequest(response);
                }
            }
             catch (Exception) //types of exceptions to be studied   /// try catch implementation to be studied
            {
                response.IsSuccess = false;
                response.ResponseMessage = "Smething went wrong";
            }
            return BadRequest(response);
        }


        [HttpPost]
        [Route("AddEmployee_SP")]
        public ActionResult PostEmp([FromBody] List<Employee> employees) //SQlClient Liberary is AWESOMEE.....Employee data
        {
            Response response = new Response();
            try
            {
               var validate = ValidateEmployees(employees); //validate Employees list passed to this method
                if (validate == "") //Data Okay hai
                {
                    using (SqlConnection cn = new SqlConnection(dbHelper.connectionString))
                    {
                       
                       // using (SqlCommand command = new SqlCommand("get_Values", cn))
                       // {
                            foreach (var data in employees)
                            {
                                try
                                {
                                      SqlCommand command = new SqlCommand();
                                       command.Connection = cn;
                                       command.CommandText = "Sp_SaveEmployee";
                                       command.CommandType = CommandType.StoredProcedure;

                                command.Parameters.Add("@Emp_id", SqlDbType.Int).Value = data.Emp_id;
                                command.Parameters.Add("@Emp_name", SqlDbType.NVarChar, 15).Value = data.Emp_name;
                                command.Parameters.Add("@Emp_city", SqlDbType.NVarChar, 15).Value = data.Emp_city;


                                cn.Open();
                                int i = command.ExecuteNonQuery();
                                   

                                    //--------------

                                    //dbNonstaticHelper _dbhelperOracleEmployee = new dbNonstaticHelper("connection string for Oracle DB");
                                    //dbNonstaticHelper _dbhelperTREmployee = new dbNonstaticHelper("connection string for TR dB");

                                    //_dbhelperOracleEmployee.ExecuteNonQuery(query);
                                    //_dbhelperTREmployee.ExecuteNonQuery(query);
                                    if (i >= 0)
                                    {
                                        response.IsSuccess = true;
                                        response.ResponseMessage = "Employees processed successfully.";
                                        return Ok(response);

                                    }
                                    else
                                    {
                                        response.IsSuccess = false;
                                        response.ResponseMessage = "ERROR";

                                    }
                                }
                                catch (Exception ex)
                                {
                                    response.IsSuccess = false;
                                    response.ResponseMessage = "Smething went wrong";
                                }

                         //   }
                        } cn.Close();
                    }
                }

                else // Data Not Okay
                {
                    response.IsSuccess = false;
                    response.ResponseMessage = validate;
                    return BadRequest(response);
                }
            }
            catch (Exception) //types of exceptions to be studied   /// try catch implementation to be studied
            {
                response.IsSuccess = false;
                response.ResponseMessage = "Smething went wrong";
            }
            return BadRequest(response);
        }
    }
}
