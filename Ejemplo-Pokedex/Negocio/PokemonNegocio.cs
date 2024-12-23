﻿using System;
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
                comando.CommandText = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id From POKEMONS P, ELEMENTOS E, ELEMENTOS D where E.Id = P.IdTipo and D.Id = P.IdDebilidad And P.Activo = 1 ";
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

                throw ex;
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
        public List<Pokemon> filtrar(string campo, string criterio, string filtro)
        {
            List<Pokemon> lista = new List<Pokemon>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id From POKEMONS P, ELEMENTOS E, ELEMENTOS D where E.Id = P.IdTipo and D.Id = P.IdDebilidad And P.Activo = 1 and ";


                if (campo == "Numero")
                {
                        switch (criterio)
                        {
                            case "Mayor a":

                                consulta += "Numero > " + filtro;
                                break;
                            case "Menor a":

                                consulta += "Numero < " + filtro;
                                break;
                            default:
                                consulta += "Numero = " + filtro;
                                break;
                        }



                }
                else if (campo == "Nombre")
                {
                    
                        switch (criterio)
                        {
                            case "Comienza con":

                                consulta += "Nombre like '" +filtro +"%' ";
                                break;
                            case "Termina con":

                            consulta += "Nombre like '%" + filtro + "'";
                                break;
                            default:
                            consulta += "Nombre like '%" + filtro +"%'";
                                break;
                        }

                }
                else
                {

                    switch (criterio)
                    {
                        case "Comienza con":

                            consulta += "P.Descripcion like '" + filtro +"%' ";
                                break;
                            case "Menor a":

                                consulta += "P.Descripcion like '%" + filtro + "%'";
                                break;
                            default:
                                consulta += "P.Descripcion like '%" + filtro + "'";
                                break;
                        }
                       
                    }
                datos.Setearconsulta(consulta);
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Numero = datos.Lector.GetInt32(0);
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["UrlImagen"] is DBNull))
                        aux.UrlImagen = (string)datos.Lector["UrlImagen"];

                    aux.Tipo = new Elemento();
                    aux.Tipo.Id = (int)datos.Lector["IdTipo"];
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Id = (int)datos.Lector["IdDebilidad"];
                    aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];

                    lista.Add(aux);
                }
                    return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
      
    }
   
}
