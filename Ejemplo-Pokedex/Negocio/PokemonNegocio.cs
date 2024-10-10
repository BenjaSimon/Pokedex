using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using dominio;

namespace Negocio
{
    public class PokemonNegocio
    {
        public List<Pokemon> listar()
        {
            List<Pokemon> lista = new List<Pokemon>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;
            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=POKEDEX_DB; integrated security=true; ";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id From POKEMONS P, ELEMENTOS E, ELEMENTOS D where E.Id = P.IdTipo and D.Id = P.IdDebilidad And P.Activo = 1";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();
                while (lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)lector["Id"];
                    aux.Numero = lector.GetInt32(0);
                    aux.Nombre = (string)lector["Nombre"];
                    aux.Descripcion = (string)lector["Descripcion"];

                    if (!(lector["UrlImagen"] is DBNull))
                        aux.UrlImagen = (string)lector["UrlImagen"];

                    aux.Tipo = new Elemento();
                    aux.Tipo.Id = (int)lector["IdTipo"];
                    aux.Tipo.Descripcion = (string)lector["Tipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Id = (int)lector["IdDebilidad"];
                    aux.Debilidad.Descripcion = (string)lector["Debilidad"];

                    lista.Add(aux);
                }

                conexion.Close();
                return lista;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void agregar(Pokemon nuevo)
        {

            AccesoDatos datos = new AccesoDatos();
            datos.Setearconsulta("insert into POKEMONS (Numero, Nombre, Descripcion, Activo, IdTipo, IdDebilidad, UrlImagen)values(" + nuevo.Numero + " , '" + nuevo.Nombre + "', '" + nuevo.Descripcion + "', 1, @IdTipo, @IdDebilidad, @urlImagen)");
            datos.setearParametro("@IdTipo", nuevo.Tipo.Id);
            datos.setearParametro("@IdDebilidad", nuevo.Debilidad.Id);
            datos.setearParametro("@urlImagen", nuevo.UrlImagen);
            datos.Ejecutaraccion();
            try
            {
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                datos.Cerrarconexion();
            }
        }

        public void modificar(Pokemon poke)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.Setearconsulta("update POKEMONS set Numero = @Numero, Nombre = @Nombre, Descripcion = @Descripcion, UrlImagen = @Url, IdTipo = @IdTipo, IdDebilidad = @IdDebilidad where Id = @Id");
                datos.setearParametro("@Numero", poke.Numero);
                datos.setearParametro("@Nombre", poke.Nombre);
                datos.setearParametro("@Descripcion", poke.Descripcion);
                datos.setearParametro("@Url", poke.UrlImagen);
                datos.setearParametro("@IdTipo", poke.Tipo.Id);
                datos.setearParametro("@IdDebilidad", poke.Debilidad.Id);
                datos.setearParametro("@Id", poke.Id);

                datos.Ejecutaraccion();
            }

            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.Cerrarconexion();
               }
        }

        public void eliminarFisico(int Id)
        {
            try
            {

                AccesoDatos datos = new AccesoDatos();
                datos.Setearconsulta("delete from POKEMONS where Id = @Id");
                datos.setearParametro("@Id", Id);
                datos.Ejecutaraccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void EliminarLog(int Id)
        {

            AccesoDatos datos = new AccesoDatos();
            datos.Setearconsulta("update POKEMONS set Activo = 0 where Id = @Id");
            datos.setearParametro("@Id", Id);
            datos.Ejecutaraccion();
        }
        
      
    }
   
}
